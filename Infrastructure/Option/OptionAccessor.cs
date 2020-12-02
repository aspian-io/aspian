using System.Collections.Generic;
using System.Threading.Tasks;
using Aspian.Application.Core.Interfaces;
using Aspian.Application.Core.OptionServices.AdminServices;
using Aspian.Domain.OptionModel;
using Aspian.Domain.SiteModel;
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

        public async Task<List<OptionmetaResult>> GetOptionValuesBySectionAsync(SectionEnum section, SiteTypeEnum siteType)
        {
            var option = await _context.Options.SingleOrDefaultAsync(x => x.Section == section && x.Site.SiteType == siteType);
            var optionmetaResult = new List<OptionmetaResult>();
            foreach (var optionmeta in option.Optionmetas)
            {
                optionmetaResult.Add(new OptionmetaResult{
                    PublicKeyName = optionmeta.PublicKeyName,
                    Key = optionmeta.Key,
                    KeyDescription = optionmeta.KeyDescription,
                    Value = optionmeta.Value,
                    ValueDescription = optionmeta.ValueDescription,
                    AdditionalInfo = optionmeta.AdditionalInfo
                });
            }

            return optionmetaResult;
        }

        public async Task<OptionmetaResult> GetOptionmetaByKeyAsync(KeyEnum key)
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

        public async Task<OptionmetaResult> GetOptionmetaByKeyDescriptionAsync(string keyDescription)
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