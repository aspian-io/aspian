using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.PostServices.DTOs;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;

namespace Aspian.Application.Core.PostServices
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
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


            public virtual ICollection<AttachmentDto> Attachments { get; set; }
            public Guid? ParentId { get; set; }
            public virtual ICollection<TaxonomyPostDto> TaxonomyPosts { get; set; }
            public virtual ICollection<Postmeta> Postmetas { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes!");
            }
        }
    }
}