using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Identity
{
    public class PageCategory:BaseEntity<int>
    {        
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Code { get; set; }
        public string? Icon { get; set; } = "";
        public int Index { get; set; }
        public string? Path { get; set; } = "";
        public string? ShowType { get; set; } = "";
        /// how to display this section
    }
}
