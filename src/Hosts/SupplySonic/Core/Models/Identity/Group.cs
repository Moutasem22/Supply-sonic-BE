using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Identity
{
    public class Group : BaseEntity<int>
    {
        public string NameAr { get; private set; }
        public string NameEn { get; private set; }

        public Group(string nameAr, string nameEn)
        {
            NameAr = nameAr;
            NameEn = nameEn;
        }

        public void Update(string nameAr, string nameEn)
        {
            NameAr = nameAr;
            NameEn = nameEn;
        }
    }
}
