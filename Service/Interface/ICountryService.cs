using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ICountryService
    {
        List<Country> GetAllCountries();
        Country GetCountryById(int? id);
        void CreateNewCountry(Country country);
        void UpdateExistingCountry(Country country);
        void DeleteCountry(int id);
        Dictionary<string, int> GetCompanyStatisticsByCountryId(int? id);

    }
}
