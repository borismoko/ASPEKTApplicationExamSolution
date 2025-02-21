using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApplicationASPEKT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly ICompanyService _companyService;
        private readonly ICountryService _countryService;

        public ContactsController(IContactService contactService, ICompanyService companyService, ICountryService countryService)
        {
            _contactService = contactService;
            _companyService = companyService;
            _countryService = countryService;
        }

        [HttpGet("filter")]
        public ActionResult<IEnumerable<Contact>> FilterContacts(int? countryId, int? companyId)
        {
            var contacts = _contactService.FilterContacts(countryId, companyId);
            return contacts;
        }

        // GET: api/Contacts
        [HttpGet]
        public ActionResult<IEnumerable<Contact>> GetContacts()
        {
            return _contactService.GetAllContacts();
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public ActionResult<Contact> GetContact(int id)
        {
            var contact = _contactService.GetContactById(id);

            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutContact(int id, Contact contact)
        {
            if (id != contact.Id)
            {
                return BadRequest();
            }

            var existingContact = _contactService.GetContactById(id);
            if (existingContact == null)
            {
                return NotFound();
            }

            existingContact.Name = contact.Name;

            _contactService.UpdateExistingContact(existingContact);
            return NoContent();
        }

        // POST: api/Contacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Contact> PostContact(Contact contact)
        {
            var existingCompany = _companyService.GetCompanyById(contact.CompanyId);
            var existingCountry = _countryService.GetCountryById(contact.CountryId);

            if (existingCompany == null || existingCountry == null)
            {
                return BadRequest("Invalid CompanyId or CountryId.");
            }

            contact.Company = existingCompany;
            contact.Country = existingCountry;
                

            _contactService.CreateNewContact(contact);
            return CreatedAtAction("GetContact", new { id = contact.Id }, contact);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        {
            var contact = _contactService.GetContactById(id);
            if (contact == null)
            {
                return NotFound();
            }

            _contactService.DeleteContact(id);

            return NoContent();
        }

        private bool ContactExists(int id)
        {
            return _contactService.GetContactById(id) != null;
        }
    }
}
