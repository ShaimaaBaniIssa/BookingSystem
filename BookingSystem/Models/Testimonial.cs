using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models;

public partial class Testimonial
{
    public decimal Testimonialid { get; set; }

    public string? Reviewtext { get; set; }

    public decimal? Hotelid { get; set; }

    public decimal? Customerid { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Hotel? Hotel { get; set; }

    public string? Status { get; set; }
}
