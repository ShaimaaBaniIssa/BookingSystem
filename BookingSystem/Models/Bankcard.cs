using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Bankcard
{
    public decimal Id { get; set; }

    public string Cardnumber { get; set; } = null!;

    public string Cvv { get; set; } = null!;

    public DateTime Expirydate { get; set; }

    public string Cardholdername { get; set; } = null!;

    public decimal Balance { get; set; }
    public string CardType { get; set; }
}
