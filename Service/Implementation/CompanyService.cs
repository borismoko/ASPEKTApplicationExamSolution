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
    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Company> _comapanyRepository;

        public CompanyService(IRepository<Company> repository)
        {
            _comapanyRepository = repository;
        }

        public void CreateNewCompany(Company company)
        {
            _comapanyRepository.Insert(company);
        }

        public void DeleteCompany(int id)
        {
            _comapanyRepository.Delete(_comapanyRepository.Get(id));
        }

        public List<Company> GetAllCompanies()
        {
            return _comapanyRepository.GetAll().ToList();
        }

        public Company GetCompanyById(int? id)
        {
            return _comapanyRepository.Get(id);
        }

        public void UpdateExistingCompany(Company company)
        {
            _comapanyRepository.Update(company);
        }
    }
}