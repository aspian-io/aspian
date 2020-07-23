using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.OptionServices.AdminServices;
using Aspian.Domain.OptionModel;
using Aspian.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Option
{
    public class OptionAccessor : IOptionAccessor
    {
        private readonly DataContext _context;
        public OptionAccessor(DataContext context, IMapper mapper)
        {
            _context = context;
        }

        public async Task<OptionmetaResult> GetOptionByKeyAsync(KeyEnum key)
        {
            var option = await _context.Optionmetas.SingleOrDefaultAsync(x => x.Key == key);
            var optionmetaResult = new OptionmetaResult
            {
                PublicKeyName = option.PublicKeyName,
                Key = option.Key,
                KeyDescription = option.KeyDescription,
                Value = option.Value,
                ValueDescription = option.ValueDescription,
                AdditionalInfo = option.AdditionalInfo
            };

            return optionmetaResult;
        }

        public async Task<OptionmetaResult> GetOptionByKeyDescriptionAsync(string keyDescription)
        {
            var option = await _context.Optionmetas.SingleOrDefaultAsync(x => x.KeyDescription == keyDescription);
            var optionmetaResult = new OptionmetaResult
            {
                PublicKeyName = option.PublicKeyName,
                Key = option.Key,
                KeyDescription = option.KeyDescription,
                Value = option.Value,
                ValueDescription = option.ValueDescription,
                AdditionalInfo = option.AdditionalInfo
            };

            return optionmetaResult;
        }
    }
}