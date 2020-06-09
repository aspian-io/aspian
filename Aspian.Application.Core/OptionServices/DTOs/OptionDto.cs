using System;
using System.Collections.Generic;

namespace Aspian.Application.Core.OptionServices.DTOs
{
    public class OptionDto
    {
        public string Section { get; set; }
        public string Description { get; set; }
        public virtual SiteDto Site { get; set; }
        public virtual ICollection<OptionmetaDto> Optionmetas { get; set; }
    }
}