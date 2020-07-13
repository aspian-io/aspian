using System;
using System.Collections.Generic;
using Aspian.Domain.TaxonomyModel;

namespace Aspian.Application.Core.PostServices.DTOs
{
    public class TaxonomyDto
    {
        public Guid Id { get; set; }
        public TaxonomyTypeEnum Type { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedById { get; set; }


        #region Navigaiton Properties
        public Guid? SiteId { get; set; }
        public Guid? ParentId { get; set; }
        public virtual TaxonomyDto Parent { get; set; }
        public virtual ICollection<TaxonomyDto> ChildTaxonomies { get; set; }
        //public virtual ICollection<TaxonomyPostDto> TaxonomyPosts { get; set; }
        public virtual TermDto Term { get; set; }
        #endregion
    }
}