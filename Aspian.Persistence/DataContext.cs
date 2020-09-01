using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.AttachmentModel;
using Aspian.Domain.BaseModel;
using Aspian.Domain.CommentModel;
using Aspian.Domain.OptionModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Domain.UserModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Persistence
{
    public class DataContext : IdentityDbContext<User>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DataContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;

        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentHistory> CommentHistories { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Optionmeta> Optionmetas { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Postmeta> Postmetas { get; set; }
        public DbSet<PostAttachment> PostAttachments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Attachmentmeta> Attachmentmetas { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<Termmeta> Termmetas { get; set; }
        public DbSet<Taxonomy> Taxonomies { get; set; }
        public DbSet<TaxonomyPost> TaxonomyPosts { get; set; }
        public DbSet<Usermeta> Usermetas { get; set; }
        public DbSet<UserToken> Tokens { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Model configurations
            builder.ApplyConfiguration(new SiteConfig());
            builder.ApplyConfiguration(new ActivityConfig());
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new UsermetaConfig());
            builder.ApplyConfiguration(new CommentConfig());
            builder.ApplyConfiguration(new CommentmetaConfig());
            builder.ApplyConfiguration(new CommentHistoryConfig());
            builder.ApplyConfiguration(new OptionConfig());
            builder.ApplyConfiguration(new OptionmetaConfig());
            builder.ApplyConfiguration(new PostConfig());
            builder.ApplyConfiguration(new PostmetaConfig());
            builder.ApplyConfiguration(new PostAttachmentConfig());
            builder.ApplyConfiguration(new AttachmentConfig());
            builder.ApplyConfiguration(new AttachmentmetaConfig());
            builder.ApplyConfiguration(new TermConfig());
            builder.ApplyConfiguration(new TermmetaConfig());
            builder.ApplyConfiguration(new TaxonomyConfig());
            builder.ApplyConfiguration(new TaxonomyPostConfig());
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new UsermetaConfig());
            builder.ApplyConfiguration(new UserTokenConfig());
        }


        // Adds creation and modification info before saving
        public override int SaveChanges()
        {
            AddMetaData();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AddMetaData();
            return await base.SaveChangesAsync(true, cancellationToken);
        }

        private void AddMetaData()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is IEntityMetadata && (x.State == EntityState.Added || x.State == EntityState.Modified));


            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added && entity.Entity is IEntityCreate)
                {
                    string currentUserName = null;
                    if (_httpContextAccessor.HttpContext != null)
                        currentUserName = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                    ((IEntityCreate)entity.Entity).CreatedAt = DateTime.UtcNow;

                    if (currentUserName != null)
                        ((IEntityCreate)entity.Entity).CreatedById = base.Users.SingleOrDefault(user => user.UserName == currentUserName).Id;
                }
                if (entity.State == EntityState.Modified && entity.Entity is IEntityModify)
                {
                    string currentUserName = null;
                    if (_httpContextAccessor.HttpContext != null)
                        currentUserName = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                    ((IEntityModify)entity.Entity).ModifiedAt = DateTime.UtcNow;

                    if (currentUserName != null)
                        ((IEntityModify)entity.Entity).ModifiedById = base.Users.SingleOrDefault(user => user.UserName == currentUserName).Id;
                }
                if (entity.State == EntityState.Added && entity.Entity is IEntityInfo || entity.State == EntityState.Modified && entity.Entity is IEntityInfo)
                {
                    if (_httpContextAccessor.HttpContext != null)
                    {
                        ((IEntityInfo)entity.Entity).UserIPAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                        ((IEntityInfo)entity.Entity).UserAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"];
                    }
                    else
                    {
                        ((IEntityInfo)entity.Entity).UserIPAddress = "::1";
                        ((IEntityInfo)entity.Entity).UserAgent = "Seeded Data";
                    }
                }

            }
        }

    }
}
