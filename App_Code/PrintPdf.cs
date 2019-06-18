﻿using System;
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
    public string OfferPdf(Orders.NewOrder order, bool isForeign, string lang) {
        try {
            GetFont(8, Font.ITALIC).SetColor(255, 122, 56);
            Paragraph p = new Paragraph();
            var doc = new Document();
            string path = Server.MapPath("~/upload/offer/temp/");
            f.DeleteFolder(path);
            f.CreateFolder(path);
            string fileName = Guid.NewGuid().ToString();
            string filePath = Path.Combine(path, string.Format("{0}.pdf", fileName));
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

            doc.Open();

            AppendHeader(doc, lang);


            #region Body

            PdfPTable table0 = new PdfPTable(1);
            table0.WidthPercentage = 100f;
            table0.SpacingBefore = 40f;
            p = new Paragraph();
            p.Add(new Paragraph(string.Format("Predračun broj: {0}", order.number), GetFont(12, Font.BOLD)));
            table0.AddCell(new PdfPCell(p) { Border = PdfPCell.NO_BORDER, Padding = 2, MinimumHeight = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER, BackgroundColor=Color.LIGHT_GRAY });
            doc.Add(table0);
           
            PdfPTable table1 = new PdfPTable(4);
            table1.WidthPercentage = 100f;
            table1.SpacingBefore = 10f;
            float[] widths = new float[] { 1f, 3f, 1f, 2f };
            table1.SetWidths(widths);
            table1.AddCell(new PdfPCell(new Paragraph("Naziv kupca:", GetFont())) { Border = PdfPCell.NO_BORDER });
            table1.AddCell(new PdfPCell(new Paragraph(order.companyName, GetFont())) { Border = PdfPCell.NO_BORDER });
            table1.AddCell(new PdfPCell(new Paragraph("Datum izdavanja:", GetFont())) { Border = PdfPCell.NO_BORDER });
            table1.AddCell(new PdfPCell(new Paragraph(DateTime.Today.ToShortDateString(), GetFont())) { Border = PdfPCell.NO_BORDER });

            table1.AddCell(new PdfPCell(new Paragraph("OIB:", GetFont())) { Border = PdfPCell.NO_BORDER });
            table1.AddCell(new PdfPCell(new Paragraph(order.pin, GetFont())) { Border = PdfPCell.NO_BORDER });
            table1.AddCell(new PdfPCell(new Paragraph("Vrijedi do:", GetFont())) { Border = PdfPCell.NO_BORDER });
            table1.AddCell(new PdfPCell(new Paragraph(DateTime.Today.AddDays(15).ToShortDateString(), GetFont())) { Border = PdfPCell.NO_BORDER });

            table1.AddCell(new PdfPCell(new Paragraph("Adresa:", GetFont())) { Border = PdfPCell.NO_BORDER });
            table1.AddCell(new PdfPCell(new Paragraph(order.address, GetFont())) { Border = PdfPCell.NO_BORDER });
            table1.AddCell(new PdfPCell(new Paragraph("Valuta:", GetFont())) { Border = PdfPCell.NO_BORDER });
            table1.AddCell(new PdfPCell(new Paragraph("kn", GetFont())) { Border = PdfPCell.NO_BORDER });

            table1.AddCell(new PdfPCell(new Paragraph("Mjesto:", GetFont())) { Border = PdfPCell.NO_BORDER });
            table1.AddCell(new PdfPCell(new Paragraph(string.Format("{0} {1}", order.postalCode, order.city), GetFont())) { Border = PdfPCell.NO_BORDER });
            table1.AddCell(new PdfPCell(new Paragraph("Poziv na broj:", GetFont())) { Border = PdfPCell.NO_BORDER });
            table1.AddCell(new PdfPCell(new Paragraph(order.number.Replace('/', '-'), GetFont())) { Border = PdfPCell.NO_BORDER });

            table1.AddCell(new PdfPCell(new Paragraph("Telefon:", GetFont())) { Border = PdfPCell.NO_BORDER });
            table1.AddCell(new PdfPCell(new Paragraph(order.phone, GetFont())) { Border = PdfPCell.NO_BORDER });
            table1.AddCell(new PdfPCell(new Paragraph("", GetFont())) { Border = PdfPCell.NO_BORDER });
            table1.AddCell(new PdfPCell(new Paragraph("", GetFont())) { Border = PdfPCell.NO_BORDER });

            table1.AddCell(new PdfPCell(new Paragraph("E-mail:", GetFont())) { Border = PdfPCell.NO_BORDER });
            table1.AddCell(new PdfPCell(new Paragraph(order.email, GetFont())) { Border = PdfPCell.NO_BORDER });
            table1.AddCell(new PdfPCell(new Paragraph("", GetFont())) { Border = PdfPCell.NO_BORDER });
            table1.AddCell(new PdfPCell(new Paragraph("", GetFont())) { Border = PdfPCell.NO_BORDER });

            doc.Add(table1);

            PdfPTable table = new PdfPTable(8);
            table.WidthPercentage = 100f;
            table.SpacingBefore = 10f;
            widths = new float[] { 1f, 4f, 1f, 1f, 1.5f, 1f, 1f, 1.5f };
            table.SetWidths(widths);

            string price_title = string.Format(@"Cijena
(bez PDV-a)");
            string total_title = string.Format(@"Ukupno
(bez PDV-a)");

            table.AddCell(new PdfPCell(new Paragraph("Rb", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2, MinimumHeight = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfCell.ALIGN_MIDDLE, BackgroundColor = Color.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Paragraph("Proizvod / usluga", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2, MinimumHeight = 20, HorizontalAlignment = PdfPCell.ALIGN_LEFT, VerticalAlignment = PdfCell.ALIGN_MIDDLE, BackgroundColor = Color.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Paragraph("Količina", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2, MinimumHeight = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfCell.ALIGN_MIDDLE, BackgroundColor = Color.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Paragraph("j.m.", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2, MinimumHeight = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfCell.ALIGN_MIDDLE, BackgroundColor = Color.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Paragraph(price_title, GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2, MinimumHeight = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfCell.ALIGN_MIDDLE, BackgroundColor = Color.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Paragraph("Rabat%", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2, MinimumHeight = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfCell.ALIGN_MIDDLE, BackgroundColor = Color.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Paragraph("PDV%", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2, MinimumHeight = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfCell.ALIGN_MIDDLE, BackgroundColor = Color.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Paragraph(total_title, GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2, MinimumHeight = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER, VerticalAlignment = PdfCell.ALIGN_MIDDLE, BackgroundColor = Color.LIGHT_GRAY });

            Price pr = new Price();
            double vat = Math.Round((pr.GetCoeff().vat-1)*100, 2);

            int row = 0;
            foreach (Orders.Item item in order.items) {
                row++;
                table.AddCell(new PdfPCell(new Phrase(string.Format("{0}.", row), GetFont())) { Border = PdfPCell.BOTTOM_BORDER, Padding = 2, MinimumHeight = 30, PaddingTop = 2, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase(ItemDes(item, lang), GetFont())) { Border = PdfPCell.BOTTOM_BORDER, Padding = 2, MinimumHeight = 30, PaddingTop = 2 });
                table.AddCell(new PdfPCell(new Phrase(item.quantity.ToString(), GetFont())) { Border = PdfPCell.BOTTOM_BORDER, Padding = 2, MinimumHeight = 30, PaddingTop = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase("kom.", GetFont())) { Border = PdfPCell.BOTTOM_BORDER, Padding = 2, MinimumHeight = 30, PaddingTop = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(string.Format("{0}", string.Format("{0:N}", item.price)), GetFont())) { Border = PdfPCell.BOTTOM_BORDER, Padding = 2, MinimumHeight = 30, PaddingTop = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(string.Format("{0}%", string.Format("{0:N}", item.discount * 100)), GetFont())) { Border = PdfPCell.BOTTOM_BORDER, Padding = 2, MinimumHeight = 30, PaddingTop = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(string.Format("{0}%", string.Format("{0:N}", vat)), GetFont())) { Border = PdfPCell.BOTTOM_BORDER, Padding = 2, MinimumHeight = 30, PaddingTop = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
                table.AddCell(new PdfPCell(new Phrase(string.Format("{0}",  string.Format("{0:N}", (item.price * item.quantity) - (item.price * item.quantity * item.discount))), GetFont())) { Border = PdfPCell.BOTTOM_BORDER, Padding = 2, MinimumHeight = 30, PaddingTop = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            }

            doc.Add(table);

            PdfPTable table2 = new PdfPTable(3);
            table2.WidthPercentage = 100f;
            table2.SpacingBefore = 10f;
            widths = new float[] { 5f, 2f, 1f };
            table2.SetWidths(widths);

            //TODO: srediti kod, refaktorirati: order.items.Sum(a => a.price * a.quantity)

            table2.AddCell(new PdfPCell(new Phrase("", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2 });
            table2.AddCell(new PdfPCell(new Phrase("Ukupno (bez PDV-a i rabata):", GetFont(true))) { Border = PdfPCell.NO_BORDER, Padding = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            table2.AddCell(new PdfPCell(new Phrase(string.Format("{0:N} kn", order.items.Sum(a => a.price * a.quantity)), GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            table2.AddCell(new PdfPCell(new Phrase("", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2 });
            table2.AddCell(new PdfPCell(new Phrase("Ukupno rabat:", GetFont(true))) { Border = PdfPCell.NO_BORDER, Padding = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            table2.AddCell(new PdfPCell(new Phrase(string.Format("{0:N} kn", order.items.Sum(a => a.price * a.quantity * a.discount)), GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            table2.AddCell(new PdfPCell(new Phrase("", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2 });
            table2.AddCell(new PdfPCell(new Phrase("Ukupno (bez PDV-a):", GetFont(true))) { Border = PdfPCell.NO_BORDER, Padding = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            table2.AddCell(new PdfPCell(new Phrase(string.Format("{0:N} kn", order.items.Sum(a => a.price * a.quantity) - order.items.Sum(a => a.price * a.quantity * a.discount)), GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            table2.AddCell(new PdfPCell(new Phrase("", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2, PaddingBottom = 5 });
            table2.AddCell(new PdfPCell(new Phrase("Ukupan PDV:", GetFont(true))) { Border = PdfPCell.BOTTOM_BORDER, Padding = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT, PaddingBottom = 5 });
            table2.AddCell(new PdfPCell(new Phrase(string.Format("{0:N} kn", (order.items.Sum(a => a.price * a.quantity) - order.items.Sum(a => a.price * a.quantity * a.discount)) * (pr.GetCoeff().vat-1)), GetFont())) { Border = PdfPCell.BOTTOM_BORDER, Padding = 2, PaddingBottom = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            table2.AddCell(new PdfPCell(new Phrase("", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2 });
            table2.AddCell(new PdfPCell(new Phrase("Ukupno za platiti:", GetFont(true))) { Border = PdfPCell.NO_BORDER, Padding = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            table2.AddCell(new PdfPCell(new Phrase(string.Format("{0:N} kn", (order.items.Sum(a => a.price * a.quantity) - order.items.Sum(a => a.price * a.quantity * a.discount) + (order.items.Sum(a => a.price * a.quantity) - order.items.Sum(a => a.price * a.quantity * a.discount)) * (pr.GetCoeff().vat - 1))), GetFont(10, true))) { Border = PdfPCell.NO_BORDER, Padding = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });


            //table2.AddCell(new PdfPCell(new Phrase(string.Format("{0:N} kn", totPrice * order.discount.coeff), GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 2 });

            doc.Add(table2);


            p = new Paragraph();
            p.Add(new Chunk(string.Format("UKUPNO KOMADA: {0}", order.items.Count), GetFont(true)));
            doc.Add(p);

            p = new Paragraph();
            p.Add(new Chunk("OVO NIJE FISKALIZIRANI RAČUN.", GetFont(true)));
            doc.Add(p);

            string note = @"
Vaš predračun/ponuda vrijedi do datuma roka plaćanja istaknutog na vrhu dokumenta.
Plaćanje je 100% avans ukoliko nije drugačije navedeno.
Uplatu predračuna ili ponude možete izvršiti na naše sljedeće račune pri Erste & Steiermärkische Bank d.d.
IBAN: HR4424020061100647760 ili
IBAN: HR3024020061500057346
Model plaćanja: HR99 / HR00
Poziv na broj možete pustiti prazan, Vaše uplate vodimo pod vašim imenom.
Dostavu vršimo dostavnim službama: Overseas Express / GLS i dostavu naplaćujemo po trenutno važećim cjenicima
navedenih dostavnih službi, ukoliko nije drugačije navedeno.
Ukoliko imate bilo kakvih pitanja slobodno nam se obratite putem maila: info@megamajice.com, info@studio-lateralus.hr,
putem naših web stranica: www.megamajice.com ili pozivom na broj: +385 91 460 70 10 (Poziv, SMS, Viber ili Whatsapp)
VAŽNO: po ovom dokumentu iskazani porez NIJE MOGUĆE koristiti kao pretporez!";

            p = new Paragraph();
            p.Add(new Chunk(note, GetFont(true)));
            doc.Add(p);

            float spacing = 20f;
            //if (row == 1) { spacing = 160f; }
            //if (row == 2) { spacing = 140f; }
            //if (row == 3) { spacing = 100f; }
            //if (row == 4) { spacing = 60f; }
            //if (row == 5) { spacing = 20f; }

            #endregion Body

            #region Footer

            if (row > 3) {
                doc.NewPage();
                AppendHeader(doc, lang);
                AppendFooter(doc, spacing);
            } else {
                AppendFooter(doc, spacing);
            }

            #endregion Footer


            doc.Close();

            return fileName;
        } catch (Exception e) {
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
    private Font GetFont(int size, bool x) {
        return GetFont(size, x == true ? Font.BOLD : Font.NORMAL);
    }

    private string ItemDes(Orders.Item item,  string lang) {
        return string.Format("{0} {1} {2} {3}", item.brand.ToUpper(), item.style.ToUpper(), lang == "hr" ? item.shortdesc_hr.ToUpper() : item.shortdesc_en.ToUpper(), t.Tran(item.color, lang).ToUpper());
    }

    private void AppendFooter(Document doc, float spacing) {

        PdfPTable table = new PdfPTable(2);
        table.SpacingBefore = spacing;
        table.WidthPercentage = 100f;
        //  float[] sign_widths = new float[] { 4f, 1f };
        // table3.SetWidths(sign_widths);
        table.AddCell(new PdfPCell(new Phrase("Zahvaljujemo na Vašem povjerenju.", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 0, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_LEFT });
        table.AddCell(new PdfPCell(new Phrase("Obračunao(la): Mirza Hodžić", GetFont())) { Border = PdfPCell.NO_BORDER, Padding = 0, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

        doc.Add(table);

        string footer = @"
Lateralus j.d.o.o. jednostavno društvo sa ograničenom odgovornošću sa sjedištem u Kastvu, registrirano na Trgovačkom sudu u Rijeci sa temeljnim kapitalom: 200,00kn, uplaćenim u cijelosti. Osnivač društva: Mirza Hodžić. Društvo zastupa: Azra Hodžić, zastupa samostalno i neograničeno.
Matični broj društva: 040297018, OIB: 90780660216
Transakcijski računi (IBAN): ERSTE- HR 44 2402006 11 00 64 77 60; ERSTE - HR30 2402006 15 00 05 73 46 (SWIFT/BIC: ESBCHR22 / BANK: Erste & Steiermärkische Bank d.d.)";

        Paragraph p = new Paragraph();
        p.Add(new Chunk(footer, GetFont()));
        doc.Add(p);
    }

    private void AppendHeader(Document doc, string lang) {
        Image logo = Image.GetInstance(logoPath);
        logo.Alignment = Image.ALIGN_RIGHT;
        //logo.ScaleToFit(160f, 30f);
        logo.ScaleToFit(280f, 70f);
        logo.SpacingAfter = 15f;
        //logo.ScalePercent(9f);

        Admin.CompanyInfo ci = JsonConvert.DeserializeObject<Admin.CompanyInfo>(f.GetFile("json", "companyinfo"));

        Paragraph p = new Paragraph();
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
    }


}
