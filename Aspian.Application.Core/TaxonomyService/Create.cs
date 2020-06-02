using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Domain.BaseModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;

namespace Aspian.Application.Core.TaxonomyService
{
    public class Create
    {
        public class Command : Entitymeta, IRequest
        {
            public TaxonomyEnum Taxonomy { get; set; }
            public string Description { get; set; }


            public Guid? ParentId { get; set; }
            public string TermName { get; set; }
            public string TermSlug { get; set; }
            public Guid SiteId { get; set; }
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
                var taxonomy = new TermTaxonomy {
                    Taxonomy = request.Taxonomy,
                    Description = request.Description,
                    ParentId = request.ParentId,
                    SiteId = request.SiteId,
                    Term = new Term {
                        Name = request.TermName,
                        Slug = request.TermSlug
                    }
                };

                _context.TermTaxonomies.Add(taxonomy);

                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}