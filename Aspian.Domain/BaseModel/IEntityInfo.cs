namespace Aspian.Domain.BaseModel
{
    public interface IEntityInfo : IEntity, IEntityMetadata
    {
        string UserAgent { get; set; }
        string UserIPAddress { get; set; }
    }
}