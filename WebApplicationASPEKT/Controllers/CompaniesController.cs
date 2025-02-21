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
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IContactService _contactService;
        private readonly ILogger<CompaniesController> _logger;

        public CompaniesController(ICompanyService companyService, IContactService contactService, ILogger<CompaniesController> logger)
        {
            _companyService = companyService;
            _contactService = contactService;
            _logger = logger;
        }

        // GET: api/Companies
        [HttpGet]
        public ActionResult<IEnumerable<Company>> GetCompanies()
        {
            try
            {
                _logger.LogInformation("Fetching all companies.");
                var companies = _companyService.GetAllCompanies();
                _logger.LogInformation($"Found {companies.Count()} companies.");
                return Ok(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching companies.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching companies.");
            }
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public ActionResult<Company> GetCompany(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching company with ID {id}.");
                var company = _companyService.GetCompanyById(id);

                if (company == null)
                {
                    _logger.LogWarning($"Company with ID {id} not found.");
                    return NotFound();
                }

                _logger.LogInformation($"Company with ID {id} found.");
                return Ok(company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching company with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching company with ID {id}.");
            }
        }

        // PUT: api/Companies/5
        [HttpPut("{id}")]
        public IActionResult PutCompany(int id, Company company)
        {
            try
            {
                if (id != company.Id)
                {
                    _logger.LogWarning("ID mismatch in PUT request.");
                    return BadRequest();
                }

                var existingCompany = _companyService.GetCompanyById(company.Id);
                if (existingCompany == null)
                {
                    _logger.LogWarning($"Company with ID {company.Id} not found for update.");
                    return NotFound();
                }

                existingCompany.Name = company.Name;

                var contacts = _contactService.GetAllContacts();
                foreach (var contact in contacts.Where(c => c.CompanyId == id))
                {
                    var existingContact = _contactService.GetContactById(contact.CompanyId);
                    if (existingContact == null)
                    {
                        _logger.LogWarning($"Contact with CompanyId {id} not found.");
                        return NotFound();
                    }

                    existingContact.Company.Name = company.Name;
                    _contactService.UpdateExistingContact(existingContact);
                }

                _companyService.UpdateExistingCompany(existingCompany);
                _logger.LogInformation($"Company with ID {company.Id} updated successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating company.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating company.");
            }
        }

        // POST: api/Companies
        [HttpPost]
        public ActionResult<Company> PostCompany(Company company)
        {
            try
            {
                _logger.LogInformation($"Creating a new company with name {company.Name}.");
                _companyService.CreateNewCompany(company);
                _logger.LogInformation($"Company with ID {company.Id} created successfully.");
                return CreatedAtAction("GetCompany", new { id = company.Id }, company);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating company.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating company.");
            }
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCompany(int id)
        {
            try
            {
                var company = _companyService.GetCompanyById(id);
                if (company == null)
                {
                    _logger.LogWarning($"Company with ID {id} not found for deletion.");
                    return NotFound();
                }

                _companyService.DeleteCompany(id);
                _logger.LogInformation($"Company with ID {id} deleted successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting company with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting company with ID {id}.");
            }
        }

        private bool CompanyExists(int id)
        {
            return _companyService.GetCompanyById(id) != null;
        }
    }
}
