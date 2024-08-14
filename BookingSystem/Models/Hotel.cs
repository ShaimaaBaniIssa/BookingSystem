using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models;

public partial class Hotel
{
    public decimal Hotelid { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string Phonenumber { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Imagepath { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }


    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
}
