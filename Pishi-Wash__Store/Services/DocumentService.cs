using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;

namespace Pishi_Wash__Store.Services
{
    public class DocumentService
    {
        public async Task GetCheck(float OrderAmmount, float DiscountAmmount, Point PickupPoint, int OrderCode, int OrderNumber)
        {
            var doc = new Document();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            PdfWriter.GetInstance(doc, new FileStream("Document.pdf", FileMode.Create));
            doc.Open();

            BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\comic.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font = new(baseFont, 16, Font.NORMAL);
            Font Headerfont = new(baseFont, 32, Font.NORMAL);

            var content = new Paragraph("Благодарим за покупку", Headerfont);
            content.Alignment = Element.ALIGN_CENTER;
            doc.Add(content);


            doc.Add(new Paragraph(" "));

            PdfPTable table = new(2);

            table.AddCell(new PdfPCell(new Paragraph("Дата заказа:", font)));
            table.AddCell(new PdfPCell(new Paragraph(DateOnly.FromDateTime(DateTime.Now).ToString("d"), font)));

            table.AddCell(new PdfPCell(new Paragraph("Номер заказа:", font)));
            table.AddCell(new PdfPCell(new Paragraph(string.Format("{0}", OrderNumber), font)));

            PdfPTable tableOrder = new(2);
            tableOrder.AddCell(new PdfPCell(new Paragraph("Артикул", font)));
            tableOrder.AddCell(new PdfPCell(new Paragraph("Кол-во", font)));
            foreach (var item in Global.CurrentCart)
            {
                tableOrder.AddCell(new PdfPCell(new Paragraph(item.ArticleName, font)));
                tableOrder.AddCell(new PdfPCell(new Paragraph(item.Count.ToString(), font)));
            }

            table.AddCell(new PdfPCell(new Paragraph("Состав заказа:", font)));
            table.AddCell(tableOrder);

            table.AddCell(new PdfPCell(new Paragraph("Сумма заказа:", font)));
            table.AddCell(new PdfPCell(new Paragraph(string.Format("{0:C2}", OrderAmmount), font)));

            table.AddCell(new PdfPCell(new Paragraph("Сумма скидки:", font)));
            table.AddCell(new PdfPCell(new Paragraph(string.Format("{0:C2}", DiscountAmmount), font)));

            table.AddCell(new PdfPCell(new Paragraph("Пункт выдачи:", font)));
            table.AddCell(new PdfPCell(new Paragraph(string.Format("{0}, г. {1}, ул. {2}, д. {3}", 
                PickupPoint.Index, PickupPoint.City, PickupPoint.Street, PickupPoint.House), font)));

            table.AddCell(new PdfPCell(new Paragraph("Код получения:", font)));
            table.AddCell(new PdfPCell(new Paragraph(string.Format("{0}", OrderCode), font)));



            doc.Add(table);
            doc.Close();

            await Task.CompletedTask;
        }
    }
}
