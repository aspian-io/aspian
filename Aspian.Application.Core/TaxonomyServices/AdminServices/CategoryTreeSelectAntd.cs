using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.TaxonomyServices.AdminServices.DTOs;
using Aspian.Domain.TaxonomyModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.TaxonomyServices.AdminServices
{
    public class CategoryTreeSelectAntd
    {
        public class Query : IRequest<List<AntdCategoryTreeSelectDto>> { }

        public class Handler : IRequestHandler<Query, List<AntdCategoryTreeSelectDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<AntdCategoryTreeSelectDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var categories = await _context.Taxonomies.Where(x => x.Type == TaxonomyTypeEnum.category && x.ParentId == null).ToListAsync();
                return _mapper.Map<List<AntdCategoryTreeSelectDto>>(categories);
            }
        }
    }
}