using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Company : BaseEntity
    {
        public string? Name { get; set; }

        // public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
