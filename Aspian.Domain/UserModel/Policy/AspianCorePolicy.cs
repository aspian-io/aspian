namespace Aspian.Domain.UserModel.Policy
{
    public class AspianCorePolicy
    {
        public const string AdminOnlyPolicy = "AdminOnlyPolicy";
        public const string DeveloperOnlyPolicy = "DeveloperOnlyPolicy";
        // Activity Policies
        // Admin services
        public const string AdminActivityListPolicy = "AdminActivityListPolicy";

        // Attachment Policies
        // Admin services
        public const string AdminAttachmentAddPolicy = "AdminAttachmentAddPolicy";
        public const string AdminAttachmentDeletePolicy = "AdminAttachmentDeletePolicy";
        public const string AdminAttachmentDownloadPolicy = "AdminAttachmentDownloadPolicy";
        public const string AdminAttachmentGetImagePolicy = "AdminAttachmentGetImagePolicy";
        public const string AdminAttachmentListPolicy = "AdminAttachmentListPolicy";

        // Comment Policies
        // Admin services
        public const string AdminCommentApprovePolicy = "AdminCommentApprovePolicy";
        public const string AdminCommentCreatePolicy = "AdminCommentCreatePolicy";
        public const string AdminCommentDeletePolicy = "AdminCommentDeletePolicy";
        public const string AdminCommentDetailsPolicy = "AdminCommentDetailsPolicy";
        public const string AdminCommentEditPolicy = "AdminCommentEditPolicy";
        public const string AdminCommentListPolicy = "AdminCommentListPolicy";
        public const string AdminCommentUnapprovePolicy = "AdminCommentUnapprovePolicy";

        // Option Policies
        // Admin services
        public const string AdminOptionEditPolicy = "AdminOptionEditPolicy";
        public const string AdminOptionListPolicy = "AdminOptionListPolicy";
        public const string AdminOptionRestoreDefaultPolicy = "AdminOptionRestoreDefaultPolicy";

        // Post Policies
        // Admin Services
        public const string AdminPostCreatePolicy = "AdminPostCreatePolicy";
        public const string AdminPostDeletePolicy = "AdminPostDeletePolicy";
        public const string AdminPostDetailsPolicy = "AdminPostDetailsPolicy";
        public const string AdminPostEditPolicy = "AdminPostEditPolicy";
        public const string AdminPostListPolicy = "AdminPostListPolicy";

        // Site Policies
        // Admin Services
        public const string AdminSiteDetailsPolicy = "AdminSiteDetailsPolicy";
        public const string AdminSiteListPolicy = "AdminSiteListPolicy";
        public const string AdminSiteEditPolicy = "AdminSiteEditPolicy";

        // Taxonomy Policies
        // Admin services
        public const string AdminTaxonomyCreatePolicy = "AdminTaxonomyCreatePolicy";
        public const string AdminTaxonomyDeletePolicy = "AdminTaxonomyDeletePolicy";
        public const string AdminTaxonomyDetailsPolicy = "AdminTaxonomyDetailsPolicy";
        public const string AdminTaxonomyEditPolicy = "AdminTaxonomyEditPolicy";
        public const string AdminTaxonomyListPolicy = "AdminTaxonomyListPolicy";

        // User Policies
        // Admin services
        public const string AdminUserCurrentPolicy = "AdminUserCurrentPolicy";
        public const string AdminUserLockoutPolicy = "AdminUserLockoutPolicy";
        public const string AdminUserUnlockPolicy = "AdminUserUnlockPolicy";
    }
}