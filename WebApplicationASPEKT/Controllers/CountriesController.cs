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
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly IContactService _contactService;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController(ICountryService countryService, IContactService contactService, ILogger<CountriesController> logger)
        {
            _countryService = countryService;
            _contactService = contactService;
            _logger = logger;
        }

        // GET: api/Countries/5/statistics
        [HttpGet("{countryId}/statistics")]
        public ActionResult<Dictionary<string, int>> GetCompanyStatistics(int? countryId)
        {
            try
            {
                _logger.LogInformation("Fetching company statistics for CountryId: {CountryId}", countryId);
                var statistics = _countryService.GetCompanyStatisticsByCountryId(countryId);
                if (statistics == null || !statistics.Any())
                {
                    _logger.LogWarning("No statistics found for CountryId: {CountryId}", countryId);
                    return NotFound();
                }

                _logger.LogInformation($"Statistics found for CountryId: {countryId}");
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching statistics for CountryId: {CountryId}", countryId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching company statistics.");
            }
        }

        // GET: api/Countries
        [HttpGet]
        public ActionResult<IEnumerable<Country>> GetCountries()
        {
            try
            {
                _logger.LogInformation("Fetching all countries.");
                var countries = _countryService.GetAllCountries();
                _logger.LogInformation($"Found {countries.Count()} countries.");
                return Ok(countries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching countries.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching countries.");
            }
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public ActionResult<Country> GetCountry(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching country with ID {id}.");
                var country = _countryService.GetCountryById(id);

                if (country == null)
                {
                    _logger.LogWarning($"Country with ID {id} not found.");
                    return NotFound();
                }

                _logger.LogInformation($"Country with ID {id} found.");
                return Ok(country);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching country with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while fetching country with ID {id}.");
            }
        }

        // PUT: api/Countries/5
        [HttpPut("{id}")]
        public IActionResult PutCountry(int id, Country country)
        {
            try
            {
                if (id != country.Id)
                {
                    _logger.LogWarning("ID mismatch in PUT request.");
                    return BadRequest();
                }

                var existingCountry = _countryService.GetCountryById(country.Id);
                if (existingCountry == null)
                {
                    _logger.LogWarning($"Country with ID {id} not found for update.");
                    return NotFound();
                }

                existingCountry.Name = country.Name;

                var contacts = _contactService.GetAllContacts();
                foreach (var contact in contacts.Where(c => c.CountryId == id))
                {
                    var existingContact = _contactService.GetContactById(contact.CompanyId);
                    if (existingContact == null)
                    {
                        _logger.LogWarning($"Contact with CompanyId {contact.CompanyId} not found.");
                        return NotFound();
                    }

                    existingContact.Company.Name = country.Name;
                    _contactService.UpdateExistingContact(existingContact);
                }

                _countryService.UpdateExistingCountry(existingCountry);
                _logger.LogInformation($"Country with ID {id} updated successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating country.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating country.");
            }
        }

        // POST: api/Countries
        [HttpPost]
        public ActionResult<Country> PostCountry(Country country)
        {
            try
            {
                _logger.LogInformation($"Creating a new country with Name {country.Name}.");
                _countryService.CreateNewCountry(country);
                _logger.LogInformation($"Country with ID {country.Id} created successfully.");
                return CreatedAtAction("GetCountry", new { id = country.Id }, country);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating country.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating country.");
            }
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCountry(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting country with ID {id}.");
                var country = _countryService.GetCountryById(id);
                if (country == null)
                {
                    _logger.LogWarning($"Country with ID {id} not found for deletion.");
                    return NotFound();
                }

                _countryService.DeleteCountry(id);
                _logger.LogInformation($"Country with ID {id} deleted successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting country with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting country with ID {id}.");
            }
        }

        private bool CountryExists(int id)
        {
            return _countryService.GetCountryById(id) != null;
        }
    }
}
