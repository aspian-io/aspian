using System.Collections.Generic;

namespace Aspian.Domain.UserModel.Policy
{
    public class AspianCoreClaimValue
    {
        public const string Admin = "Admin";
        public const string Developer = "Developer";
        public const string Member = "Member";

        // Activity Claims
        // Admin services
        public const string AdminActivityListClaim = "AdminActivityList";

        // Attachment Claims
        // Admin services
        public const string AdminAttachmentAddClaim = "AdminAttachmentAdd";
        public const string AdminAttachmentDeleteClaim = "AdminAttachmentDelete";
        public const string AdminAttachmentDownloadClaim = "AdminAttachmentDownload";
        public const string AdminAttachmentGetImageClaim = "AdminAttachmentGetImage";
        public const string AdminAttachmentListClaim = "AdminAttachmentList";
        public const string AdminAttachmentSettingsClaim = "AdminAttachmentSettings";

        // Comment Claims
        // Admin services
        public const string AdminCommentApproveClaim = "AdminCommentApprove";
        public const string AdminCommentCreateClaim = "AdminCommentCreate";
        public const string AdminCommentDeleteClaim = "AdminCommentDelete";
        public const string AdminCommentDetailsClaim = "AdminCommentDetails";
        public const string AdminCommentEditClaim = "AdminCommentEdit";
        public const string AdminCommentListClaim = "AdminCommentList";
        public const string AdminCommentUnapproveClaim = "AdminCommentUnapprove";

        // Option Claims
        // Admin services
        public const string AdminOptionEditClaim = "AdminOptionEdit";
        public const string AdminOptionListClaim = "AdminOptionList";
        public const string AdminOptionRestoreDefaultClaim = "AdminOptionRestoreDefault";

        // Post Claims
        // Admin Services
        public const string AdminPostCreateClaim = "AdminPostCreate";
        public const string AdminPostDeleteClaim = "AdminPostDelete";
        public const string AdminPostDetailsClaim = "AdminPostDetails";
        public const string AdminPostEditClaim = "AdminPostEdit";
        public const string AdminPostListClaim = "AdminPostList";

        // Site Claims
        // Admin Services
        public const string AdminSiteDetailsClaim = "AdminSiteDetails";
        public const string AdminSiteListClaim = "AdminSiteList";
        public const string AdminSiteEditClaim = "AdminSiteEdit";

        // Taxonomy Claims
        // Admin services
        public const string AdminTaxonomyCreateClaim = "AdminTaxonomyCreate";
        public const string AdminTaxonomyDeleteClaim = "AdminTaxonomyDelete";
        public const string AdminTaxonomyDetailsClaim = "AdminTaxonomyDetails";
        public const string AdminTaxonomyEditClaim = "AdminTaxonomyEdit";
        public const string AdminTaxonomyListClaim = "AdminTaxonomyList";

        // User Claims
        // Admin services
        public const string AdminUserCurrentClaim = "AdminUserCurrent";
        public const string AdminUserLockoutClaim = "AdminUserLockoutClaim";
        public const string AdminUserUnlockClaim = "AdminUserUnlockClaim";
    }
}