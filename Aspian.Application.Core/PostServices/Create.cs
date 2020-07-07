using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.AttachmentServices;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.Interfaces;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Persistence;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.PostServices
{
    public class Create
    {
        public class Command : IRequest
        {
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Excerpt { get; set; }
            public string Content { get; set; }
            public string Slug { get; set; }
            public PostStatusEnum PostStatus { get; set; }
            public bool CommentAllowed { get; set; }
            public int Order { get; set; }
            public int ViewCount { get; set; }
            public PostTypeEnum Type { get; set; }

            public List<Guid> AttachedFileIds { get; set; }
            public Guid? CategoryTaxonomyId { get; set; }
            public List<string> Tags { get; set; }

            public Guid? ParentId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
                RuleFor(x => x.Content).NotEmpty();
                RuleFor(x => x.PostStatus).NotNull();
                RuleFor(x => x.Type).NotNull();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly ISlugGenerator _slugGenerator;
            private readonly UrlEncoder _urlEncoder;
            public Handler(DataContext context, IMapper mapper, ISlugGenerator slugGenerator, UrlEncoder urlEncoder)
            {
                _urlEncoder = urlEncoder;
                _slugGenerator = slugGenerator;
                _mapper = mapper;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var site = _context.Sites.FirstOrDefault(x => x.SiteType == SiteTypeEnum.Blog);
                var taxonomies = new List<TermTaxonomy>();
                var categoryTaxonomy = _context.TermTaxonomies.FirstOrDefault(x => x.Id == request.CategoryTaxonomyId);
                taxonomies.Add(categoryTaxonomy);

                foreach (var tag in request.Tags)
                {
                    var termTaxonomy = _context.TermTaxonomies.FirstOrDefault(x => x.Term.Name == tag);
                    if (termTaxonomy != null)
                    {
                        taxonomies.Add(termTaxonomy);
                    }
                    else
                    {
                        var taxonomy = new TermTaxonomy
                        {
                            Taxonomy = TaxonomyEnum.tag,
                            SiteId = site.Id,
                            Term = new Term
                            {
                                Name = tag,
                                Slug = _slugGenerator.GenerateSlug(tag)
                            }
                        };
                        _context.TermTaxonomies.Add(taxonomy);
                        var taxonomyAdded = await _context.SaveChangesAsync() > 0;

                        if (taxonomyAdded)
                            taxonomies.Add(taxonomy);
                        else
                            throw new Exception("Problem saving a new tag");
                    }
                }


                var post = _mapper.Map<Post>(request);
                post.SiteId = site.Id;
                post.Slug = string.IsNullOrWhiteSpace(request.Slug) ?
                            _slugGenerator.GenerateSlug(request.Title) :
                            _urlEncoder.Encode(request.Title.Trim().Replace(" ", "-"));
                taxonomies.ForEach(t => _context.TermPosts.Add(new TermPost { Post = post, TermTaxonomy = t }));
                request.AttachedFileIds.ForEach(a => _context.Attachments.FirstOrDefault(x => x.Id == a).AttachmentOwnerPost = post);

                if (await _context.Posts.Where(x => x.Title == post.Title).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new {Title = "Title is already exists. Please change it and try again."});

                if (await _context.Posts.Where(x => x.Slug == post.Slug).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new {Slug = "Slug is already exists. Please change it and try again."});

                _context.Posts.Add(post);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes!");
            }
        }
    }
}