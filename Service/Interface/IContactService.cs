using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IContactService
    {
        List<Contact> GetAllContacts();
        Contact GetContactById(int? id);
        void CreateNewContact(Contact contact);
        void UpdateExistingContact(Contact contact);
        void DeleteContact(int id);

        List<Contact> FilterContacts(int? countryId, int? companyId);
    }
}
