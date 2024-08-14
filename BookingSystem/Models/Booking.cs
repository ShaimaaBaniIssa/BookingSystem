using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models;

public partial class Booking
{
    public decimal Bookingid { get; set; }

    public DateTime? Checkin { get; set; }

    public DateTime? Checkout { get; set; }

    public decimal? Totalprice { get; set; }

    public decimal? Numberofpersons { get; set; }

    public string? Status { get; set; }

    public decimal? Roomid { get; set; }

    public virtual Room? Room { get; set; }
    public string? AppUserId { get; set; }
    [ForeignKey(nameof(AppUserId))]
    public virtual AppUser? AppUser { get; set; }
}
