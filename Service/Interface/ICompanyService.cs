using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ICompanyService
    {
        List<Company> GetAllCompanies();
        Company GetCompanyById(int? id);
        void CreateNewCompany(Company company);
        void UpdateExistingCompany(Company company);
        void DeleteCompany(int id);
    }
}
