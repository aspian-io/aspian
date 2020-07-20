using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Aspian.Domain.CommentModel;
using Aspian.Domain.OptionModel;
using Aspian.Domain.PostModel;
using Aspian.Domain.SiteModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Domain.UserModel;
using Aspian.Domain.UserModel.Policy;
using Microsoft.AspNetCore.Identity;

namespace Aspian.Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<User> userManager)
        {
            // Seeding Users
            if (!userManager.Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        DisplayName = "Bob",
                        UserName = "bob",
                        Email = "bob@test.com",
                        Role = "Admin"
                    },
                    new User
                    {
                        DisplayName = "Tom",
                        UserName = "tom",
                        Email = "tom@test.com",
                        Role = "Member"
                    },
                    new User
                    {
                        DisplayName = "Jane",
                        UserName = "jane",
                        Email = "jane@test.com",
                        Role = "Member"
                    }
                };
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");

                    // Add Admin policy as a claim to bob claims
                    if (user.UserName == "bob")
                        await userManager.AddClaimAsync(user, new Claim(AspianClaimType.Role, AspianClaimValue.Admin));
                }
            }

            // Sedding Sites
            if (!context.Sites.Any())
            {
                var sites = new List<Site>
                {
                    new Site {
                        Id = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                        Domain = "localhost",
                        Path = "/",
                        Registered = DateTime.UtcNow,
                        SiteType = SiteTypeEnum.Blog,
                        Activated = true
                    },
                    new Site {
                        Id = Guid.Parse("134D3087-EFE9-4AB6-F13C-08D80310CDEE"),
                        Domain = "localhost",
                        Path = "/store",
                        Registered = DateTime.UtcNow,
                        SiteType = SiteTypeEnum.Store,
                        Activated = true
                    },
                    new Site {
                        Id = Guid.Parse("5AE37FF3-C221-43D3-F13D-08D80310CDEE"),
                        Domain = "localhost",
                        Path = "/lms",
                        Registered = DateTime.UtcNow,
                        SiteType = SiteTypeEnum.LMS,
                        Activated = true
                    },
                    new Site {
                        Id = Guid.Parse("B56877AE-FB1E-479C-F13E-08D80310CDEE"),
                        Domain = "localhost",
                        Path = "/ehealth",
                        Registered = DateTime.UtcNow,
                        SiteType = SiteTypeEnum.eHealth,
                        Activated = true
                    }
                };

                context.Sites.AddRange(sites);
                context.SaveChanges();
            }

            // Seeding Posts
            if (!context.Posts.Any())
            {
                var posts = new List<Post>
                {
                    new Post {
                        Id = Guid.Parse("751480BA-1717-4FB6-6AA5-08D8034324CD"),
                        Title = "Post Title 1",
                        Subtitle = "Post Subtitle 1",

                        Excerpt = "Lorem ipsum dolor sit amet consectetur adipisicing elit.",

                        Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Officia quam ducimus maiores magnam explicabo vitae, suscipit veniam! Laboriosam, eaque tempora consequatur quo quaerat impedit dolorem laudantium, ipsum nemo libero sed.",

                        Slug = "post-title-1",
                        PostStatus = PostStatusEnum.Publish,
                        CommentAllowed = true,
                        ViewCount = 23,
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                    },
                    new Post {
                        Id = Guid.Parse("5D6BB51B-A118-4E05-6AA6-08D8034324CD"),
                        Title = "Post Title 2",
                        Subtitle = "Post Subtitle 2",

                        Excerpt = "Lorem ipsum dolor sit amet consectetur adipisicing elit.",

                        Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Officia quam ducimus maiores magnam explicabo vitae, suscipit veniam! Laboriosam, eaque tempora consequatur quo quaerat impedit dolorem laudantium, ipsum nemo libero sed.",

                        Slug = "post-title-2",
                        PostStatus = PostStatusEnum.Publish,
                        CommentAllowed = true,
                        ViewCount = 2,
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                    },
                    new Post {
                        Id = Guid.Parse("04F5CCA9-2CCE-47BB-6AA7-08D8034324CD"),
                        Title = "Post Title 3",
                        Subtitle = "Post Subtitle 3",

                        Excerpt = "Lorem ipsum dolor sit amet consectetur adipisicing elit.",

                        Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Officia quam ducimus maiores magnam explicabo vitae, suscipit veniam! Laboriosam, eaque tempora consequatur quo quaerat impedit dolorem laudantium, ipsum nemo libero sed.",

                        Slug = "post-title-3",
                        PostStatus = PostStatusEnum.Publish,
                        CommentAllowed = true,
                        ViewCount = 12,
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                    },
                    new Post {
                        Id = Guid.Parse("9BA88FA5-EA8B-4A69-6AA8-08D8034324CD"),
                        Title = "Post Title 4",
                        Subtitle = "Post Subtitle 4",

                        Excerpt = "Lorem ipsum dolor sit amet consectetur adipisicing elit.",

                        Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Officia quam ducimus maiores magnam explicabo vitae, suscipit veniam! Laboriosam, eaque tempora consequatur quo quaerat impedit dolorem laudantium, ipsum nemo libero sed.",

                        Slug = "post-title-4",
                        PostStatus = PostStatusEnum.Publish,
                        CommentAllowed = true,
                        ViewCount = 14,
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                    },
                    new Post {
                        Id = Guid.Parse("5F331D14-851E-4137-6AA9-08D8034324CD"),
                        Title = "Post Title 5",
                        Subtitle = "Post Subtitle 5",

                        Excerpt = "Lorem ipsum dolor sit amet consectetur adipisicing elit.",

                        Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Officia quam ducimus maiores magnam explicabo vitae, suscipit veniam! Laboriosam, eaque tempora consequatur quo quaerat impedit dolorem laudantium, ipsum nemo libero sed.",

                        Slug = "post-title-5",
                        PostStatus = PostStatusEnum.Publish,
                        CommentAllowed = true,
                        ViewCount = 16,
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                    }
                };

                context.Posts.AddRange(posts);
                context.SaveChanges();
            }

            // Seeding TermTaxonomies and Related Terms
            if (!context.Taxonomies.Any())
            {
                var taxonomies = new List<Taxonomy>
                {
                    new Taxonomy {
                        Id = Guid.Parse("D3BFEBD2-71B1-48DF-0285-08D803D1EA56"),
                        Type = TaxonomyTypeEnum.category,
                        Term = new Term {
                            Name = "Category 1",
                            Slug = "category-1",
                            TaxonomyId = Guid.Parse("D3BFEBD2-71B1-48DF-0285-08D803D1EA56")
                        },
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE")
                    },
                    new Taxonomy {
                        Id = Guid.Parse("64893B00-5F9A-4B91-0286-08D803D1EA56"),
                        Type = TaxonomyTypeEnum.category,
                        Term = new Term {
                            Name = "Category 2",
                            Slug = "category-2",
                            TaxonomyId = Guid.Parse("64893B00-5F9A-4B91-0286-08D803D1EA56")
                        },
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE")
                    },
                    new Taxonomy {
                        Id = Guid.Parse("9926607F-D704-4724-0287-08D803D1EA56"),
                        Type = TaxonomyTypeEnum.category,
                        Term = new Term {
                            Name = "Category 3",
                            Slug = "category-3",
                            TaxonomyId = Guid.Parse("9926607F-D704-4724-0287-08D803D1EA56")
                        },
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE")
                    },
                    new Taxonomy {
                        Id = Guid.Parse("A4A4FB1A-38A4-4B3B-0288-08D803D1EA56"),
                        Type = TaxonomyTypeEnum.category,
                        Term = new Term {
                            Name = "Category 4",
                            Slug = "category-4",
                            TaxonomyId = Guid.Parse("A4A4FB1A-38A4-4B3B-0288-08D803D1EA56")
                        },
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE")
                    },
                    new Taxonomy {
                        Id = Guid.Parse("4C35DEFF-E65C-41EF-0289-08D803D1EA56"),
                        Type = TaxonomyTypeEnum.tag,
                        Term = new Term {
                            Name = "Tag_1",
                            Slug = "tag-1",
                            TaxonomyId = Guid.Parse("4C35DEFF-E65C-41EF-0289-08D803D1EA56")
                        },
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE")
                    },
                    new Taxonomy {
                        Id = Guid.Parse("7EBB45E6-A80B-484E-028A-08D803D1EA56"),
                        Type = TaxonomyTypeEnum.tag,
                        Term = new Term {
                            Name = "Tag_2",
                            Slug = "tag-2",
                            TaxonomyId = Guid.Parse("7EBB45E6-A80B-484E-028A-08D803D1EA56")
                        },
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE")
                    },
                    new Taxonomy {
                        Id = Guid.Parse("6C1EC7B9-F7C4-43F2-028B-08D803D1EA56"),
                        Type = TaxonomyTypeEnum.tag,
                        Term = new Term {
                            Name = "Tag_3",
                            Slug = "tag-3",
                            TaxonomyId = Guid.Parse("6C1EC7B9-F7C4-43F2-028B-08D803D1EA56")
                        },
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE")
                    },
                };

                context.Taxonomies.AddRange(taxonomies);
                context.SaveChanges();
            }

            // Seeding TermPosts
            if (!context.TaxonomyPosts.Any())
            {
                var taxonomyPosts = new List<TaxonomyPost>
                {
                    new TaxonomyPost {
                        PostId = Guid.Parse("751480BA-1717-4FB6-6AA5-08D8034324CD"),
                        TaxonomyId = Guid.Parse("D3BFEBD2-71B1-48DF-0285-08D803D1EA56")
                    },
                    new TaxonomyPost {
                        PostId = Guid.Parse("5D6BB51B-A118-4E05-6AA6-08D8034324CD"),
                        TaxonomyId = Guid.Parse("D3BFEBD2-71B1-48DF-0285-08D803D1EA56")
                    },
                    new TaxonomyPost {
                        PostId = Guid.Parse("04F5CCA9-2CCE-47BB-6AA7-08D8034324CD"),
                        TaxonomyId = Guid.Parse("64893B00-5F9A-4B91-0286-08D803D1EA56")
                    },
                    new TaxonomyPost {
                        PostId = Guid.Parse("9BA88FA5-EA8B-4A69-6AA8-08D8034324CD"),
                        TaxonomyId = Guid.Parse("9926607F-D704-4724-0287-08D803D1EA56")
                    },
                    new TaxonomyPost {
                        PostId = Guid.Parse("5F331D14-851E-4137-6AA9-08D8034324CD"),
                        TaxonomyId = Guid.Parse("A4A4FB1A-38A4-4B3B-0288-08D803D1EA56")
                    },
                    new TaxonomyPost {
                        PostId = Guid.Parse("751480BA-1717-4FB6-6AA5-08D8034324CD"),
                        TaxonomyId = Guid.Parse("6C1EC7B9-F7C4-43F2-028B-08D803D1EA56")
                    },
                    new TaxonomyPost {
                        PostId = Guid.Parse("751480BA-1717-4FB6-6AA5-08D8034324CD"),
                        TaxonomyId = Guid.Parse("7EBB45E6-A80B-484E-028A-08D803D1EA56")
                    }
                };

                context.TaxonomyPosts.AddRange(taxonomyPosts);
                context.SaveChanges();
            }

            // Seeding Options
            if (!context.Options.Any())
            {
                var options = new List<Option>
                {
                    new Option {
                        Section = SectionEnum.Activity,
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                        Optionmetas = new List<Optionmeta> {
                            new Optionmeta {
                                PublicKeyName = "Logging Activities",
                                Key = KeyEnum.Activity__LoggingActivities,
                                KeyDescription = "Enable/Disable Ativity logging",
                                Value = ValueEnum.Activity__LoggingActivities_Enable,
                                ValueDescription = "Enabled",
                                DefaultValue = ValueEnum.Activity__LoggingActivities_Enable,
                                DefaultValueDescription = "Enabled"
                            },
                            new Optionmeta {
                                PublicKeyName = "Pruning Date",
                                Key = KeyEnum.Activity__PruningDate,
                                KeyDescription = "Pruning Activity logs date",
                                Value = ValueEnum.Activity__PruningDate_EveryMonth,
                                ValueDescription = "Every month",
                                DefaultValue = ValueEnum.Activity__PruningDate_EveryMonth,
                                DefaultValueDescription = "Every month"
                            },
                            new Optionmeta {
                                PublicKeyName = "Pruning",
                                Key = KeyEnum.Activity__Pruning,
                                KeyDescription = "Pruning Activity logs",
                                Value = ValueEnum.Activity__PruningActivities_Enable,
                                ValueDescription = "",
                                DefaultValue = ValueEnum.Activity__PruningActivities_Enable,
                                DefaultValueDescription = "Every month"
                            }
                        }
                    },
                    new Option {
                        Section = SectionEnum.Comment,
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                        Optionmetas = new List<Optionmeta> {
                            new Optionmeta {
                                PublicKeyName = "Comment Blog",
                                Key = KeyEnum.Comment_Blog,
                                KeyDescription = "Approved/NotApproved Blog Comments",
                                Value = ValueEnum.Comment_Approved,
                                ValueDescription = "Approved",
                                DefaultValue = ValueEnum.Comment_Approved,
                                DefaultValueDescription = "Approved"
                            }
                        }
                    },
                    new Option {
                        Section = SectionEnum.Attachment,
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                        Optionmetas = new List<Optionmeta> {
                            new Optionmeta {
                                PublicKeyName = ".mp3",
                                Key = KeyEnum.Attachment__Audio_Mp3,
                                KeyDescription = "audio/mpeg",
                                Value = ValueEnum.Attachments__Allowed,
                                ValueDescription = "Allowed",
                                DefaultValue = ValueEnum.Attachments__Allowed,
                                DefaultValueDescription = "Allowed"
                            },
                            new Optionmeta {
                                PublicKeyName = ".wma",
                                Key = KeyEnum.Attachment__Audio_Wma,
                                KeyDescription = "audio/x-ms-wma",
                                Value = ValueEnum.Attachments__NotAllowed,
                                ValueDescription = "NotAllowed",
                                DefaultValue = ValueEnum.Attachments__NotAllowed,
                                DefaultValueDescription = "NotAllowed"
                            },

                            new Optionmeta {
                                PublicKeyName = ".bmp",
                                Key = KeyEnum.Attachment__Photo_Bmp,
                                KeyDescription = "image/bmp",
                                Value = ValueEnum.Attachments__NotAllowed,
                                ValueDescription = "NotAllowed",
                                DefaultValue = ValueEnum.Attachments__NotAllowed,
                                DefaultValueDescription = "NotAllowed"
                            },
                            new Optionmeta {
                                PublicKeyName = ".gif",
                                Key = KeyEnum.Attachment__Photo_Gif,
                                KeyDescription = "image/gif",
                                Value = ValueEnum.Attachments__NotAllowed,
                                ValueDescription = "NotAllowed",
                                DefaultValue = ValueEnum.Attachments__NotAllowed,
                                DefaultValueDescription = "NotAllowed"
                            },
                            new Optionmeta {
                                PublicKeyName = ".jpg/.jpeg",
                                Key = KeyEnum.Attachment__Photo_Jpg,
                                KeyDescription = "image/jpeg",
                                Value = ValueEnum.Attachments__Allowed,
                                ValueDescription = "Allowed",
                                DefaultValue = ValueEnum.Attachments__Allowed,
                                DefaultValueDescription = "Allowed"
                            },
                            new Optionmeta {
                                PublicKeyName = ".png",
                                Key = KeyEnum.Attachment__Photo_Png,
                                KeyDescription = "image/png",
                                Value = ValueEnum.Attachments__Allowed,
                                ValueDescription = "Allowed",
                                DefaultValue = ValueEnum.Attachments__Allowed,
                                DefaultValueDescription = "Allowed"
                            },
                            new Optionmeta {
                                PublicKeyName = ".svg",
                                Key = KeyEnum.Attachment__Photo_Svg,
                                KeyDescription = "image/svg+xml",
                                Value = ValueEnum.Attachments__NotAllowed,
                                ValueDescription = "NotAllowed",
                                DefaultValue = ValueEnum.Attachments__NotAllowed,
                                DefaultValueDescription = "NotAllowed"
                            },

                            new Optionmeta {
                                PublicKeyName = ".doc",
                                Key = KeyEnum.Attachment__Text_Doc,
                                KeyDescription = "application/msword",
                                Value = ValueEnum.Attachments__Allowed,
                                ValueDescription = "Allowed",
                                DefaultValue = ValueEnum.Attachments__Allowed,
                                DefaultValueDescription = "Allowed"
                            },
                            new Optionmeta {
                                PublicKeyName = ".docx",
                                Key = KeyEnum.Attachment__Text_Docx,
                                KeyDescription = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                                Value = ValueEnum.Attachments__Allowed,
                                ValueDescription = "Allowed",
                                DefaultValue = ValueEnum.Attachments__Allowed,
                                DefaultValueDescription = "Allowed"
                            },
                            new Optionmeta {
                                PublicKeyName = ".pdf",
                                Key = KeyEnum.Attachment__Text_Pdf,
                                KeyDescription = "application/pdf",
                                Value = ValueEnum.Attachments__Allowed,
                                ValueDescription = "Allowed",
                                DefaultValue = ValueEnum.Attachments__Allowed,
                                DefaultValueDescription = "Allowed"
                            },
                            new Optionmeta {
                                PublicKeyName = ".rtf",
                                Key = KeyEnum.Attachment__Text_Rtf,
                                KeyDescription = "application/rtf",
                                Value = ValueEnum.Attachments__NotAllowed,
                                ValueDescription = "NotAllowed",
                                DefaultValue = ValueEnum.Attachments__NotAllowed,
                                DefaultValueDescription = "NotAllowed"
                            },
                            new Optionmeta {
                                PublicKeyName = ".txt",
                                Key = KeyEnum.Attachment__Text_Txt,
                                KeyDescription = "text/plain",
                                Value = ValueEnum.Attachments__NotAllowed,
                                ValueDescription = "NotAllowed",
                                DefaultValue = ValueEnum.Attachments__NotAllowed,
                                DefaultValueDescription = "NotAllowed"
                            },
                            new Optionmeta {
                                PublicKeyName = ".xls",
                                Key = KeyEnum.Attachment__Text_Xls,
                                KeyDescription = "application/vnd.ms-excel",
                                Value = ValueEnum.Attachments__Allowed,
                                ValueDescription = "Allowed",
                                DefaultValue = ValueEnum.Attachments__Allowed,
                                DefaultValueDescription = "Allowed"
                            },
                            new Optionmeta {
                                PublicKeyName = ".xlsx",
                                Key = KeyEnum.Attachment__Text_Xlsx,
                                KeyDescription = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                Value = ValueEnum.Attachments__Allowed,
                                ValueDescription = "Allowed",
                                DefaultValue = ValueEnum.Attachments__Allowed,
                                DefaultValueDescription = "Allowed"
                            },

                            new Optionmeta {
                                PublicKeyName = ".3gp",
                                Key = KeyEnum.Attachment__Video_3gp,
                                KeyDescription = "video/3gpp",
                                Value = ValueEnum.Attachments__NotAllowed,
                                ValueDescription = "NotAllowed",
                                DefaultValue = ValueEnum.Attachments__NotAllowed,
                                DefaultValueDescription = "NotAllowed"
                            },
                            new Optionmeta {
                                PublicKeyName = ".avi",
                                Key = KeyEnum.Attachment__Video_Avi,
                                KeyDescription = "video/x-msvideo",
                                Value = ValueEnum.Attachments__NotAllowed,
                                ValueDescription = "NotAllowed",
                                DefaultValue = ValueEnum.Attachments__NotAllowed,
                                DefaultValueDescription = "NotAllowed"
                            },
                            new Optionmeta {
                                PublicKeyName = ".flv",
                                Key = KeyEnum.Attachment__Video_Flv,
                                KeyDescription = "video/x-flv",
                                Value = ValueEnum.Attachments__NotAllowed,
                                ValueDescription = "NotAllowed",
                                DefaultValue = ValueEnum.Attachments__NotAllowed,
                                DefaultValueDescription = "NotAllowed"
                            },
                            new Optionmeta {
                                PublicKeyName = ".mp4",
                                Key = KeyEnum.Attachment__Video_Mp4,
                                KeyDescription = "video/mp4",
                                Value = ValueEnum.Attachments__Allowed,
                                ValueDescription = "Allowed",
                                DefaultValue = ValueEnum.Attachments__Allowed,
                                DefaultValueDescription = "Allowed"
                            },
                            new Optionmeta {
                                PublicKeyName = ".mpeg/.mpg",
                                Key = KeyEnum.Attachment__Video_Mpeg,
                                KeyDescription = "video/mpeg",
                                Value = ValueEnum.Attachments__NotAllowed,
                                ValueDescription = "NotAllowed",
                                DefaultValue = ValueEnum.Attachments__NotAllowed,
                                DefaultValueDescription = "NotAllowed"
                            },
                            new Optionmeta {
                                PublicKeyName = ".wmv",
                                Key = KeyEnum.Attachment__Video_Wmv,
                                KeyDescription = "video/x-ms-wmv",
                                Value = ValueEnum.Attachments__NotAllowed,
                                ValueDescription = "NotAllowed",
                                DefaultValue = ValueEnum.Attachments__NotAllowed,
                                DefaultValueDescription = "NotAllowed"
                            },
                            new Optionmeta {
                                PublicKeyName = ".mkv",
                                Key = KeyEnum.Attachment__Video_Mkv,
                                KeyDescription = "video/x-matroska",
                                Value = ValueEnum.Attachments__NotAllowed,
                                ValueDescription = "NotAllowed",
                                DefaultValue = ValueEnum.Attachments__NotAllowed,
                                DefaultValueDescription = "NotAllowed"
                            },

                            new Optionmeta {
                                PublicKeyName = ".zip",
                                Key = KeyEnum.Attachment__Compressed_Zip,
                                KeyDescription = "application/zip",
                                Value = ValueEnum.Attachments__Allowed,
                                ValueDescription = "Allowed",
                                DefaultValue = ValueEnum.Attachments__Allowed,
                                DefaultValueDescription = "Allowed"
                            },
                            new Optionmeta {
                                PublicKeyName = ".rar",
                                Key = KeyEnum.Attachment__Compressed_Rar,
                                KeyDescription = "application/vnd.rar",
                                Value = ValueEnum.Attachments__NotAllowed,
                                ValueDescription = "NotAllowed",
                                DefaultValue = ValueEnum.Attachments__NotAllowed,
                                DefaultValueDescription = "NotAllowed"
                            },
                            new Optionmeta {
                                PublicKeyName = ".7z",
                                Key = KeyEnum.Attachment__Compressed_7z,
                                KeyDescription = "application/x-7z-compressed",
                                Value = ValueEnum.Attachments__NotAllowed,
                                ValueDescription = "NotAllowed",
                                DefaultValue = ValueEnum.Attachments__NotAllowed,
                                DefaultValueDescription = "NotAllowed"
                            }
                        }
                    },
                };

                context.Options.AddRange(options);
                context.SaveChanges();
            }

            // Seeding comments
            if (!context.Comments.Any())
            {
                var comments = new List<Comment>
                {
                    new Comment {
                        Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Officia quam ducimus maiores magnam explicabo vitae, suscipit veniam! Laboriosam, eaque tempora consequatur quo quaerat impedit dolorem laudantium, ipsum nemo libero sed.",
                        Approved = true,
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                        PostId = Guid.Parse("751480BA-1717-4FB6-6AA5-08D8034324CD"),

                        Replies = new List<Comment> {
                            new Comment {
                                Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit.",
                                Approved = true,
                                SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                                PostId = Guid.Parse("751480BA-1717-4FB6-6AA5-08D8034324CD")
                            },
                            new Comment {
                                Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit.Lorem ipsum dolor, sit amet consectetur adipisicing elit.",
                                Approved = true,
                                SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                                PostId = Guid.Parse("751480BA-1717-4FB6-6AA5-08D8034324CD")
                            }
                        }
                    },
                    new Comment {
                        Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit.",
                        Approved = true,
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                        PostId = Guid.Parse("751480BA-1717-4FB6-6AA5-08D8034324CD"),

                        Replies = new List<Comment> {
                            new Comment {
                                Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit.Lorem ipsum dolor, sit amet consectetur adipisicing elit.",
                                Approved = true,
                                SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                                PostId = Guid.Parse("751480BA-1717-4FB6-6AA5-08D8034324CD")
                            }
                        }
                    },
                    new Comment {
                        Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Officia quam ducimus maiores magnam explicabo vitae, suscipit veniam! Laboriosam, eaque tempora consequatur quo quaerat impedit dolorem laudantium, ipsum nemo libero sed.",
                        Approved = true,
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                        PostId = Guid.Parse("04F5CCA9-2CCE-47BB-6AA7-08D8034324CD")
                    },
                    new Comment {
                        Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit.",
                        Approved = true,
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                        PostId = Guid.Parse("04F5CCA9-2CCE-47BB-6AA7-08D8034324CD"),

                        Replies = new List<Comment> {
                            new Comment {
                                Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit.",
                                Approved = true,
                                SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                                PostId = Guid.Parse("751480BA-1717-4FB6-6AA5-08D8034324CD")
                            },
                            new Comment {
                                Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit.Lorem ipsum dolor, sit amet consectetur adipisicing elit.",
                                Approved = true,
                                SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                                PostId = Guid.Parse("751480BA-1717-4FB6-6AA5-08D8034324CD")
                            },
                            new Comment {
                                Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit.",
                                Approved = true,
                                SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                                PostId = Guid.Parse("751480BA-1717-4FB6-6AA5-08D8034324CD")
                            },
                            new Comment {
                                Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit.Lorem ipsum dolor, sit amet consectetur adipisicing elit.",
                                Approved = true,
                                SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                                PostId = Guid.Parse("751480BA-1717-4FB6-6AA5-08D8034324CD")
                            }
                        }
                    },
                    new Comment {
                        Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Officia quam ducimus maiores magnam explicabo vitae, suscipit veniam! Laboriosam, eaque tempora consequatur quo quaerat impedit dolorem laudantium, ipsum nemo libero sed.",
                        Approved = true,
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                        PostId = Guid.Parse("9BA88FA5-EA8B-4A69-6AA8-08D8034324CD")
                    },
                    new Comment {
                        Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit.",
                        Approved = true,
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                        PostId = Guid.Parse("5F331D14-851E-4137-6AA9-08D8034324CD")
                    },
                    new Comment {
                        Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Officia quam ducimus maiores magnam explicabo vitae, suscipit veniam! Laboriosam, eaque tempora consequatur quo quaerat impedit dolorem laudantium, ipsum nemo libero sed.",
                        Approved = true,
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                        PostId = Guid.Parse("5F331D14-851E-4137-6AA9-08D8034324CD")
                    },
                    new Comment {
                        Content = "Lorem ipsum dolor, sit amet consectetur adipisicing elit.",
                        Approved = true,
                        SiteId = Guid.Parse("B613403D-3C49-4263-F13B-08D80310CDEE"),
                        PostId = Guid.Parse("5F331D14-851E-4137-6AA9-08D8034324CD")
                    }
                };

                context.Comments.AddRange(comments);
                context.SaveChanges();
            }
        }
    }
}