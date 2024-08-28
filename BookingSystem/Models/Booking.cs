using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models;

public partial class Booking
{
    public decimal Bookingid { get; set; }

    public DateTime? Checkin { get; set; }

    public DateTime? Checkout { get; set; }
    public DateTime? BookDate { get; set; }

    public decimal? Totalprice { get; set; }

    public decimal? Numberofpersons { get; set; }

    public string? Status { get; set; }

    public decimal? Roomid { get; set; }
    public decimal? Customerid { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Room? Room { get; set; }
}
