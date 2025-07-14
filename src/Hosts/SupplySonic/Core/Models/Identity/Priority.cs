using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Identity
{
    public class Priority : BaseEntity<int>
    {
        private Priority()
        {

        }
        public string NameAr { get; private set; }
        public string NameEn { get; private set; }

        public Priority(string nameAr, string nameEn, bool isactive)
        {
            NameAr = nameAr;
            NameEn = nameEn;
            IsActive = isactive;
        }

        public void Update(string nameAr, string nameEn, bool isactive)
        {
            NameAr = nameAr;
            NameEn = nameEn;
            IsActive = isactive;
        }
    }
}
