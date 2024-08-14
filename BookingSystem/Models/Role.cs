using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models
{
    public class Role
    {

   
        public decimal Roleid { get; set; }
        public string? Rolename { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();


    }
}
