using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.Errors;
using Aspian.Application.Core.TaxonomyServices.DTOs;
using Aspian.Domain.TaxonomyModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;

namespace Aspian.Application.Core.TaxonomyServices
{
    public class Details
    {
        public class Query : IRequest<TaxonomyDto>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, TaxonomyDto>
        {
            private readonly DataContext _constext;
            private readonly IMapper _mapper;

            public Handler(DataContext constext, IMapper mapper)
            {
                _mapper = mapper;
                _constext = constext;
            }

            public async Task<TaxonomyDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var taxonomy = await _constext.Taxonomies.FindAsync(request.Id);

                if (taxonomy == null)
                    throw new RestException(HttpStatusCode.NotFound, new { taxonomy = "Not found!" });

                return _mapper.Map<TaxonomyDto>(taxonomy);
            }
        }
    }
}