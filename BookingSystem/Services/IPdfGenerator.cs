using BookingSystem.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BookingSystem.Services
{
    public interface IPdfGenerator
    {
        public IDocument GetInvoice(Invoice invoice);
        
    }
}
