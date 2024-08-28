namespace BookingSystem.Utility
{
    public class Invoice
    {

        public string CustomerName { get; set; }
        public string HotelName { get; set; }
        public string RoomType { get; set; }
        public decimal RoomId { get; set; }

        public decimal? TotalPrice { get; set; }
        public DateTime? CheckIn { get; set; }

        public DateTime? CheckOut { get; set; }
        public string CardNumber { get; set; }
        public string LogoPath { get; set; }

    }
}
