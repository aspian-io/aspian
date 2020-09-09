using Aspian.Domain.BaseModel;

namespace Aspian.Domain.ScheduleModel
{
    public interface IScheduleOption : IEntityBase
    {
        ScheduleOptionKeyEnum Key { get; set; }
        string Value { get; set; }
    }
}

public enum ScheduleOptionKeyEnum
{
    StoppedWorking
}