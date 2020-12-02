using System.Collections.Generic;
using System.Threading.Tasks;
using Aspian.Application.Core.OptionServices;
using Aspian.Application.Core.OptionServices.AdminServices;
using Aspian.Domain.OptionModel;
using Aspian.Domain.SiteModel;

namespace Aspian.Application.Core.Interfaces
{
    public interface IOptionAccessor
    {
        /// <summary>
        /// Gets option values related to a specific option section by <paramref name="section"/>.
        /// </summary>
        /// <returns>
        /// Collection of option values related to an option section.
        /// </returns>
        /// <param name="section" >The section name which must be a SectionEnum.</param>
        /// <param name="siteType" >The site module to which options are related and must be a SiteTypeEnum.</param>
        Task<List<OptionmetaResult>> GetOptionValuesBySectionAsync(SectionEnum section, SiteTypeEnum siteType);

        /// <summary>
        /// Gets a specific option info by <paramref name="key"/>.
        /// </summary>
        /// <returns>
        /// Option key and value with its extra information.
        /// </returns>
        /// <param name="key" >The option key which must be a KeyEnum.</param>
        Task<OptionmetaResult> GetOptionmetaByKeyAsync(KeyEnum key);

        /// <summary>
        /// Gets a specific option info by <paramref name="keyDescription"/>.
        /// </summary>
        /// <returns>
        /// Option key and value with its extra information.
        /// </returns>
        /// <param name="keyDescription" >The option key description which must be string.</param>
        Task<OptionmetaResult> GetOptionmetaByKeyDescriptionAsync(string keyDescription);
    }
}