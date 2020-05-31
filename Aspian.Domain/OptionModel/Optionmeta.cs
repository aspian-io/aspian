using System;
using Aspian.Domain.BaseModel;
using Aspian.Domain.UserModel;

namespace Aspian.Domain.OptionModel
{
    public class Optionmeta : EntityBase, IOptionmeta
    {
        public KeyEnum Key { get; set; }
        public string KeyDescription { get; set; }
        public ValueEnum Value { get; set; }
        public string DefaultValue { get; set; }
        public string ValueDescription { get; set; }
        public string AdditionalInfo { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedById { get; set; }
        public User ModifiedBy { get; set; }
        public int Version { get; set; }
        public string UserAgent { get; set; }
        public string UserIPAddress { get; set; }


        #region Navigation Properties
            public Guid OptionId { get; set; }
            public virtual Option Options { get; set; }
        #endregion
    }
}