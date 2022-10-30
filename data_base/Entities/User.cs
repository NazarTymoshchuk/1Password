using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data_base.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required, MaxLength(16)]
        public string Password { get; set; }
        [Required]
        public string Username { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}
