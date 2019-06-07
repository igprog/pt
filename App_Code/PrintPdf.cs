using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Configuration;
using Igprog;

/// <summary>
/// PrintPdf
/// </summary>
[WebService(Namespace = "https://promo-tekstil.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class PrintPdf : System.Web.Services.WebService {
    string dataBase = ConfigurationManager.AppSettings["UserDataBase"];
    DataBase db = new DataBase();
    Files f = new Files();
    string logoPath = HttpContext.Current.Server.MapPath(string.Format("~/assets/img/lateralus_logo_2019_450px.png"));
    iTextSharp.text.pdf.draw.LineSeparator line = new iTextSharp.text.pdf.draw.LineSeparator(0f, 100f, Color.BLACK, Element.ALIGN_LEFT, 1);
    Translate t = new Translate();

    public PrintPdf() {
    }


    [WebMethod]
    public string InvoicePdf(Orders.NewOrder order, bool isForeign, string lang) {
        try {
            GetFont(8, Font.ITALIC).SetColor(255, 122, 56);
            Paragraph p = new Paragraph();
            var doc = new Document();
            string path = Server.MapPath("~/upload/invoice/temp/");
            f.DeleteFolder(path);
            f.CreateFolder(path);
            string fileName = Guid.NewGuid().ToString();
            string filePath = Path.Combine(path, string.Format("{0}.pdf", fileName));
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

            doc.Open();

            #region Header
            Image logo = Image.GetInstance(logoPath);
            logo.Alignment = Image.ALIGN_RIGHT;
            //logo.ScaleToFit(160f, 30f);
            logo.ScaleToFit(280f, 70f);
            logo.SpacingAfter = 15f;
            //logo.ScalePercent(9f);

            Admin.CompanyInfo ci = JsonConvert.DeserializeObject<Admin.CompanyInfo>(f.GetFile("json", "companyinfo"));

            p = new Paragraph();
            p.Add(new Chunk(ci.company, GetFont(12, Font.BOLD)));
            p.Alignment = Element.ALIGN_RIGHT;
            doc.Add(p);

            string info = string.Format(@"
{0}
{1} {2}, {3} {4}: {5}
IBAN: {6}
IBAN: {7}
TEL: {8}
MAIL: {9}"
, ci.companylong
, ci.zipCode, ci.city, ci.address, t.Tran("pin", lang).ToUpper(), ci.pin
, ci.iban, ci.iban1
, ci.phone
, ci.email
);

            PdfPTable header_table = new PdfPTable(2);
            header_table.AddCell(new PdfPCell(logo) { Border = PdfPCell.NO_BORDER, Padding = 0, VerticalAlignment = PdfCell.ALIGN_BOTTOM });
            header_table.AddCell(new PdfPCell(new Phrase(info, GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 0, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            header_table.WidthPercentage = 100f;
            float[] header_widths = new float[] { 1f, 1f };
            header_table.SetWidths(header_widths);
            doc.Add(header_table);

            doc.Add(new Chunk(line));
            #endregion Header

            #region Body

            PdfPTable table0 = new PdfPTable(1);
            table0.WidthPercentage = 100f;
            p = new Paragraph();
            p.Add(new Paragraph(string.Format("Predračun broj: {0}", order.number), GetFont(12, Font.BOLD)));
            table0.AddCell(new PdfPCell(p) { Border = PdfPCell.NO_BORDER, Padding = 2, MinimumHeight = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER, BackgroundColor=Color.LIGHT_GRAY });
            doc.Add(table0);
           

            PdfPTable table1 = new PdfPTable(2);
            table1.WidthPercentage = 100f;
            string client = string.Format(@"
Naziv kupca:   {0}
OIB:           {1}
Adresa:        {2}
Mjesto:        {3} {4}
Telefon:       {5}
E-mail:        {6}"
, order.companyName
, order.pin
, order.address
, order.postalCode
, order.city
, order.phone
, order.email);

            p = new Paragraph();
            p.Add(new Paragraph(client, GetFont()));
            table1.AddCell(new PdfPCell(p) { Border = PdfPCell.NO_BORDER });

            string offerInfo = string.Format(@"
Datum izdavanja:   {0}
Vrijedi do:        {1}
Valuta:            {2}
Poziv na broj:     {3}"
, DateTime.Today.ToShortDateString()
, DateTime.Today.AddDays(15).ToShortDateString()
, "kn"
, order.number.Replace('/','-'));

            p = new Paragraph();
            p.Add(new Paragraph(offerInfo, GetFont()));
            table1.AddCell(new PdfPCell(p) { Border = PdfPCell.NO_BORDER });

            doc.Add(table1);




//            string client = string.Format(@"
//{0}
//{1}
//{2} {3}
//{4}

//{5}",
//          !string.IsNullOrWhiteSpace(order.companyName) ? order.companyName : string.Format("{0} {1}", order.firstName, order.lastName),
//            order.address,
//            order.postalCode,
//            order.city,
//            order.country,
//            !string.IsNullOrWhiteSpace(order.pin) ? string.Format("OIB{0}: {1}", isForeign ? string.Format(" / {0}", t.Tran("pin", "en").ToUpper()) : "", order.pin) : "");




//            p = new Paragraph();
//            p.Add(new Chunk("RAČUN R2", GetFont(12)));
//            if (isForeign) { p.Add(new Chunk(" / INVOICE", GetFont(8, Font.ITALIC))); }
//            doc.Add(p);

//            p = new Paragraph();
//            p.Add(new Chunk("Obračun prema naplaćenoj naknadi", GetFont(9, Font.ITALIC)));
//            if (isForeign) { p.Add(new Chunk(" / calculation according to a paid compensation", GetFont(8, Font.ITALIC))); }
//            doc.Add(p);

//            p = new Paragraph();
//            p.Add(new Chunk("Broj računa", GetFont()));
//            if (isForeign) { p.Add(new Chunk(" / invoice number", GetFont(8, Font.ITALIC))); }
//            p.Add(new Chunk(":", isForeign ? GetFont(8, Font.ITALIC) : GetFont(10)));
//            p.Add(new Chunk(string.Format(" {0}/1/1", order.number), GetFont(10)));
//            doc.Add(p);

            PdfPTable table = new PdfPTable(5);

            p = new Paragraph();
            p.Add(new Paragraph("Redni broj", GetFont()));
            if (isForeign) { p.Add(new Chunk("number", GetFont(8, Font.ITALIC))); }
            table.AddCell(new PdfPCell(p) { Border = PdfPCell.BOTTOM_BORDER, Padding = 2, MinimumHeight = 30, PaddingTop = 15, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

            p = new Paragraph();
            p.Add(new Paragraph("Naziv proizvoda / usluge", GetFont()));
            if (isForeign) { p.Add(new Chunk("description", GetFont(8, Font.ITALIC))); }
            table.AddCell(new PdfPCell(p) { Border = PdfPCell.BOTTOM_BORDER, Padding = 2, MinimumHeight = 30, PaddingTop = 15, });

            p = new Paragraph();
            p.Add(new Paragraph("Količina", GetFont()));
            if (isForeign) { p.Add(new Chunk("quantity", GetFont(8, Font.ITALIC))); }
            table.AddCell(new PdfPCell(p) { Border = PdfPCell.BOTTOM_BORDER, Padding = 2, MinimumHeight = 30, PaddingTop = 15, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

            p = new Paragraph();
            p.Add(new Paragraph("Jedinična cijena", GetFont()));
            if (isForeign) { p.Add(new Chunk("unit price", GetFont(8, Font.ITALIC))); }
            table.AddCell(new PdfPCell(p) { Border = PdfPCell.BOTTOM_BORDER, Padding = 2, MinimumHeight = 30, PaddingTop = 15, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

            p = new Paragraph();
            p.Add(new Paragraph("Ukupno", GetFont()));
            if (isForeign) { p.Add(new Chunk("total", GetFont(8, Font.ITALIC))); }
            table.AddCell(new PdfPCell(p) { Border = PdfPCell.BOTTOM_BORDER, Padding = 2, MinimumHeight = 30, PaddingTop = 15, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

            int row = 0;
            double totPrice = 0;
            foreach (Orders.Item item in order.items)
            {
                row++;
                totPrice = totPrice + (item.quantity);
                table.AddCell(new PdfPCell(new Phrase(string.Format("{0}.", row), GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2, MinimumHeight = 30, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase(item.style, GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2, MinimumHeight = 30, PaddingTop = 5 });
                table.AddCell(new PdfPCell(new Phrase(item.quantity.ToString(), GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2, MinimumHeight = 30, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase(string.Format("{0} kn", string.Format("{0:N}", 0)), GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2, MinimumHeight = 30, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(string.Format("{0} kn", string.Format("{0:N}", 0 * item.quantity)), GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2, MinimumHeight = 30, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            }

            table.AddCell(new PdfPCell(new Phrase("", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2, PaddingTop = 5 });
            table.AddCell(new PdfPCell(new Phrase("", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2, PaddingTop = 5 });
            table.AddCell(new PdfPCell(new Phrase("Ukupan iznos računa: ", GetFont(10))) { Border = PdfPCell.TOP_BORDER, Padding = 2, PaddingTop = 5, Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            table.AddCell(new PdfPCell(new Phrase(string.Format("{0} kn", string.Format("{0:N}", totPrice)), GetFont(10, Font.BOLD))) { Border = PdfPCell.TOP_BORDER, Padding = 2, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

            if (isForeign)
            {
                table.AddCell(new PdfPCell(new Phrase("", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2 });
                table.AddCell(new PdfPCell(new Phrase("", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2 });
                table.AddCell(new PdfPCell(new Phrase("Total: ", GetFont(8, Font.ITALIC))) { Border = PdfPCell.NO_BORDER, Padding = 2, Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(string.Format("{0} €", string.Format("{0:N}", 0)), GetFont(10, Font.BOLD))) { Border = PdfPCell.NO_BORDER, Padding = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            }


            table.WidthPercentage = 100f;
            float[] widths = new float[] { 1f, 3f, 1f, 1f, 1f };
            table.SetWidths(widths);
            doc.Add(table);

            p = new Paragraph();
            p.Add(new Chunk("PDV nije obračunat jer obveznik IG PROG nije u sustavu PDV - a po čl. 90, st. 1.Zakona o porezu na dodanu vrijednost.", GetFont(9, Font.ITALIC)));
            if (isForeign) { p.Add(new Chunk(" / VAT is not charged because taxpayer IG PROG is not registerd for VAT under Art 90, para 1 of the Law om VAT.", GetFont(8, Font.ITALIC))); }
            doc.Add(p);

            PdfPTable invoiceInfo_table = new PdfPTable(2);

            p = new Paragraph();
            p.Add(new Chunk("Datum i vrijeme", GetFont()));
            if (isForeign) { p.Add(new Chunk(" / date and time", GetFont(8, Font.ITALIC))); }
            p.Add(new Chunk(":", isForeign ? GetFont(8, Font.ITALIC) : GetFont(10)));
            invoiceInfo_table.AddCell(new PdfPCell(p) { Border = PdfPCell.NO_BORDER, Padding = 0, PaddingTop = 20 });
            invoiceInfo_table.AddCell(new PdfPCell(new Phrase(order.orderDate, GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 0, PaddingTop = 20, });

            p = new Paragraph();
            p.Add(new Chunk("Oznaka operatera", GetFont()));
            if (isForeign) { p.Add(new Chunk(" / operator", GetFont(8, Font.ITALIC))); }
            p.Add(new Chunk(":", isForeign ? GetFont(8, Font.ITALIC) : GetFont(10)));
            invoiceInfo_table.AddCell(new PdfPCell(p) { Border = PdfPCell.NO_BORDER, Padding = 0, PaddingTop = 5 });
            invoiceInfo_table.AddCell(new PdfPCell(new Phrase("IG", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 0, PaddingTop = 5, });

            p = new Paragraph();
            p.Add(new Chunk("Način plaćanja", GetFont()));
            if (isForeign) { p.Add(new Chunk(" / payment method", GetFont(8, Font.ITALIC))); }
            p.Add(new Chunk(":", isForeign ? GetFont(8, Font.ITALIC) : GetFont(10)));
            invoiceInfo_table.AddCell(new PdfPCell(p) { Border = PdfPCell.NO_BORDER, Padding = 0, PaddingTop = 5 });
            p = new Paragraph();
            p.Add(new Chunk("Transakcijski račun", GetFont()));
            if (isForeign) { p.Add(new Chunk(" / transaction occount", GetFont(8, Font.ITALIC))); }
            invoiceInfo_table.AddCell(new PdfPCell(p) { Border = PdfPCell.NO_BORDER, Padding = 0, PaddingTop = 5, });

            p = new Paragraph();
            p.Add(new Chunk("Mjesto isporuke", GetFont()));
            if (isForeign) { p.Add(new Chunk(" / place of issue", GetFont(8, Font.ITALIC))); }
            p.Add(new Chunk(":", isForeign ? GetFont(8, Font.ITALIC) : GetFont(10)));
            invoiceInfo_table.AddCell(new PdfPCell(p) { Border = PdfPCell.NO_BORDER, Padding = 0, PaddingTop = 5 });
            invoiceInfo_table.AddCell(new PdfPCell(new Phrase("Rijeka", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 0, PaddingTop = 5, });

            invoiceInfo_table.WidthPercentage = 100f;
            float[] invoiceInfo_widths = new float[] { 1f, 4f };
            if (isForeign) { invoiceInfo_widths = new float[] { 2f, 4f }; }
            invoiceInfo_table.SetWidths(invoiceInfo_widths);
            doc.Add(invoiceInfo_table);

            float spacing = 140f;
            if (row == 1) { spacing = 160f; }
            if (row == 2) { spacing = 140f; }
            if (row == 3) { spacing = 100f; }
            if (row == 4) { spacing = 60f; }
            if (row == 5) { spacing = 20f; }

            if (!string.IsNullOrWhiteSpace(order.note))
            {
                Paragraph title = new Paragraph();
                title.SpacingBefore = 20f;
                title.Font = GetFont();
                title.Add(order.note);
                doc.Add(title);
                spacing = spacing - 40f;
            }
            if (isForeign) { spacing = spacing - 40f; }
            #endregion Body

            #region Footer
            PdfPTable sign_table = new PdfPTable(2);
            sign_table.SpacingBefore = spacing;
            sign_table.AddCell(new PdfPCell(new Phrase("", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 0, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            sign_table.AddCell(new PdfPCell(new Phrase("Odgovorna osoba:", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 0, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
            sign_table.AddCell(new PdfPCell(new Phrase("", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 0, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            sign_table.AddCell(new PdfPCell(new Phrase("Igor Gašparović", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 0, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

            sign_table.WidthPercentage = 100f;
            float[] sign_widths = new float[] { 4f, 1f };
            sign_table.SetWidths(sign_widths);
            doc.Add(sign_table);

            PdfPTable footer_table = new PdfPTable(1);
            footer_table.AddCell(new PdfPCell(new Phrase("mob: +385 98 330 966   |   email: igprog@yahoo.com   |   web: www.igprog.hr", GetFont(8))) { Border = PdfPCell.NO_BORDER, Padding = 0, PaddingTop = 80, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
            doc.Add(footer_table);
            #endregion Footer



            doc.Close();

            return fileName;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }


    private Font GetFont(int size, int style) {
       return FontFactory.GetFont(HttpContext.Current.Server.MapPath("~/assets/fonts/ARIALUNI.TTF"), BaseFont.IDENTITY_H, false, size, style);
    }

    private Font GetFont() {
        return GetFont(9, 0); // Normal font
    }

    private Font GetFont(int size) {
        return GetFont(size, 0);
    }

    private Font GetFont(bool x) {
        return GetFont(9, x == true ? Font.BOLD: Font.NORMAL);
    }





}
