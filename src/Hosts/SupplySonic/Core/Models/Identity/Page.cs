
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Identity
{
    public class Page:BaseEntity<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Code { get; set; }
        public string? Icon { get; set; }
        public int Index { get; set; }
        public string Path { get; set; }
        public int? PageCategoryId { get; set; }
        public ICollection<PageAction> PageActions { get; set; }       
        public bool HasDataPermissions { get; set; }
        public bool? HasWorkflow { get; set; }
        public bool? IsWFEnabled { get; set; }
        public bool IsAdmin { get; set; }
        public Page()
        {
            PageActions = new HashSet<PageAction>();
           
        }
        public void EnableWorkflow()
        {
            this.IsWFEnabled = true;
        }

        public void DisableWorkflow()
        {
            this.IsWFEnabled = false;
        }
    }
}
