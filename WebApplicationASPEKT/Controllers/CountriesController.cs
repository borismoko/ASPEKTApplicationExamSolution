using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApplicationASPEKT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly IContactService _contactService;

        public CountriesController(ICountryService countryService, IContactService contactService)
        {
            _countryService = countryService;
            _contactService = contactService;
        }

        [HttpGet("{countryId}/statistics")]
        public ActionResult<Dictionary<string, int>> GetCompanyStatistics(int? countryId)
        {
            var statistics = _countryService.GetCompanyStatisticsByCountryId(countryId);
            return statistics;
        }

        // GET: api/Countries
        [HttpGet]
        public ActionResult<IEnumerable<Country>> GetCountries()
        {
            return _countryService.GetAllCountries();
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public ActionResult<Country> GetCountry(int id)
        {
            var country = _countryService.GetCountryById(id);

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutCountry(int id, Country country)
        {
            if (id != country.Id)
            {
                return BadRequest();
            }


            var existingCountry = _countryService.GetCountryById(country.Id);
            if (existingCountry == null)
            {
                return NotFound();
            }

            existingCountry.Name = country.Name;

            var contacts = _contactService.GetAllContacts();
            foreach (var contact in contacts.Where(c => c.CountryId == id))
            {
                var existingContact = _contactService.GetContactById(contact.CompanyId);
                if (existingContact == null)
                {
                    return NotFound();
                }

                existingContact.Company.Name = country.Name;

                _contactService.UpdateExistingContact(existingContact);
            }


            _countryService.UpdateExistingCountry(existingCountry);
            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Country> PostCountry(Country country)
        {
            _countryService.CreateNewCountry(country);
            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCountry(int id)
        {
            var country = _countryService.GetCountryById(id);
            if (country == null)
            {
                return NotFound();
            }

            _countryService.DeleteCountry(id);

            return NoContent();
        }

        private bool CountryExists(int id)
        {
            return _countryService.GetCountryById(id) != null;
        }
    }
}
