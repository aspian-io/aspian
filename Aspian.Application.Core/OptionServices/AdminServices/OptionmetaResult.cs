using Aspian.Domain.OptionModel;

namespace Aspian.Application.Core.OptionServices.AdminServices
{
    public class OptionmetaResult
    {
        public string PublicKeyName { get; set; }
        public KeyEnum Key { get; set; }
        public string KeyDescription { get; set; }
        public ValueEnum Value { get; set; }
        public string ValueDescription { get; set; }
        public string AdditionalInfo { get; set; }
    }
}