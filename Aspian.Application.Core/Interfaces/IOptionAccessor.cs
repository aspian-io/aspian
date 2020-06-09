using System.Threading.Tasks;
using Aspian.Application.Core.OptionServices;
using Aspian.Domain.OptionModel;

namespace Aspian.Application.Core.Interfaces
{
    public interface IOptionAccessor
    {
        /// <summary>
        /// Gets a specific option info by <paramref name="key"/>.
        /// </summary>
        /// <returns>
        /// Option key and value with its extra information.
        /// </returns>
        /// <param name="key" >The option key which must be a KeyEnum.</param>
        Task<OptionmetaResult> GetOptionByKeyAsync(KeyEnum key);

        /// <summary>
        /// Gets a specific option info by <paramref name="keyDescription"/>.
        /// </summary>
        /// <returns>
        /// Option key and value with its extra information.
        /// </returns>
        /// <param name="keyDescription" >The option key description which must be string.</param>
        Task<OptionmetaResult> GetOptionByKeyDescriptionAsync(string keyDescription);
    }
}