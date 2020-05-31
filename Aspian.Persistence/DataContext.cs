﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspian.Domain.ActivityModel;
using Aspian.Domain.BaseModel;
using Aspian.Domain.CommentModel;
using Aspian.Domain.OptionModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Domain.UserModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Persistence
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityOccurrence> ActivityOccurrences { get; set; }
        public DbSet<ActivityOccurrencemeta> ActivityOccurrencemetas { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentHistory> CommentHistories { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Optionmeta> Optionmetas { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Postmeta> Postmetas { get; set; }
        public DbSet<PostHistory> PostHistories { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<Termmeta> Termmetas { get; set; }
        public DbSet<TermTaxonomy> TermTaxonomies { get; set; }
        public DbSet<TermPost> TermPosts { get; set; }
        public DbSet<Usermeta> Usermetas { get; set; }


        protected override void OnModelCreating(ModelBuilder builder) 
        {
            base.OnModelCreating(builder);

            // Model configurations
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new UsermetaConfig());
            builder.ApplyConfiguration(new ActivityOccurrenceConfig());
            builder.ApplyConfiguration(new ActivityOccurrencemetaConfig());
            builder.ApplyConfiguration(new CommentConfig());
            builder.ApplyConfiguration(new CommentmetaConfig());
            builder.ApplyConfiguration(new CommentHistoryConfig());
            builder.ApplyConfiguration(new OptionConfig());
            builder.ApplyConfiguration(new OptionmetaConfig());
            builder.ApplyConfiguration(new PostConfig());
            builder.ApplyConfiguration(new PostmetaConfig());
            builder.ApplyConfiguration(new PostHistoryConfig());
            builder.ApplyConfiguration(new TermConfig());
            builder.ApplyConfiguration(new TermmetaConfig());
            builder.ApplyConfiguration(new TermTaxonomyConfig());
            builder.ApplyConfiguration(new TermPostConfig());
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new UsermetaConfig());
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
                    ((IEntityCreate)entity.Entity).CreatedDate = DateTime.UtcNow;
                } 
                else if (entity.State == EntityState.Modified && entity is IEntityModify)
                {
                    ((IEntityModify)entity.Entity).ModifiedDate = DateTime.UtcNow;
                }

            }
        }
 
    }
}
