using System;
using System.Collections.Generic;
using Aspian.Domain.TaxonomyModel;

namespace Aspian.Application.Core.PostServices.DTOs
{
    public class TaxonomyDto
    {
        public Guid Id { get; set; }
        public TaxonomyEnum Name { get; set; }
        public string Description { get; set; }


        #region Navigaiton Properties
        public Guid? ParentId { get; set; }
        public virtual TaxonomyDto Parent { get; set; }
        public virtual ICollection<TaxonomyDto> ChildTaxonomies { get; set; }
        //public virtual ICollection<TaxonomyPostDto> TaxonomyPosts { get; set; }
        public virtual TermDto Term { get; set; }
        #endregion
    }
}