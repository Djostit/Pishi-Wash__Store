using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Properties;
using Paragraph = iText.Layout.Element.Paragraph;
using Table = iText.Layout.Element.Table;

namespace Pishi_Wash__Store.Services
{
    public class DocumentService
    {
        public async Task GetCheck(float OrderAmmount, float DiscountAmmount, Point PickupPoint, int OrderCode, int OrderNumber)
        {
            PdfWriter writer = new ($"Талон за {DateOnly.FromDateTime(DateTime.Now).ToString("d")}.pdf");
            PdfDocument pdf = new (writer);
            Document document = new(pdf);

  
            PdfFont comic = PdfFontFactory.CreateFont(@"C:\Windows\Fonts\comic.ttf", PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.PREFER_NOT_EMBEDDED);
           
            var content = new Paragraph("Благодарим за покупку")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(32);
            document.Add(content);

            content = new Paragraph(" ")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16);
            document.Add(content);

            Table table = new(2, true);

            table.AddCell(new Paragraph("Дата заказа:")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(16));

            table.AddCell(new Paragraph(DateOnly.FromDateTime(DateTime.Now).ToString("d"))
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(16));

            table.AddCell(new Paragraph("Номер заказа:")
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(16));

            table.AddCell(new Paragraph(string.Format("{0}", OrderNumber))
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetFont(comic)
                .SetFontSize(16));

            var tableOrder = new Table(2, false)
                .SetWidth(UnitValue.CreatePercentValue(100))
                .SetHeight(UnitValue.CreatePercentValue(100))
                .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

            tableOrder.AddCell(new Paragraph("Артикул")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            tableOrder.AddCell(new Paragraph("Кол-во")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            foreach (var item in Global.CurrentCart)
            {
                tableOrder.AddCell(new Paragraph(item.ArticleName)
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

                tableOrder.AddCell(new Paragraph(item.Count.ToString())
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));
            }

            table.AddCell(new Paragraph("Состав заказа:")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            table.AddCell(tableOrder);

            table.AddCell(new Paragraph("Сумма заказа:")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            table.AddCell(new Paragraph(string.Format("{0:C2}", OrderAmmount))
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            table.AddCell(new Paragraph("Сумма скидки:")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            table.AddCell(new Paragraph(string.Format("{0:C2}", DiscountAmmount))
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            table.AddCell(new Paragraph("Пункт выдачи:")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            table.AddCell(new Paragraph(string.Format("{0}, г. {1}, ул. {2}, д. {3}",
                PickupPoint.Index, PickupPoint.City, PickupPoint.Street, PickupPoint.House))
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            table.AddCell(new Paragraph("Код получения:")
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            table.AddCell(new Paragraph(string.Format("{0}", OrderCode))
               .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
               .SetFont(comic)
               .SetFontSize(16));

            document.Add(table);

            table.Complete();

            document.Close();

            await Task.CompletedTask;
        }
    }
}
