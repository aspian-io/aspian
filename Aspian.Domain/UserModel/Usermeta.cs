using Aspian.Domain.BaseModel;

namespace Aspian.Domain.UserModel
{
    public class Usermeta : Entitymeta, IUsermeta
    {
        public string MetaKey { get; set; }
        public string MetaValue { get; set; }
    }
}