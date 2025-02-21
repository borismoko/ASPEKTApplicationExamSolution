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
    public class GetCompanyStatisticsByCountry : IContactService
    {
        private readonly IRepository<Contact> _contactRepository;
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<Country> _countryRepository;

        public GetCompanyStatisticsByCountry(IRepository<Contact> contactRepository, IRepository<Company> companyRepository, IRepository<Country> countryRepository)
        {
            this._contactRepository = contactRepository;
            this._companyRepository = companyRepository;
            this._countryRepository = countryRepository;
        }

        public List<Contact> FilterContacts(int? countryId, int? companyId)
        {
            var allContacts = _contactRepository.GetAll();

            return allContacts.Where(c =>
                (!companyId.HasValue || c.CompanyId == companyId) &&
                (!countryId.HasValue || c.CountryId == countryId)
            ).ToList();
        }


        public void CreateNewContact(Contact contact)
        {
            _contactRepository.Insert(contact);
        }

        public void DeleteContact(int id)
        {
            _contactRepository.Delete(_contactRepository.Get(id));
        }

        public List<Contact> GetAllContacts()
        {
            return _contactRepository.GetAll().ToList();
        }

        public Contact GetContactById(int? id)
        {
            return _contactRepository.Get(id);
        }

        public void UpdateExistingContact(Contact contact)
        {
            _contactRepository.Update(contact);
        }

    }
}
