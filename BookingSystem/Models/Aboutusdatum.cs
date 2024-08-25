using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Models;

public partial class Aboutusdatum
{
    public decimal Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Imgpath1 { get; set; }
    public string? Imgpath2 { get; set; }
    public string? Imgpath3 { get; set; }
    public string? Imgpath4 { get; set; }
    public string? ImgTitle1 { get; set; }
    public string? ImgTitle2 { get; set; }
    public string? ImgTitle3 { get; set; }
    public string? ImgTitle4 { get; set; }

    [NotMapped]
    public IFormFile? ImageFile1 { get; set; }
    [NotMapped]
    public IFormFile? ImageFile2 { get; set; }
    [NotMapped]
    public IFormFile? ImageFile3 { get; set; }
    [NotMapped]
    public IFormFile? ImageFile4 { get; set; }

    public decimal? HomeId { get; set; }

    public virtual Homedatum? Homedatum { get; set; }





}
