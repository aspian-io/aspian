using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.PostModel
{
    public class Postmeta : Entitymeta, IPostmeta
    {
        
        public string MetaKey { get; set; }
        public string MetaValue { get; set; }

        #region Navigation Properties
            public Guid PostId { get; set; }
            public virtual Post Post { get; set; }
        #endregion
    }
}