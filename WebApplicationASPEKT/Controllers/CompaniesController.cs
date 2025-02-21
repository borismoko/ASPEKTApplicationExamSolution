using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace WebApplicationASPEKT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IContactService _contactService;

        public CompaniesController(ICompanyService companyService, IContactService contactService)
        {
            _companyService = companyService;
            _contactService = contactService;
        }



        // GET: api/Companies
        [HttpGet]
        public ActionResult<IEnumerable<Company>> GetCompanies()
        {
            return _companyService.GetAllCompanies();
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public ActionResult<Company> GetCompany(int id)
        {
            var company = _companyService.GetCompanyById(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutCompany(int id, Company company)
        {
            if (id != company.Id)
            {
                return BadRequest();
            }

            var existingCompany = _companyService.GetCompanyById(company.Id);
            if (existingCompany == null)
            {
                return NotFound();
            }

            existingCompany.Name = company.Name;

            var contacts = _contactService.GetAllContacts();
            foreach (var contact in contacts.Where(c => c.CompanyId == id))
            {
                var existingContact = _contactService.GetContactById(contact.CompanyId);
                if (existingContact == null)
                {
                    return NotFound();
                }

                existingContact.Company.Name = company.Name;

                _contactService.UpdateExistingContact(existingContact);
            }


            _companyService.UpdateExistingCompany(existingCompany);
            return NoContent();
        }

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Company> PostCompany(Company company)
        {
            _companyService.CreateNewCompany(company);
            return CreatedAtAction("GetCompany", new { id = company.Id }, company);
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCompany(int id)
        {
            var company = _companyService.GetCompanyById(id);
            if (company == null)
            {
                return NotFound();
            }

            _companyService.DeleteCompany(id);

            return NoContent();
        }

        private bool CompanyExists(int id)
        {
            return _companyService.GetCompanyById(id) != null;
        }
    }
}