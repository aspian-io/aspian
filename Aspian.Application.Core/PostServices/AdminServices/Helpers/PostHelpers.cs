using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspian.Domain.PostModel;
using Aspian.Domain.TaxonomyModel;
using Aspian.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Aspian.Application.Core.PostServices.AdminServices.Helpers
{
    public class HelperTDO
    {
        public List<Post> PostsEnvelope { get; set; }
        public int PostCount { get; set; }
    }
    public static class PostHelpers
    {
        /// <summary>
        /// Gets paginated, sorted and filtered list of posts.
        /// </summary>
        /// <param name="context" >An instance of DataContext which is required.</param>
        /// <param name="Limit" >Number of the posts passes through the page an it can be null.</param>
        /// <param name="Offset" >Number of the posts you want to skip before taking requested numbers of records and it can be null.</param>
        /// <param name="Field" >Name of requested column to sort.</param>
        /// <param name="Order" >Order of sorting. It can be 'ascend', 'descend' or empty</param>
        /// <param name="FilterKey" >Name of the column you want to filter.</param>
        /// <param name="FilterValue" >The value you want the posts to be filtered base on that.</param>
        /// <param name="StartDate" >Starting date for range date filter.</param>
        /// <param name="EndDate" >Ending date for range date filter.</param>
        /// <param name="StartNumber" >Starting number for slider filter.</param>
        /// <param name="EndNumber" >Ending number for slider filter.</param>
        public static async Task<HelperTDO> PaginateAndFilterAndSort(DataContext context, int? Limit, int? Offset, string Field, string Order, string FilterKey, string FilterValue, DateTime? StartDate, DateTime? EndDate, int? StartNumber, int? EndNumber)
        {
            var queryable = context.Posts.AsQueryable();
            var postCount = queryable.Count();

            List<Post> posts;

            // Sorting and pagination logic
            if (Field != null && Order != null && Order != null && FilterValue == null)
            {
                switch (Field)
                {
                    case "title":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.Title)
                                    .ToListAsync();
                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.Title)
                                    .ToListAsync();
                                break;

                            default:
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ThenByDescending(x => x.ModifiedAt)
                                    .ThenByDescending(x => x.Title)
                                    .ToListAsync();
                                break;
                        }
                        break;

                    case "postCategory":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.TaxonomyPosts.FirstOrDefault(x => x.Taxonomy.Type == TaxonomyTypeEnum.category))
                                    .ToListAsync();
                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.TaxonomyPosts.FirstOrDefault(x => x.Taxonomy.Type == TaxonomyTypeEnum.category))
                                    .ToListAsync();
                                break;

                            default:
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ThenByDescending(x => x.ModifiedAt)
                                    .ThenByDescending(x => x.Title)
                                    .ToListAsync();
                                break;
                        }
                        break;

                    case "postStatus":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.PostStatus)
                                    .ToListAsync();
                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.PostStatus)
                                    .ToListAsync();
                                break;

                            default:
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ThenByDescending(x => x.ModifiedAt)
                                    .ThenByDescending(x => x.Title)
                                    .ToListAsync();
                                break;
                        }
                        break;

                    case "postAttachments":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.PostAttachments.Count)
                                    .ToListAsync();
                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.PostAttachments.Count)
                                    .ToListAsync();
                                break;

                            default:
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ThenByDescending(x => x.ModifiedAt)
                                    .ThenByDescending(x => x.Title)
                                    .ToListAsync();
                                break;
                        }
                        break;

                    case "commentAllowed":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.CommentAllowed)
                                    .ToListAsync();
                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CommentAllowed)
                                    .ToListAsync();
                                break;

                            default:
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ThenByDescending(x => x.ModifiedAt)
                                    .ThenByDescending(x => x.Title)
                                    .ToListAsync();
                                break;
                        }
                        break;

                    case "viewCount":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.ViewCount)
                                    .ToListAsync();
                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.ViewCount)
                                    .ToListAsync();
                                break;

                            default:
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ThenByDescending(x => x.ModifiedAt)
                                    .ThenByDescending(x => x.Title)
                                    .ToListAsync();
                                break;
                        }
                        break;

                    case "postHistories":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.PostHistories.Count)
                                    .ToListAsync();
                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.PostHistories.Count)
                                    .ToListAsync();
                                break;

                            default:
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ThenByDescending(x => x.ModifiedAt)
                                    .ThenByDescending(x => x.Title)
                                    .ToListAsync();
                                break;
                        }
                        break;

                    case "comments":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.Comments.Count)
                                    .ToListAsync();
                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.Comments.Count)
                                    .ToListAsync();
                                break;

                            default:
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ThenByDescending(x => x.ModifiedAt)
                                    .ThenByDescending(x => x.Title)
                                    .ToListAsync();
                                break;
                        }
                        break;

                    case "childPosts":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.ChildPosts.Count)
                                    .ToListAsync();
                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.ChildPosts.Count)
                                    .ToListAsync();
                                break;

                            default:
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ThenByDescending(x => x.ModifiedAt)
                                    .ThenByDescending(x => x.Title)
                                    .ToListAsync();
                                break;
                        }
                        break;

                    case "createdAt":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.CreatedAt)
                                    .ToListAsync();
                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ToListAsync();
                                break;

                            default:
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ThenByDescending(x => x.ModifiedAt)
                                    .ThenByDescending(x => x.Title)
                                    .ToListAsync();
                                break;
                        }
                        break;

                    case "createdBy":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.CreatedBy)
                                    .ToListAsync();
                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedBy)
                                    .ToListAsync();
                                break;

                            default:
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ThenByDescending(x => x.ModifiedAt)
                                    .ThenByDescending(x => x.Title)
                                    .ToListAsync();
                                break;
                        }
                        break;

                    case "modifiedAt":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.ModifiedAt)
                                    .ToListAsync();
                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.ModifiedAt)
                                    .ToListAsync();
                                break;

                            default:
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ThenByDescending(x => x.ModifiedAt)
                                    .ThenByDescending(x => x.Title)
                                    .ToListAsync();
                                break;
                        }
                        break;

                    case "modifiedBy":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.ModifiedBy)
                                    .ToListAsync();
                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.ModifiedBy)
                                    .ToListAsync();
                                break;

                            default:
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ThenByDescending(x => x.ModifiedAt)
                                    .ThenByDescending(x => x.Title)
                                    .ToListAsync();
                                break;
                        }
                        break;

                    case "userAgent":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.UserAgent)
                                    .ToListAsync();
                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.UserAgent)
                                    .ToListAsync();
                                break;

                            default:
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ThenByDescending(x => x.ModifiedAt)
                                    .ThenByDescending(x => x.Title)
                                    .ToListAsync();
                                break;
                        }
                        break;

                    case "userIPAddress":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.UserIPAddress)
                                    .ToListAsync();
                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.UserIPAddress)
                                    .ToListAsync();
                                break;

                            default:
                                posts =
                                    await queryable
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ThenByDescending(x => x.ModifiedAt)
                                    .ThenByDescending(x => x.Title)
                                    .ToListAsync();
                                break;
                        }
                        break;

                    default:
                        posts =
                        await queryable
                        .Skip(Offset ?? 0)
                        .Take(Limit ?? 3)
                        .OrderByDescending(x => x.CreatedAt)
                        .ThenByDescending(x => x.ModifiedAt)
                        .ThenByDescending(x => x.Title)
                        .ToListAsync();
                        break;
                }
            }

            // Filtering and pagination logic
            else if (FilterKey != null && FilterValue != null)
            {
                switch (FilterKey)
                {
                    case "title":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Where(x => x.Title.ToLower().Contains(FilterValue.ToLower()))
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.Title)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Where(x => x.Title.ToLower().Contains(FilterValue.ToLower()))
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.Title)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            default:
                                posts =
                                    await queryable
                                    .Where(x => x.Title.ToLower().Contains(FilterValue.ToLower()))
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.Title)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;
                        }
                        break;

                    case "postCategory":
                        switch (Order)
                        {
                            case "ascend":
                                posts = await (from p in context.Posts
                                               from tp in p.TaxonomyPosts
                                               where tp.Taxonomy.Type == TaxonomyTypeEnum.category
                                               where tp.Taxonomy.Term.Name.ToLower().Contains(FilterValue.ToLower())
                                               orderby p.Title
                                               select p).ToListAsync();

                                postCount = posts.Count;

                                break;

                            case "descend":
                                posts = await (from p in context.Posts
                                               from tp in p.TaxonomyPosts
                                               where tp.Taxonomy.Type == TaxonomyTypeEnum.category
                                               where tp.Taxonomy.Term.Name.ToLower().Contains(FilterValue.ToLower())
                                               orderby p.Title descending
                                               select p).ToListAsync();

                                postCount = posts.Count;

                                break;

                            default:
                                posts = await (from p in context.Posts
                                               from tp in p.TaxonomyPosts
                                               where tp.Taxonomy.Type == TaxonomyTypeEnum.category
                                               where tp.Taxonomy.Term.Name.ToLower().Contains(FilterValue.ToLower())
                                               orderby p.Title
                                               select p).ToListAsync();

                                postCount = posts.Count;

                                break;
                        }
                        break;

                    case "createdBy":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Where(x => x.CreatedBy.UserName.ToLower().Contains(FilterValue.ToLower()))
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.CreatedBy.UserName)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Where(x => x.CreatedBy.UserName.ToLower().Contains(FilterValue.ToLower()))
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedBy.UserName)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            default:
                                posts =
                                    await queryable
                                    .Where(x => x.CreatedBy.UserName.ToLower().Contains(FilterValue.ToLower()))
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedBy.UserName)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;
                        }
                        break;

                    case "modifiedBy":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Where(x => x.ModifiedBy.UserName.ToLower().Contains(FilterValue.ToLower()))
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.ModifiedBy.UserName)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Where(x => x.ModifiedBy.UserName.ToLower().Contains(FilterValue.ToLower()))
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.ModifiedBy.UserName)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            default:
                                posts =
                                    await queryable
                                    .Where(x => x.ModifiedBy.UserName.ToLower().Contains(FilterValue.ToLower()))
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.ModifiedBy.UserName)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;
                        }
                        break;

                    case "userIPAddress":
                        switch (Order)
                        {
                            case "ascend":
                                posts = await queryable.Where(x => x.UserIPAddress == FilterValue).ToListAsync();

                                postCount = posts.Count;

                                break;

                            case "descend":
                                posts = await queryable.Where(x => x.UserIPAddress == FilterValue).ToListAsync();

                                postCount = posts.Count;

                                break;

                            default:
                                posts = await queryable.Where(x => x.UserIPAddress == FilterValue).ToListAsync();

                                postCount = posts.Count;

                                break;
                        }
                        break;

                    case "postStatus":
                        PostStatusEnum _valueToEnum = (PostStatusEnum)Enum.Parse(typeof(PostStatusEnum), FilterValue);
                        switch (Order)
                        {
                            case "ascend":

                                posts = await queryable.Where(x => x.PostStatus == _valueToEnum).ToListAsync();

                                postCount = posts.Count;

                                break;

                            case "descend":
                                posts = await queryable.Where(x => x.PostStatus == _valueToEnum).ToListAsync();

                                postCount = posts.Count;

                                break;

                            default:
                                posts = await queryable.Where(x => x.PostStatus == _valueToEnum).ToListAsync();

                                postCount = posts.Count;

                                break;
                        }
                        break;

                    case "commentAllowed":
                        bool commentAllowedFilterValue = Convert.ToBoolean(FilterValue);
                        switch (Order)
                        {
                            case "ascend":

                                posts = await queryable.Where(x => x.CommentAllowed == commentAllowedFilterValue).ToListAsync();

                                postCount = posts.Count;

                                break;

                            case "descend":
                                posts = await queryable.Where(x => x.CommentAllowed == commentAllowedFilterValue).ToListAsync();

                                postCount = posts.Count;

                                break;

                            default:
                                posts = await queryable.Where(x => x.CommentAllowed == commentAllowedFilterValue).ToListAsync();

                                postCount = posts.Count;

                                break;
                        }
                        break;

                    case "pinned":
                        bool pinFilterValue = Convert.ToBoolean(FilterValue);
                        switch (Order)
                        {
                            case "ascend":

                                posts = await queryable.Where(x => x.IsPinned == pinFilterValue).ToListAsync();

                                postCount = posts.Count;

                                break;

                            case "descend":
                                posts = await queryable.Where(x => x.IsPinned == pinFilterValue).ToListAsync();

                                postCount = posts.Count;

                                break;

                            default:
                                posts = await queryable.Where(x => x.IsPinned == pinFilterValue).ToListAsync();

                                postCount = posts.Count;

                                break;
                        }
                        break;

                    default:
                        posts =
                        await queryable
                        .Skip(Offset ?? 0)
                        .Take(Limit ?? 3)
                        .OrderByDescending(x => x.CreatedAt)
                        .ThenByDescending(x => x.ModifiedAt)
                        .ThenByDescending(x => x.Title)
                        .ToListAsync();
                        break;
                }
            }
            else if (FilterKey != null && StartDate != null && EndDate != null)
            {
                switch (FilterKey)
                {
                    case "createdAt":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Where(x => x.CreatedAt >= StartDate && x.CreatedAt <= EndDate)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.CreatedAt)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Where(x => x.CreatedAt >= StartDate && x.CreatedAt <= EndDate)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            default:
                                posts =
                                    await queryable
                                    .Where(x => x.CreatedAt >= StartDate && x.CreatedAt <= EndDate)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;
                        }
                        break;

                    case "modifiedAt":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Where(x => x.ModifiedAt >= StartDate && x.CreatedAt <= EndDate)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.ModifiedAt)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Where(x => x.ModifiedAt >= StartDate && x.CreatedAt <= EndDate)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.ModifiedAt)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            default:
                                posts =
                                    await queryable
                                    .Where(x => x.ModifiedAt >= StartDate && x.CreatedAt <= EndDate)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.ModifiedAt)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;
                        }
                        break;

                    default:
                        posts =
                        await queryable
                        .Skip(Offset ?? 0)
                        .Take(Limit ?? 3)
                        .OrderByDescending(x => x.CreatedAt)
                        .ThenByDescending(x => x.ModifiedAt)
                        .ThenByDescending(x => x.Title)
                        .ToListAsync();
                        break;

                }
            }
            else if (FilterKey != null && StartNumber != null && EndNumber != null)
            {
                switch (FilterKey)
                {
                    case "postAttachments":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Where(x => x.PostAttachments.Count >= StartNumber && x.PostAttachments.Count <= EndNumber)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.PostAttachments.Count)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Where(x => x.PostAttachments.Count >= StartNumber && x.PostAttachments.Count <= EndNumber)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.PostAttachments.Count)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            default:
                                posts =
                                    await queryable
                                    .Where(x => x.PostAttachments.Count >= StartNumber && x.PostAttachments.Count <= EndNumber)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.PostAttachments.Count)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;
                        }
                        break;

                    case "viewCount":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Where(x => x.ViewCount >= StartNumber && x.ViewCount <= EndNumber)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.ViewCount)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Where(x => x.ViewCount >= StartNumber && x.ViewCount <= EndNumber)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.ViewCount)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            default:
                                posts =
                                    await queryable
                                    .Where(x => x.ViewCount >= StartNumber && x.ViewCount <= EndNumber)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.ViewCount)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;
                        }
                        break;

                    case "postHistories":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Where(x => x.PostHistories.Count >= StartNumber && x.PostHistories.Count <= EndNumber)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.PostHistories.Count)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Where(x => x.PostHistories.Count >= StartNumber && x.PostHistories.Count <= EndNumber)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.PostHistories.Count)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            default:
                                posts =
                                    await queryable
                                    .Where(x => x.PostHistories.Count >= StartNumber && x.PostHistories.Count <= EndNumber)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.PostHistories.Count)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;
                        }
                        break;

                    case "comments":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Where(x => x.Comments.Count >= StartNumber && x.Comments.Count <= EndNumber)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.CreatedAt)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Where(x => x.Comments.Count >= StartNumber && x.Comments.Count <= EndNumber)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.Comments.Count)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            default:
                                posts =
                                    await queryable
                                    .Where(x => x.Comments.Count >= StartNumber && x.Comments.Count <= EndNumber)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.Comments.Count)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;
                        }
                        break;

                    case "childPosts":
                        switch (Order)
                        {
                            case "ascend":
                                posts =
                                    await queryable
                                    .Where(x => x.ChildPosts.Count >= StartNumber && x.ChildPosts.Count <= EndNumber)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderBy(x => x.ChildPosts.Count)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            case "descend":
                                posts =
                                    await queryable
                                    .Where(x => x.ChildPosts.Count >= StartNumber && x.ChildPosts.Count <= EndNumber)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.ChildPosts.Count)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;

                            default:
                                posts =
                                    await queryable
                                    .Where(x => x.ChildPosts.Count >= StartNumber && x.ChildPosts.Count <= EndNumber)
                                    .Skip(Offset ?? 0)
                                    .Take(Limit ?? 3)
                                    .OrderByDescending(x => x.ChildPosts.Count)
                                    .ToListAsync();

                                postCount = posts.Count;

                                break;
                        }
                        break;


                    default:
                        posts =
                        await queryable
                        .Skip(Offset ?? 0)
                        .Take(Limit ?? 3)
                        .OrderByDescending(x => x.CreatedAt)
                        .ThenByDescending(x => x.ModifiedAt)
                        .ThenByDescending(x => x.Title)
                        .ToListAsync();
                        break;

                }
            }
            else
            {
                posts =
                        await queryable
                        .Skip(Offset ?? 0)
                        .Take(Limit ?? 3)
                        .OrderByDescending(x => x.CreatedAt)
                        .ThenByDescending(x => x.ModifiedAt)
                        .ThenByDescending(x => x.Title)
                        .ToListAsync();
            }

            return new HelperTDO
            {
                PostsEnvelope = posts,
                PostCount = postCount
            };

        }
    }
}