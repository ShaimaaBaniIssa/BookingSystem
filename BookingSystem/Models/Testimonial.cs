using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models;

public partial class Testimonial
{
    public decimal Testimonialid { get; set; }

    public string? Reviewtext { get; set; }

    public decimal? Hotelid { get; set; }

    public virtual Hotel? Hotel { get; set; }
    public string? AppUserId { get; set; }
    [ForeignKey(nameof(AppUserId))]
    public virtual AppUser? AppUser { get; set; }

    public string? Status { get; set; }
}
