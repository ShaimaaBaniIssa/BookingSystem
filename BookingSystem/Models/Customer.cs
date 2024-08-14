using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models
{
    public class Customer
    {

        public decimal Customerid { get; set; }

        public string Firstname { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phonenumber { get; set; } = null!;

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();

        public virtual ICollection<UserLogin> Userlogins { get; set; } = new List<UserLogin>();


    }
}
