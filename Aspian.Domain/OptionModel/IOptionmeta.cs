using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.OptionModel
{
    public interface IOptionmeta : IEntityBase, IEntityModify, IEntityInfo
    {
        public KeyEnum Key { get; set; }
        string KeyDescription { get; set; }
        public ValueEnum Value { get; set; }
        string DefaultValue { get; set; }
        string ValueDescription { get; set; }
        string AdditionalInfo { get; set; }

        #region Navigation Properties
        Guid OptionId { get; set; }
        Option Options { get; set; }
        #endregion
    }

    public enum KeyEnum
    {
        /// <summary>
        /// Enable/Disable Avtivity Logging
        /// </summary>
        Activity__LoggingActivities,
        /// <summary>
        /// Determines Activity pruning date
        /// </summary>
        Activity__PruningDate,


    }

    public enum ValueEnum
    {
        /// <summary>
        /// Disable Avtivity Logging
        /// </summary>
        Activity__LoggingActivities_Disable,
        /// <summary>
        /// Enable Avtivity Logging
        /// </summary>
        Activity__LoggingActivities_Enable,
        Activity__PruningDate_EveryDay,
        Activity__PruningDate_EveryWeek,
        Activity__PruningDate_EveryTwoWeeks,
        Activity__PruningDate_EveryMonth,
        Activity__PruningDate_EveryThreeMonth,
        Activity__PruningDate_EverySixMonth,
        Activity__PruningDate_EveryYear

    }
}