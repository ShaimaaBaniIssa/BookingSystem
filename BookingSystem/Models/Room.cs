﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models;

public partial class Room
{
    public decimal Roomid { get; set; }

    public string Name { get; set; } = null!;

    public string Roomtype { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal? Price { get; set; }

    public decimal? Maxcapacity { get; set; }

    public decimal? Availabilty { get; set; }

    public decimal? Hotelid { get; set; }

    public string? Imagepath { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Hotel? Hotel { get; set; }
}
