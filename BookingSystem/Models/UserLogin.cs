

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models
{
    public class UserLogin
    {
     
        public decimal Id { get; set; }
        public string? Username { get; set; }
        public string? Hashedpassword { get; set; }
        public decimal Roleid { get; set; }

        public decimal Customerid { get; set; }

        public virtual Customer Customer { get; set; } = null!;

        public virtual Role Role { get; set; } = null!;

    }
}
