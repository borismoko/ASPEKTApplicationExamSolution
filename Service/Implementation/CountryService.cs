using Domain.Models;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class CountryService : ICountryService
    {
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<Contact> _contactRepository;
        private readonly IContactService _contactService;

        public CountryService(IRepository<Country> countryRepository, IRepository<Company> companyRepository, IRepository<Contact> contactRepository, IContactService contactService)
        {
            _countryRepository = countryRepository;
            _companyRepository = companyRepository;
            _contactRepository = contactRepository;
            _contactService = contactService;
        }

        public void CreateNewCountry(Country country)
        {
            _countryRepository.Insert(country);
        }

        public Dictionary<string, int> GetCompanyStatisticsByCountryId(int? id)
        {
            var contacts = _contactRepository.GetAll();
            var companyStats = new Dictionary<string, int>();

            foreach (var contact in contacts.Where(c => c.CountryId == id))
            {
                var contactsFiltered = _contactService.FilterContacts(id, contact.CompanyId);
                companyStats[key: contact.Company.Name] = contactsFiltered.Count();
            }

            return companyStats;
        }

        public void DeleteCountry(int id)
        {
            _countryRepository.Delete(_countryRepository.Get(id));
        }

        public List<Country> GetAllCountries()
        {
            return _countryRepository.GetAll().ToList();
        }

        public Country GetCountryById(int? id)
        {
            return _countryRepository.Get(id);
        }

        public void UpdateExistingCountry(Country country)
        {
            _countryRepository.Update(country);
        }
    }
}