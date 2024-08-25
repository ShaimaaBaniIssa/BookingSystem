using System;
using System.Collections.Generic;

namespace BookingSystem.Models;

public partial class Review
{
    public decimal Id { get; set; }

    public DateTime? Rdate { get; set; }

    public string? Rtext { get; set; }

    public string? Username { get; set; }

    public string? Useremail { get; set; }
}
