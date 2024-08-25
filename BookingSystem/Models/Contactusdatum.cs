using System;
using System.Collections.Generic;

namespace BookingSystem.Models;

public partial class Contactusdatum
{
    public decimal Id { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Phonenumber { get; set; }

    public string? Description { get; set; }

    public string? Locationurl { get; set; }
    public decimal? HomeId { get; set; }

    public virtual Homedatum? Homedatum { get; set; }
}
