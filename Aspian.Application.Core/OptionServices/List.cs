using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Application.Core.OptionServices.DTOs;
using Aspian.Domain.OptionModel;
using Aspian.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.OptionServices
{
    public class List
    {
        public class Query : IRequest<List<OptionDto>> { }

        public class Handler : IRequestHandler<Query, List<OptionDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<OptionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var options = await _context.Options.ToListAsync();

                return _mapper.Map<List<OptionDto>>(options);
            }
        }
    }
}