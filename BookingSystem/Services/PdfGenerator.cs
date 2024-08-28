using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using BookingSystem.Utility;
namespace BookingSystem.Services
{
    public class PdfGenerator : IPdfGenerator
    {
           
  

        public IDocument GetInvoice(Invoice invoice)
            {

            
            QuestPDF.Settings.License = LicenseType.Community;
                return Document.Create(container =>
                {
                    container
                 .Page(page =>
                 {
                     page.Size(PageSizes.A4);
                     page.Margin(2, Unit.Centimetre);
                     page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial"));

                     page.Header().Row(row =>
                     {
                         row.RelativeItem().Column(column =>
                         {
                             column.Item().Text("INVOICE").FontSize(24).SemiBold().FontColor(Colors.Brown.Medium);
                             column.Item().Text($"Date: {DateTime.Now:dd MMMM yyyy}").FontSize(12);
                         });


                         row.ConstantItem(100).Height(50).AlignRight().AlignMiddle().Image(invoice.LogoPath, ImageScaling.FitArea);
                     });

                     page.Content().Column(column =>
                     {
                         column.Spacing(10);
                         column.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);

                         column.Item().Row(row =>
                         {
                             row.RelativeItem().Text($"Billed To:").SemiBold();
                             row.RelativeItem().Text($"Hotel:").SemiBold().AlignRight();
                         });

                         column.Item().Row(row =>
                         {
                             row.RelativeItem().Text(invoice.CustomerName);
                             row.RelativeItem().Text(invoice.HotelName).AlignRight();
                         });

                         column.Item().Row(row =>
                         {
                             row.RelativeItem().Text($"Check-In: {invoice.CheckIn?.ToString("dd MMMM yyyy")}");
                             row.RelativeItem().Text($"Room Id: {invoice.RoomId}").AlignRight();
                         });

                         column.Item().Row(row =>
                         {
                             row.RelativeItem().Text($"Check-Out: {invoice.CheckOut?.ToString("dd MMMM yyyy")}");
                             row.RelativeItem().Text($"Room Type: {invoice.RoomType}").AlignRight();


                         });

                         column.Item().Text($"Card Number: {invoice.CardNumber}").FontSize(10).Italic();

                         column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);

                         column.Item().PaddingVertical(10).AlignRight().Column(innerColumn =>
                         {

                             innerColumn.Item().Text("Total Price:").FontSize(14).SemiBold().AlignRight().FontColor(Colors.Black);
                             innerColumn.Item().Text($"{invoice.TotalPrice?.ToString()} JOD").FontSize(16).Bold().AlignRight().FontColor(Colors.Red.Medium);
                         });
                     });

                     page.Footer().AlignCenter().Text(text =>
                     {
                         text.Span("Thank you for choosing ").FontSize(10);
                         text.Span($"{invoice.HotelName}.").FontSize(10).SemiBold();
                         text.Line("We are delighted to have you as our guest and appreciate your trust in us.").FontSize(10);
                     });
                 });
                });
            }



        } 
    
}
