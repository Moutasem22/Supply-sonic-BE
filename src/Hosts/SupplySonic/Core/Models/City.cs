using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class City : BaseEntity<int>
{
    public string? NameEn { get; private set; }
    public string? NameAr { get; private set; }
    public string? NameUzbek { get; private set; }
    public string? NameRussian { get; private set; }

    public int? CountryId { get; private set; }
    public Country Country { get; private set; }

    public City(string? nameEn, string? nameAr, string? nameUzbek, string? nameRussian, int? countryId)
    {
        NameEn = nameEn;
        NameAr = nameAr;
        NameUzbek = nameUzbek;
        NameRussian = nameRussian;
        CountryId = countryId;
    }

    public void Update(string? nameEn, string? nameAr, string? nameUzbek, string? nameRussian, int? countryId)
    {
        NameEn = nameEn;
        NameAr = nameAr;
        NameUzbek = nameUzbek;
        NameRussian = nameRussian;
        CountryId = countryId;
    }
}
