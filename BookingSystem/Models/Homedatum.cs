using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models;

public partial class Homedatum
{
    public decimal Id { get; set; }

    public string? Logopath { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Imgpath1 { get; set; }

    public string? Imgpath2 { get; set; }

    public string? Imgpath3 { get; set; }

    [NotMapped]
    public IFormFile? ImageFile1 { get; set; }
    [NotMapped]
    public IFormFile? ImageFile2 { get; set; }
    [NotMapped]
    public IFormFile? ImageFile3 { get; set; }
    [NotMapped]
    public IFormFile? Logo { get; set; }

}
