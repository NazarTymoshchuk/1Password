using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data_base.Entities
{
    public class Account
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        [MaxLength(100)]
        public string UserName { get; set; }
        public string LinkToSite { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
