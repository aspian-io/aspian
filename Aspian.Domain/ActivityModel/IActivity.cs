using System.Collections.Generic;
using Aspian.Domain.BaseModel;

namespace Aspian.Domain.ActivityModel
{
    public interface IActivity : IEntityBase, IEntityCreate, IEntityInfo
    {
        ActivityCodeEnum Code { get; set; }
        ActivitySeverityEnum Severity { get; set; }
        ActivityObjectEnum ObjectName { get; set; }
        string Message { get; set; }

    }

    public enum ActivitySeverityEnum
    {
        Information,
        Low,
        Medium,
        High,
        Critical
    }

    public enum ActivityObjectEnum
    {
        Site = 1,
        Option = 2,
        Taxonomy = 3,
        Post = 4,
        Comment = 5,
        Attachemnt = 6,
        User = 7
    }

    public enum ActivityCodeEnum
    {
        SiteList = 1002,
        SiteDetails = 1003,

        OptionList = 2002,
        OptionEdit = 2003,
        OptionRestoreDefaultOption = 2004,
        OptionRestoreDefaultOptions = 2005,

        TaxonomyCreate = 3001,
        TaxonomyList = 3002,
        TaxonomyDetails = 3003,
        TaxonomyEdit = 3004,
        TaxonomyDelete = 3005,

        PostCreate = 4001,
        PostList = 4002,
        PostDetails = 4003,
        PostEdit = 4004,
        PostDelete = 4005,

        CommentCreate = 5001,
        CommentList = 5002,
        CommentDetails = 5003,
        CommentEdit = 5004,
        CommentDelete = 5005,
        CommentApprove = 5006,

        AttachmentAdd = 6001,
        AttachmentDownload = 6002,
        AttachmentGetImage = 6003,
        AttachmentSetMainPhoto = 6004,
        AttachmentDelete = 6005,

        UserRegister = 7001,
        UserLogin = 7002,
        UserProfile = 7003
    }
}