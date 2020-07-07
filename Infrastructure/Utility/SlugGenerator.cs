using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Aspian.Application.Core.Interfaces;

namespace Infrastructure.Utility
{
    public class SlugGenerator : ISlugGenerator
    {
        public string GenerateSlug(string phrase)
        {
            string str = RemoveDiacritics(phrase.ToLower());
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public string RemoveDiacritics(string text)
        {
            var s = new string(text.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());

            return s.Normalize(NormalizationForm.FormC);
        }
    }
}