using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class Country : BaseEntity<int>
{
    public string? NameEn { get; private set; }
    public string? NameAr { get; private set; }
    public string? NameUzbek { get; private set; }
    public string? NameRussian { get; private set; }

    public virtual ICollection<City> Cities { get;  set; }

    public Country()
    {
        Cities = new HashSet<City>();
    }

    public Country(string? nameEn, string? nameAr, string? nameUzbek, string? nameRussian)
    {
        NameEn = nameEn;
        NameAr = nameAr;
        NameUzbek = nameUzbek;
        NameRussian = nameRussian;
    }
    public void Update(string? nameEn, string? nameAr, string? nameUzbek, string? nameRussian)
    {
        NameEn = nameEn;
        NameAr = nameAr;
        NameUzbek = nameUzbek;
        NameRussian = nameRussian;
    }
}
