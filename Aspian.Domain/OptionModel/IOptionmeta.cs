using System;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.OptionModel
{
    public interface IOptionmeta : IEntityBase, IEntityModify, IEntityInfo
    {
        string PublicKeyName { get; set; }
        KeyEnum Key { get; set; }
        string KeyDescription { get; set; }
        ValueEnum Value { get; set; }
        string ValueDescription { get; set; }
        ValueEnum DefaultValue { get; set; }
        string DefaultValueDescription { get; set; }
        string AdditionalInfo { get; set; }

        #region Navigation Properties
        Guid OptionId { get; set; }
        Option Option { get; set; }
        #endregion
    }

    public enum KeyEnum
    {
        Activity__LoggingActivities,
        Activity__Pruning,
        Activity__PruningDate,

        Attachment__Multiple_Upload,
        Attachment__Upload_Auto_Proceed,
        Attachment__Upload_Max_FileSize,
        Attachment__Upload_Max_NumberOfFile,
        Attachment__Upload_Min_NumberOfFile,

        AttachmentFileType__Photo_Png,
        AttachmentFileType__Photo_Jpg,
        AttachmentFileType__Photo_Bmp,
        AttachmentFileType__Photo_Gif,
        AttachmentFileType__Photo_Svg,

        AttachmentFileType__Video_3gp,
        AttachmentFileType__Video_Avi,
        AttachmentFileType__Video_Flv,
        AttachmentFileType__Video_Wmv,
        AttachmentFileType__Video_Mp4,
        AttachmentFileType__Video_Mpeg,
        AttachmentFileType__Video_Mkv,

        AttachmentFileType__Audio_Wma,
        AttachmentFileType__Audio_Mp3,

        AttachmentFileType__Text_Pdf,

        AttachmentFileType__Text_Txt,
        AttachmentFileType__Text_Rtf,
        AttachmentFileType__Text_Doc,
        AttachmentFileType__Text_Docx,
        AttachmentFileType__Text_Xls,
        AttachmentFileType__Text_Xlsx,

        AttachmentFileType__Compressed_Zip,
        AttachmentFileType__Compressed_Rar,
        AttachmentFileType__Compressed_7z,

        Blog__Comment,
    }

    public enum ValueEnum
    {
        Activity__LoggingActivities_Disable,
        Activity__LoggingActivities_Enable,
        Activity__PruningActivities_Disable,
        Activity__PruningActivities_Enable,
        Activity__PruningDate_EveryWeek,
        Activity__PruningDate_EveryMonth,
        Activity__PruningDate_EverySixMonths,
        Activity__PruningDate_EveryYear,

        Attachment__Allowed,
        Attachment__NotAllowed,

        Attachment__Specified,
        Attachment__NotSpecified,

        AttachmentFileType__Allowed,
        AttachmentFileType__NotAllowed,

        Comment_Approved,
        Comment_NotApproved

    }
}