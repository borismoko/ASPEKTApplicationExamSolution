using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplicationASPEKT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly ICompanyService _companyService;
        private readonly ICountryService _countryService;
        private readonly ILogger<ContactsController> _logger;

        public ContactsController(IContactService contactService, ICompanyService companyService, ICountryService countryService, ILogger<ContactsController> logger)
        {
            _contactService = contactService;
            _companyService = companyService;
            _countryService = countryService;
            _logger = logger;
        }

        // GET: api/Contacts/filter
        [HttpGet("filter")]
        public ActionResult<IEnumerable<Contact>> FilterContacts(int? countryId, int? companyId)
        {
            try
            {
                _logger.LogInformation("Filtering contacts with CountryId: {CountryId}, CompanyId: {CompanyId}", countryId, companyId);
                var contacts = _contactService.FilterContacts(countryId, companyId);
                _logger.LogInformation($"Found {contacts.Count()} contacts.");
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while filtering contacts.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while filtering contacts.");
            }
        }

        // GET: api/Contacts
        [HttpGet]
        public ActionResult<IEnumerable<Contact>> GetContacts()
        {
            try
            {
                _logger.LogInformation("Fetching all contacts.");
                var contacts = _contactService.GetAllContacts();
                _logger.LogInformation($"Found {contacts.Count()} contacts.");
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching contacts.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching contacts.");
            }
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public ActionResult<Contact> GetContact(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching contact with ID {id}.");
                var contact = _contactService.GetContactById(id);

                if (contact == null)
                {
                    _logger.LogWarning($"Contact with ID {id} not found.");
                    return NotFound();
                }

                _logger.LogInformation($"Contact with ID {id} found.");
                return Ok(contact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching contact with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching contact with ID {id}.");
            }
        }

        // PUT: api/Contacts/5
        [HttpPut("{id}")]
        public IActionResult PutContact(int id, Contact contact)
        {
            try
            {
                if (id != contact.Id)
                {
                    _logger.LogWarning("ID mismatch in PUT request.");
                    return BadRequest();
                }

                var existingContact = _contactService.GetContactById(id);
                if (existingContact == null)
                {
                    _logger.LogWarning($"Contact with ID {id} not found for update.");
                    return NotFound();
                }

                existingContact.Name = contact.Name;
                _contactService.UpdateExistingContact(existingContact);
                _logger.LogInformation($"Contact with ID {id} updated successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating contact.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating contact.");
            }
        }

        // POST: api/Contacts
        [HttpPost]
        public ActionResult<Contact> PostContact(Contact contact)
        {
            try
            {
                _logger.LogInformation($"Creating a new contact with Name {contact.Name}.");

                var existingCompany = _companyService.GetCompanyById(contact.CompanyId);
                var existingCountry = _countryService.GetCountryById(contact.CountryId);

                if (existingCompany == null || existingCountry == null)
                {
                    _logger.LogWarning("Invalid CompanyId or CountryId.");
                    return BadRequest("Invalid CompanyId or CountryId.");
                }

                contact.Company = existingCompany;
                contact.Country = existingCountry;
                _contactService.CreateNewContact(contact);

                _logger.LogInformation($"Contact with ID {contact.Id} created successfully.");
                return CreatedAtAction("GetContact", new { id = contact.Id }, contact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating contact.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating contact.");
            }
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting contact with ID {id}.");
                var contact = _contactService.GetContactById(id);
                if (contact == null)
                {
                    _logger.LogWarning($"Contact with ID {id} not found for deletion.");
                    return NotFound();
                }

                _contactService.DeleteContact(id);
                _logger.LogInformation($"Contact with ID {id} deleted successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting contact with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting contact with ID {id}.");
            }
        }

        private bool ContactExists(int id)
        {
            return _contactService.GetContactById(id) != null;
        }
    }
}
