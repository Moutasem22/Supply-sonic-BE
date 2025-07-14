using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Client : BaseEntity<int>
    {
        public string NameAr { get; private set; }
        public string NameEn { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Email { get; private set; }

        public Client(string nameAr, string nameEn, string phoneNumber, string email)
        {
            NameAr = nameAr;
            NameEn = nameEn;
            PhoneNumber = phoneNumber;
            Email = email;
        }
        public void Update(string nameAr, string nameEn,string phoneNumber, string email)
        {
            NameAr = nameAr;
            NameEn = nameEn;
            PhoneNumber = phoneNumber;
            Email = email;
        }
    }
}
