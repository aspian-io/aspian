using System;
using Aspian.Domain.BaseModel;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.OptionModel
{
    public class Optionmeta : EntityBase, IOptionmeta
    {
        public string PublicKeyName { get; set; }
        public KeyEnum Key { get; set; }
        public string KeyDescription { get; set; }
        public ValueEnum Value { get; set; }
        public string ValueDescription { get; set; }
        public ValueEnum DefaultValue { get; set; }
        public string DefaultValueDescription { get; set; }
        public string AdditionalInfo { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedById { get; set; }
        public User ModifiedBy { get; set; }
        public string UserAgent { get; set; }
        public string UserIPAddress { get; set; }


        #region Navigation Properties
        public Guid OptionId { get; set; }
        public virtual Option Option { get; set; }
        #endregion
    }
}