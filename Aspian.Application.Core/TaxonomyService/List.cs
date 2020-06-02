using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Domain.TaxonomyModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.TaxonomyService
{
    public class List
    {
        public class Query : IRequest<List<TaxonomyDto>> { }

        public class Handler : IRequestHandler<Query, List<TaxonomyDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<TaxonomyDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var taxonomies = await _context.TermTaxonomies.ToListAsync();

                return _mapper.Map<List<TermTaxonomy>, List<TaxonomyDto>>(taxonomies); 
            }
        }
    }
}