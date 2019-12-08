using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Configuration;
using System.Data.SQLite;
using System.Diagnostics;
using System.Text;
using Igprog;

/// <summary>
/// Products
/// </summary>
[WebService(Namespace = "https://promo-tekstil.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class Products : System.Web.Services.WebService {
    string productDataBase = ConfigurationManager.AppSettings["ProductDataBase"];
    DataBase db = new DataBase();
    Price pr = new Price();
    Categories categories = new Categories();

    public Products() {
    }

    #region Class
    public class Product {
        public string sku { get; set; }
        public string colorname { get; set; }
        public string size { get; set; }
        public string style { get; set; }
        public string brand { get; set; }
        public string modelimageurl { get; set; }
        public string colorimageurl { get; set; }
        public string packshotimageurl { get; set; }
        public string shortdesc_en { get; set; }
        public string longdesc_en { get; set; }
        public string gender_en { get; set; }
        public string category_en { get; set; }
        public string colorhex { get; set; }
        public int colorgroup_id { get; set; }
        public int isnew { get; set; }
        public string weight { get; set; }
        public string colorswatch { get; set; }
        public int outlet { get; set; }
        public string caseqty { get; set; }
        public string supplier { get; set; }

    }

    public class Style {
        public string style { get; set; }
        public string gsmweight { get; set; }
        public string sizes { get; set; }
        public string colors { get; set; }
        public string outlet { get; set; }
        public string coo { get; set; }
        public string imageurl { get; set; }
        public string altimageurl { get; set; }
        public string fabric_en { get; set; }
        public string cut_en { get; set; }
        public string details_en { get; set; }
        public string carelabels_en { get; set; }
        public string carelabellogos { get; set; }
        public string category_en { get; set; }
        public string specimageurl { get; set; }
        public string isnew { get; set; }
        public string supplier { get; set; }
        public string longdesc_en { get; set; }  // only for CottonClassic
        public string brand { get; set; }
        public string shortdesc_en { get; set; }
        public string gender_en { get; set; } 
        public string brand_code { get; set; } 
        public string gender_code { get; set; } 
        public string shortdesc_hr { get; set; } 
        public double price_min { get; set; }

    }

    public class Stock {
        #region UTT
        public string style { get; set; }
        public string color { get; set; }
        public string size { get; set; }
        public string sku { get; set; }
        public string uttstock { get; set; }
        public string suppstock { get; set; }
        public double price { get; set; }
        public double specialprice { get; set; }
        public string specialstart { get; set; }
        public string specialend { get; set; }
        public string currency { get; set; }
        public string uom { get; set; }
        public string weight { get; set; }
        public string colorswatch { get; set; }
        public int outlet { get; set; }
        public string caseqty { get; set; }
        #endregion UTT
        public string colorhex { get; set; }
        public string modelimageurl { get; set; }
        public string shortdesc_en { get; set; }
        public int quantity { get; set; }
        public string colorimageurl { get; set; }
        public string[] packshotimageurl { get; set; }

        public Price.NewPrice myprice = new Price.NewPrice();
        public string category_code { get; set; }
        public string brand_code { get; set; }
        public string supplier { get; set; }
    }

    public class ColorGroup {
        public string colorgroup_id { get; set; }
        public string colorgroupname { get; set; }
        public string colorfamily_en { get; set; }
        public string colorfamily_hu { get; set; }

    }

    public class ProductData {
        public string sku { get; set; }
        public string colorname { get; set; }
        public string size { get; set; }
        public string style { get; set; }
        public string brand { get; set; }
        public string modelimageurl { get; set; }
        public string shortdesc_en { get; set; }
        public string shortdesc_hr { get; set; }
        public string[] longdesc_en { get; set; }
        public string[] longdesc_hr { get; set; }
        public string gender_en { get; set; }
        public string category_en { get; set; }
        public string colorhex { get; set; }
        public string sizes { get; set; }
        public string colors { get; set; }
        public int colorgroup_id { get; set; }
        public int isnew { get; set; }
        public double uttprice { get; set; }

        public List<Stock> stock = new List<Stock>();

        public Price.NewPrice price_min = new Price.NewPrice();

        public Price.NewPrice price_max = new Price.NewPrice();
        public string colorimageurl { get; set; }
        public string[] packshotimageurl { get; set; }

        public List<CareLabel> carelabel = new List<CareLabel>();
        public string category_code { get; set; }
        public string brand_code { get; set; }
        public string gender_code { get; set; }

        public List<SizeSpecification> size_specification = new List<SizeSpecification>();
        public string specimageurl { get; set; }

        public List<CaseQty> piecesPerBox = new List<CaseQty>();
        public int outlet { get; set; }
        public string supplier { get; set; }

        public string imageurl { get; set; }
       // public double myprice { get; set; }

        //   public int max_stock { get; set; }
    }

    public class Category {
        public string title { get; set; }
        public string code { get; set; }
        public int count { get; set; }
        public bool isselected { get; set; }
        public int order { get; set; }
    }

    public class Brand {
        public string title { get; set; }
        public string code { get; set; }
        public bool isselected { get; set; }
    }

    public class DistColorGroup {
        public string colorfamily_en { get; set; }
        public List<ColorGroup> colorchild { get; set; }
        public bool isselected { get; set; }
    }

    public class Size {
        public string title { get; set; }
        public string code { get; set; }
        public bool isselected { get; set; }
    }

    public class Gender {
        public string title { get; set; }
        public string code { get; set; }
        public bool isselected { get; set; }
    }

    public class Distinct {
        public List<Brand> brand { get; set; }
        public List<DistColorGroup> colorGroup { get; set; }
        public List<Size> size { get; set; }
        public List<Gender> gender { get; set; }
    }

    public class ProductsData {
        public List<ProductData> products { get; set; }
        public Distinct distinct { get; set; }

        public Response response = new Response(); 
    }

    public class Response {
        public string time { get; set; }
        public double responseTime1 { get; set; }
        public double responseTime2 { get; set; }
        public double responseTime3 { get; set; }
        public double responseTime4 { get; set; }
        public double responseTime5 { get; set; }
        public double responseTime6 { get; set; }
        public double responseTime7 { get; set; }
        public double responseTime { get; set; }
        public int count { get; set; }
        public double maxPrice { get; set; }
    }

    public class CareLabel {
        public string carelabels_en { get; set; }
        public string carelabellogos { get; set; }
    }

    public class SizeSpecification {
        public string style { get; set; }
        public string size { get; set; }
        public string name_en { get; set; }
        public string value { get; set; }
    }

    public class CaseQty {
        public string size { get; set; }
        public int qty { get; set; }
    }

    public class Translation {
        public string style { get; set; }
        public string shortdesc_en { get; set; }
        public string shortdesc_hr { get; set; }
        public string longdesc_en { get; set; }
        public string longdesc_hr { get; set; }
    }

    public class ColorCC {
        public string manufName;
        public string manufCode;
        public string colorName;
        public string cmyk;
        public string rgb;
    }
    #endregion Class

    #region WebMethods
    [WebMethod]
    public string ImportLacunaXML() {
        try {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double time = 0;
            string supplier = "lacuna";
            string category = "Lacuna"; // "Workwear";
            double eurHrkCourse = Convert.ToDouble(ConfigurationManager.AppSettings["eurHrkCourse"]);
            string xml = RequestData("https://vp.lacuna.hr/exportxml.aspx?partner=40204");
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(xml);
            System.Xml.XmlNodeList products = xmlDoc.SelectNodes("/sadrzaj/proizvodi/proizvod");
            List<Product> xx = new List<Product>();
            Product x = new Product();
            List<Stock> yy = new List<Stock>();
            Stock y = new Stock();
            List<Style> zz = new List<Style>();
            Style z = new Style();

            foreach (System.Xml.XmlNode node in products) {
                x = new Product();
                x.sku = node.SelectSingleNode("sifraProizvoda").InnerText;
                x.colorname = GetColor(x.sku);
                x.size = GetSize(x.sku);
                x.style = GetStyle(x.sku); // x.sku.Split('/')[0];
                x.brand = "";
                x.modelimageurl = node.SelectSingleNode("slikaProizvoda").InnerText;
                x.shortdesc_en = node.SelectSingleNode("naziv").InnerText;
                x.longdesc_en = node.SelectSingleNode("opis").InnerText.Replace(";&lt;br&gt;", ";").Replace("###&lt", "").Replace("br&gt", "");
                x.gender_en = GetGender(x.sku);
                x.category_en = category;
                x.colorhex = GetColorHex(x.colorname); // "";
                x.colorgroup_id = 0;
                x.isnew = 0;
                x.colorimageurl = "";
                x.packshotimageurl = node.SelectSingleNode("slikaProizvoda").InnerText;
                x.weight = "";
                x.colorswatch = "";
                x.outlet = 0;
                x.caseqty = "";
                x.supplier = supplier;
                xx.Add(x);

                y = new Stock();
                y.style = x.style;
                y.color = x.colorname;
                y.size = x.size;
                y.sku = x.sku;
                y.uttstock = node.SelectSingleNode("zalihaProizvoda").InnerText.Split(',')[0];
                y.suppstock = "";
                y.price = !string.IsNullOrEmpty(node.SelectSingleNode("cijena").InnerText) ? Convert.ToDouble(node.SelectSingleNode("cijena").InnerText.Replace(",", "."))/eurHrkCourse : 0;
                y.specialprice = 0;
                y.specialstart = "";
                y.specialend = "";
                y.currency = "";
                y.uom = "";
                y.supplier = x.supplier;
                yy.Add(y);

                z = new Style();
                z.style = x.style;
                z.gsmweight = "";
                z.sizes =  "";
                z.colors = "";
                z.outlet = x.outlet.ToString();
                z.coo = "";
                z.imageurl = x.modelimageurl;
                z.altimageurl = "";
                z.fabric_en = "";
                z.cut_en = "";
                z.details_en = "";
                z.carelabels_en = "";
                z.carelabellogos = "";
                z.category_en = x.category_en;
                z.specimageurl = "";
                z.isnew = x.isnew.ToString();
                z.supplier = x.supplier;
                zz.Add(z);
            }

            List<Style> distinctStyle = zz
                          .GroupBy(p => p.style)
                          .Select(g => g.First())
                          .ToList();

            foreach(Style ds in distinctStyle) {
                ds.sizes = GetSizes(xx.Where(a => a.style == ds.style).ToList());
            }

            SaveDdb(xx, yy, distinctStyle);

            time = stopwatch.Elapsed.TotalSeconds;
            return string.Format(@"{0} items updated successfully in {1} seconds.", xx.Count(), time);
        } catch(Exception e) {
            return JsonConvert.SerializeObject(e.Message, Formatting.None);
        }
    }

    [WebMethod]
    public string ImportCottonClassicsCsv(string file) {
        try {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double time = 0;
            string supplier = "cc";
            List<Product> xx = new List<Product>();
            Product x = new Product();
            List<Stock> yy = new List<Stock>();
            Stock y = new Stock();
            List<Style> zz = new List<Style>();
            Style z = new Style();
            string path = Server.MapPath(string.Format("~/upload/{0}.csv", file));
            List<ColorCC> colors = JsonConvert.DeserializeObject<List<ColorCC>>(GetJsonFile("json", "colors_cc"));
            List<string> colornames = new List<string>();

            using (var reader = new StreamReader(path, Encoding.ASCII)) {
                while (!reader.EndOfStream) {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    if(!string.IsNullOrEmpty(values[0]) && values[0] != "sku") {
                        x = new Product();
                        x.sku = values[0];
                        x.colorname = values[1];
                        x.size = values[2];
                        x.style = values[3];
                        x.brand = values[4];
                        x.modelimageurl = values[5];
                        x.shortdesc_en = values[6];
                        x.longdesc_en = values[7];
                        x.gender_en = values[8];
                        x.category_en = values[9];
                        colornames = x.colorname.Split('/').ToList();
                        if (colornames.Count == 1) {
                            x.colorhex = colors.Find(a => a.manufName == x.brand && a.colorName == colornames[0]) != null ? string.Format("#{0}", colors.Find(a => a.manufName == x.brand && a.colorName == colornames[0]).rgb) : "";
                        } else if (colornames.Count == 2) {
                            x.colorhex = string.Format("{0}/{1}"
                                        , colors.Find(a => a.manufName == x.brand && a.colorName == colornames[0]) != null ? string.Format("#{0}", colors.Find(a => a.manufName == x.brand && a.colorName == colornames[0]).rgb) : ""
                                        , colors.Find(a => a.manufName == x.brand && a.colorName == colornames[1]) != null ? string.Format("#{0}", colors.Find(a => a.manufName == x.brand && a.colorName == colornames[1]).rgb) : "");

                        } else if (colornames.Count == 3) {
                            x.colorhex = string.Format("{0}/{1}/{2}"
                                        , colors.Find(a => a.manufName == x.brand && a.colorName == colornames[0]) != null ? string.Format("#{0}", colors.Find(a => a.manufName == x.brand && a.colorName == colornames[0]).rgb) : ""
                                        , colors.Find(a => a.manufName == x.brand && a.colorName == colornames[1]) != null ? string.Format("#{0}", colors.Find(a => a.manufName == x.brand && a.colorName == colornames[1]).rgb) : ""
                                        , colors.Find(a => a.manufName == x.brand && a.colorName == colornames[2]) != null ? string.Format("#{0}", colors.Find(a => a.manufName == x.brand && a.colorName == colornames[2]).rgb) : "");
                        } else {
                            x.colorhex = colors.Find(a => a.manufName == x.brand && a.colorName == x.colorname) != null ? string.Format("#{0}", colors.Find(a => a.manufName == x.brand && a.colorName == x.colorname).rgb) : "";
                        }
                        x.colorgroup_id = !string.IsNullOrEmpty(values[11]) ? Convert.ToInt32(values[11]) : 0;
                        x.isnew = !string.IsNullOrEmpty(values[11]) ? Convert.ToInt32(values[12]): 0;
                        x.colorimageurl = values[13];
                        x.packshotimageurl = values[14];
                        x.weight = values[15];
                        x.colorswatch = values[16];
                        x.outlet = !string.IsNullOrEmpty(values[11]) ? Convert.ToInt32(values[17]): 0;
                        x.caseqty = values[18];
                        x.supplier = supplier;
                        xx.Add(x);

                        y = new Stock();
                        y.style = x.style;
                        y.color = x.colorname;
                        y.size = x.size;
                        y.sku = x.sku;
                        y.uttstock = "5000"; // values[20];
                        y.suppstock = "5000"; //  values[21];
                        y.price = !string.IsNullOrEmpty(values[22]) ? (values[22] == "#N/A" ? 0 : Convert.ToDouble(values[22])) : 0;
                        y.specialprice = !string.IsNullOrEmpty(values[23]) ? Convert.ToDouble(values[23]) : 0;
                        y.specialstart = values[24];
                        y.specialend = values[25];
                        y.currency = values[26];
                        y.uom = values[27];
                        y.supplier = supplier;
                        yy.Add(y);

                        if (!string.IsNullOrEmpty(values[28]) && values[28] != "0" )  {
                           z = new Style();
                           z.style = values[28];
                           z.gsmweight = "";
                           z.sizes = null;
                           z.colors = null;
                           z.outlet = "0";
                           z.coo = "";
                           z.imageurl = values[31];
                           z.altimageurl = "";
                           z.fabric_en = "";
                           z.cut_en = "";
                           z.details_en = "";
                           z.carelabels_en = "";
                           z.carelabellogos = "";
                           z.category_en = GetCategory(values[32]);
                           z.specimageurl = "";
                           z.isnew = "0";
                           z.supplier = supplier;
                           z.longdesc_en = string.Format("{0};{1}", values[29], values[30]);
                           zz.Add(z);
                       }
                    }
                }
            }

            List<Product> products = new List<Product>();
            foreach (Product p in xx) {
                var aa = zz.Where(a => a.style == p.style).FirstOrDefault();
                if (!CategoryToExclude(aa.category_en)) {
                    if (!BrandToExclude(p.brand)) {
                        Product p_ = new Product();
                        p_ = p;
                        p_.category_en = aa.category_en;
                        p_.modelimageurl = aa.imageurl;
                        p_.longdesc_en = aa.longdesc_en;
                        products.Add(p_);
                    }
                }
            }

            List<Stock> stock = new List<Stock>();
            foreach (Stock st in yy) {
                if (products.Any(a => a.sku == st.sku)) {
                    Stock st_ = new Stock();
                    st_ = st;
                    stock.Add(st_);
                }
            }

            List<Style> styles = new List<Style>();
            foreach (Style s in zz) {
                if (products.Any(a => a.style == s.style)) {
                    Style s_ = new Style();
                    s_ = s;
                    var bb = products.Where(a => a.style == s.style).ToList();
                    s_.sizes = GetSizes(bb);
                    s_.colors = GetColors(bb);
                    styles.Add(s_);
                }
            }

            SaveDdb(products, stock, styles);

            time = stopwatch.Elapsed.TotalSeconds;
            return string.Format(@"{0} items updated successfully in {1} seconds.", products.Count(), time);
        } catch (Exception e) {
            return e.Message;
        }
    }

    /********* Euroton *********/
    [WebMethod]
    public string ImportCsv(string file) {
        try {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double time = 0;
            List<Product> xx = new List<Product>();
            Product x = new Product();
            List<Stock> yy = new List<Stock>();
            Stock y = new Stock();
            List<Style> zz = new List<Style>();
            Style z = new Style();
            string path = Server.MapPath(string.Format("~/upload/{0}.csv", file));

            using (var reader = new StreamReader(path, Encoding.ASCII)) {
                while (!reader.EndOfStream) {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    if(!string.IsNullOrEmpty(values[0]) && values[0] != "sku") {
                        x = new Product();
                        x.sku = values[0];
                        x.colorname = values[1];
                        x.size = values[2];
                        x.style = values[3];
                        x.brand = values[4];
                        x.modelimageurl = values[5];
                        x.shortdesc_en = values[6];
                        x.longdesc_en = values[7];
                        x.gender_en = values[8];
                        x.category_en = values[9];
                        x.colorhex = values[10];
                        x.colorgroup_id = !string.IsNullOrEmpty(values[11]) ? Convert.ToInt32(values[11]) : 0;
                        x.isnew = !string.IsNullOrEmpty(values[11]) ? Convert.ToInt32(values[12]): 0;
                        x.colorimageurl = values[13];
                        x.packshotimageurl = values[14];
                        x.weight = values[15];
                        x.colorswatch = values[16];
                        x.outlet = !string.IsNullOrEmpty(values[11]) ? Convert.ToInt32(values[17]): 0;
                        x.caseqty = values[18];
                        x.supplier = values[19].ToLower();
                        xx.Add(x);

                        y = new Stock();
                        y.style = x.style;
                        y.color = x.colorname;
                        y.size = x.size;
                        y.sku = x.sku;
                        y.uttstock = values[20];
                        y.suppstock = values[21];
                        y.price = !string.IsNullOrEmpty(values[22]) ? Convert.ToDouble(values[22]) : 0;
                        y.specialprice = !string.IsNullOrEmpty(values[23]) ? Convert.ToDouble(values[23]): 0;
                        y.specialstart = values[24];
                        y.specialend = values[25];
                        y.currency = values[26];
                        y.uom = values[27];
                        y.supplier = x.supplier;
                        yy.Add(y);

                        z = new Style();
                        z.style = x.style;
                        z.gsmweight = values[28];
                        z.sizes = null;
                        z.colors = null;
                        z.outlet = x.outlet.ToString();
                        z.coo = values[29];
                        z.imageurl = values[30];
                        z.altimageurl = values[31];
                        z.fabric_en = values[32];
                        z.cut_en = values[33];
                        z.details_en = values[34];
                        z.carelabels_en = values[35];
                        z.carelabellogos = values[36];
                        z.category_en = x.category_en;
                        z.specimageurl = values[37];
                        z.isnew = x.isnew.ToString();
                        z.supplier = x.supplier;
                        zz.Add(z);
                    }
                }
            }

            List<Style> distinctStyle = zz
                          .GroupBy(p => p.style)
                          .Select(g => g.First())
                          .ToList();

            foreach (Style ds in distinctStyle) {
                ds.sizes = GetSizes(xx.Where(a => a.style == ds.style).ToList());
                ds.colors = GetColors(xx.Where(a => a.style == ds.style).ToList());
            }

            SaveDdb(xx, yy, distinctStyle);

            time = stopwatch.Elapsed.TotalSeconds;
            return string.Format(@"{0} items updated successfully in {1} seconds.", xx.Count(), time);
        } catch (Exception e) {
            return e.Message;
        }
    }

    [WebMethod]
    public string CreateDataBaseUtt() {
        Stopwatch stopwatch = new Stopwatch();
        string supplier = "utt";
        string sql_delete = "";
        string sql = "";
        stopwatch.Start();
        double uttTime = 0;
        try {
            List<Product> products = new List<Product>();
            products = JsonConvert.DeserializeObject<List<Product>>(RequestData("https://www.utteam.com/api/dataexport/b102f37bc6e73a7d59e12828a92f26f3?action=product&format=json&variables=&fields=sku,colorname,size,style,brand,modelimageurl,shortdesc_en,longdesc_en,gender_en,category_en,colorhex,colorgroup_id,isnew,colorimageurl,packshotimageurl,weight,colorswatch,outlet,caseqty"));
            List<Stock> stock = new List<Stock>();
            stock = JsonConvert.DeserializeObject<List<Stock>>(RequestData("https://www.utteam.com/api/dataexport/b102f37bc6e73a7d59e12828a92f26f3?action=stock&format=json&variables=&fields=style,color,size,sku,uttstock,suppstock,price,specialprice,specialstart,specialend,currency,uom"));
            List<Style> style = new List<Style>();
            style = JsonConvert.DeserializeObject<List<Style>>(RequestData("https://www.utteam.com/api/dataexport/b102f37bc6e73a7d59e12828a92f26f3?action=style&format=json&variables=&fields=style,gsmweight,gender_en,sizes,colors,outlet,coo,imageurl,altimageurl,fabric_en,cut_en,details_en,carelabels_en,carelabellogos,category_en,specimageurl,isnew"));
            List<SizeSpecification> size = new List<SizeSpecification>();
            size = JsonConvert.DeserializeObject<List<SizeSpecification>>(RequestData("https://utteam.com/api/dataexport/b102f37bc6e73a7d59e12828a92f26f3?action=sizespecs&format=json&variables=&fields=style,size,name_en,value"));
            uttTime = stopwatch.Elapsed.TotalSeconds;

            db.CreateDataBase(productDataBase, db.product);
            db.CreateDataBase(productDataBase, db.style);
            //db.CreateDataBase(productDataBase, db.stock);
            db.CreateDataBase(productDataBase, db.size);
            db.CreateDataBase(productDataBase, db.translation);

            List<Translation> tt = GetTranslations();

            using (var connection = new SQLiteConnection(@"Data Source=" + Server.MapPath("~/App_Data/" + productDataBase))) {
                connection.Open();
                using (var command = new SQLiteCommand(connection)) {
                    using (var transaction = connection.BeginTransaction()) {
                        sql_delete = string.Format("DELETE FROM product WHERE supplier = '{0}';", supplier);
                        command.CommandText = sql_delete;
                        command.ExecuteNonQuery();

                        foreach (Product p in products) {
                            sql = string.Format(@"INSERT OR REPLACE INTO product (sku, colorname, size, style, brand, modelimageurl, shortdesc_en, longdesc_en, gender_en, category_en, colorhex, colorgroup_id, isnew, colorimageurl, packshotimageurl, category_code, brand_code, gender_code, weight, colorswatch, outlet, caseqty, supplier)
                                                VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}')"
                                                , p.sku, p.colorname, p.size, p.style, p.brand.Replace("'", ""), p.modelimageurl, p.shortdesc_en.Replace("'", ""), p.longdesc_en.Replace("'", ""), p.gender_en.Replace("'", "")
                                                , p.category_en.Replace("&", "and"), p.colorhex, p.colorgroup_id, p.isnew, p.colorimageurl, p.packshotimageurl
                                                , p.category_en.Replace("&", "And").Replace(" ", ""), p.brand.Replace("&", "And").Replace(" ", "").Replace("'", "")
                                                , p.gender_en.Replace(" ", "").Replace("'", ""), p.weight, p.colorswatch, p.outlet, p.caseqty, supplier);
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }

                        sql_delete = string.Format("DELETE FROM style WHERE supplier = '{0}';", supplier);
                        command.CommandText = sql_delete;
                        command.ExecuteNonQuery();
                        foreach (Style s in style) {
                            var prod = products.Find(a => a.style == s.style);
                            double price_min = stock.Where(a => a.style == s.style).Min(a => a.price);
                            var tran = tt.Find(a => a.style == s.style);
                            sql = string.Format(@"INSERT OR REPLACE INTO style (style, gsmweight, sizes, colors, outlet, coo, imageurl, altimageurl, fabric_en, cut_en, details_en, carelabels_en, carelabellogos, category_en, category_code, specimageurl, isnew, supplier, brand, shortdesc_en, gender_en, brand_code, gender_code, shortdesc_hr, price_min)
                                                VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}')"
                                                , s.style, s.gsmweight, s.sizes, s.colors, s.outlet, s.coo, s.imageurl, s.altimageurl, s.fabric_en, s.cut_en
                                                , s.details_en, s.carelabels_en, s.carelabellogos, s.category_en.Replace("&", "and")
                                                , s.category_en.Replace("&", "And").Replace(" ", ""), s.specimageurl, s.isnew, supplier
                                                , prod.brand.Replace("'", ""), prod.shortdesc_en.Replace("'", ""), prod.gender_en.Replace(" ", "").Replace("'", ""), prod.brand.Replace("&", "And").Replace(" ", "").Replace("'", ""), prod.gender_en.Replace(" ", "").Replace("'", "")
                                                , tran!=null?tran.shortdesc_hr:"", price_min);
                            command.CommandText = sql;
                            command.ExecuteNonQuery();

                            string sql_tran = string.Format(@"INSERT OR IGNORE INTO translation (style, shortdesc_en, longdesc_en, category_en, supplier)
                                                VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')"
                                               , s.style, prod.shortdesc_en.Replace("'", ""), prod.longdesc_en.Replace("'", "")
                                               , s.category_en.Replace("&", "and"), supplier);
                            command.CommandText = sql_tran;
                            command.ExecuteNonQuery();
                        }


                        //OLD
                        //foreach (Style s in style) {
                        //    sql = string.Format(@"INSERT OR REPLACE INTO style (style, gsmweight, sizes, colors, outlet, coo, imageurl, altimageurl, fabric_en, cut_en, details_en, carelabels_en, carelabellogos, category_en, category_code, specimageurl, isnew, supplier)
                        //                        VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}')"
                        //                        , s.style, s.gsmweight, s.sizes, s.colors, s.outlet, s.coo, s.imageurl, s.altimageurl, s.fabric_en, s.cut_en
                        //                        , s.details_en, s.carelabels_en, s.carelabellogos, s.category_en.Replace("&", "and")
                        //                        , s.category_en.Replace("&", "And").Replace(" ", ""), s.specimageurl, s.isnew, supplier);
                        //    command.CommandText = sql;
                        //    command.ExecuteNonQuery();
                        //}

                        //sql_delete = string.Format("DELETE FROM stock WHERE supplier = '{0}';", supplier);
                        //command.CommandText = sql_delete;
                        //command.ExecuteNonQuery();
                        //foreach (Stock s in stock) {
                        //    sql = string.Format(@"INSERT INTO stock (style, color, size, sku, uttstock, suppstock, price, specialprice, specialstart, specialend, currency, uom, supplier)
                        //                        VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}')"
                        //                        , s.style, s.color, s.size, s.sku, s.uttstock, s.suppstock, s.price
                        //                        , s.specialprice, s.specialstart , s.specialend, s.currency, s.uom, supplier);
                        //    command.CommandText = sql;
                        //    command.ExecuteNonQuery();
                        //}

                        sql_delete = string.Format("DELETE FROM size WHERE supplier = '{0}';", supplier);
                        command.CommandText = sql_delete;
                        command.ExecuteNonQuery();
                        foreach (SizeSpecification s in size) {
                            sql = string.Format(@"INSERT INTO size (style, size, name_en, value, supplier)
                                                VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')"
                                                , s.style, s.size, s.name_en.Replace("'", ""), s.value, supplier);
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }

                        //TODO category prices..

                        /*  foreach (ColorGroup cg in colorGroup) {
                              command.CommandText = @"INSERT INTO colorgroup VALUES 
                                          (@colorgroup_id, @colorgroupname, @colorfamily_en)";
                              command.Parameters.Add(new SQLiteParameter("colorgroup_id", cg.colorgroup_id));
                              command.Parameters.Add(new SQLiteParameter("colorgroupname", cg.colorgroupname));
                              command.Parameters.Add(new SQLiteParameter("colorfamily_en", cg.colorfamily_en));
                              command.ExecuteNonQuery();
                          } */


                        //foreach (Style s in style) {
                        //    sql = string.Format(@"INSERT OR IGNORE INTO translation (style, shortdesc_en, longdesc_en, category_en, supplier)
                        //                        VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')"
                        //                        , s.style, s.shortdesc_en.Replace("'", ""), s.longdesc_en.Replace("'", "")
                        //                        , s.category_en.Replace("&", "and"), supplier);
                        //    command.CommandText = sql;
                        //    command.ExecuteNonQuery();
                        //}

                        //foreach (Product p in products) {
                        //    sql = string.Format(@"INSERT OR IGNORE INTO translation (sku, shortdesc_en, longdesc_en, category_en, supplier)
                        //                        VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')"
                        //                        , p.sku, p.shortdesc_en.Replace("'", ""), p.longdesc_en.Replace("'", "")
                        //                        , p.category_en.Replace("&", "and"), supplier);
                        //    command.CommandText = sql;
                        //    command.ExecuteNonQuery();
                        //}

                        transaction.Commit();
                    }
                }
                connection.Close();
            } return string.Format(@"{0} items downloaded from UTT in {1} seconds. Insert into products.ddb in {2} seconds.", products.Count(), uttTime, (stopwatch.Elapsed.TotalSeconds - uttTime));
        } catch (Exception e) {
            uttTime = stopwatch.Elapsed.TotalSeconds;
            return string.Format("ERROR: {0} ({1} seconds)", e.Message, uttTime);
        }
    }

    [WebMethod]
    public string UpdateStockUtt() {
        try {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            GetStockUtt();
            return ("Update Stock completed successfully. Time: " + stopwatch.Elapsed.TotalSeconds + " seconds.");
        } catch (Exception e) {
            return e.Message;
        }
    }


    [WebMethod]
    public string GetCategories() {
        try {
            List<Category> x = GetDistinctCategories();
            return JsonConvert.SerializeObject(x, Formatting.None);
        } catch (Exception e) {
            return null;
        }
    }

    [WebMethod]
    public string GetDistinctFilters(string category, string group) {
        try {
            Distinct x = new Distinct();
            using (SQLiteConnection connection = new SQLiteConnection(
                string.Format("Data Source={0}", Server.MapPath(string.Format("~/App_Data/{0}", productDataBase))))) {
                connection.Open();
                x = GetDistinct(connection, category, group, null);
                connection.Close();
            }
            return JsonConvert.SerializeObject(x, Formatting.None);
        } catch (Exception e) {
            return null;
        }
    }


    [WebMethod]
    public string GetProductsByCategory(int limit, string category, string sort, string order) {
        try {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ProductsData xxx = new ProductsData();
            xxx.response.responseTime1 = stopwatch.Elapsed.TotalSeconds;
            Price.PriceCoeff priceCoeff = pr.GetCoeff();
            xxx.response.responseTime2 = stopwatch.Elapsed.TotalSeconds;
            using (SQLiteConnection connection = new SQLiteConnection(
                string.Format("Data Source={0}", Server.MapPath(string.Format("~/App_Data/{0}", productDataBase))))) {
                                                    // 0          1       2            3              4        5          6          7           8             9           10                11           12            13
                string sql = string.Format(@"SELECT style, imageurl, isnew, category_code, outlet, isnew, supplier, brand, shortdesc_en, gender_en, brand_code, gender_code, shortdesc_hr, price_min FROM style
                                        WHERE category_code = '{0}' AND CAST(price_min AS DOUBLE) > 0 {1} LIMIT {2}", category, OrderBy(sort, order), limit);
                xxx.response.responseTime3 = stopwatch.Elapsed.TotalSeconds;
                using(SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                    connection.Open();
                    xxx.response.responseTime4 = stopwatch.Elapsed.TotalSeconds;
                    using (SQLiteDataReader reader = command.ExecuteReader()) {
                        xxx.response.responseTime5 = stopwatch.Elapsed.TotalSeconds;
                        List<ProductData> xx = new List<ProductData>();
                        while (reader.Read()) {
                            xx.Add(GetProductData(reader, priceCoeff));
                        }
                        xxx.products = xx;
                    };
                }

                xxx.response.responseTime6 = stopwatch.Elapsed.TotalSeconds;
                xxx.response.count = GetCount(string.Format(@"
                                    SELECT COUNT(style) FROM style WHERE category_code = '{0}'", category), connection);
                xxx.response.maxPrice = xxx.products.Max(a => a.price_min.net);
                xxx.response.responseTime7 = stopwatch.Elapsed.TotalSeconds;
                connection.Close();
                xxx.response.time = DateTime.Now.ToString();
                xxx.response.responseTime = stopwatch.Elapsed.TotalSeconds;
            }
            return JsonConvert.SerializeObject(xxx, Formatting.None);
        } catch (Exception e) { return e.Message; }
    }

    [WebMethod]
    public string GetProductsByGroup(int limit, string group, string type, string sort, string order) {
        try {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<Categories.SelectedCategories> sc = categories.GetCategories();
            Price.PriceCoeff priceCoeff = pr.GetCoeff();
            ProductsData xxx = new ProductsData();
            string groupQuery = "";
            switch (type) {
                case "brand":
                    if(group != "all") {
                        groupQuery = string.Format("WHERE brand_code = '{0}'", group);
                    }
                break;
                case "gender":
                    groupQuery = string.Format("WHERE gender_code = '{0}'", group);
                    break;
                case "isnew":
                    groupQuery = string.Format("WHERE isnew = '{0}'", group);
                    break;
                case "outlet":
                    groupQuery = string.Format("WHERE outlet = '{0}'", group);
                    break;
            }
            groupQuery = string.IsNullOrEmpty(groupQuery) ? "WHERE CAST(price_min AS DOUBLE) > 0" : string.Format("{0} AND CAST(price_min AS DOUBLE) > 0", groupQuery);
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + productDataBase))) {
                connection.Open();
                string sql = string.Format(@"SELECT style, imageurl, isnew, category_code, outlet, isnew, supplier, brand, shortdesc_en, gender_en, brand_code, gender_code, shortdesc_hr, price_min FROM style
                                            {0} {1} LIMIT {2}", groupQuery, OrderBy(sort, order), limit);
                List<ProductData> xx = new List<ProductData>();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                    using (SQLiteDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            ProductData x = GetProductData(reader, priceCoeff);
                            if (sc.Exists(a => a.code.ToLower() == x.category_code.ToLower())) {
                                if (sc.Find(a => a.code.ToLower() == x.category_code.ToLower()).isselected == true) {
                                    xx.Add(x);
                                }
                            }
                        }
                    } 
                }
                xxx.products = xx;
                xxx.response.time = DateTime.Now.ToString();
                xxx.response.responseTime = stopwatch.Elapsed.TotalSeconds;
                xxx.response.count = GetCount(string.Format(@"
                                    SELECT COUNT(style) FROM style {0}", groupQuery), connection);
                xxx.response.maxPrice = xx.Max(a => a.price_min.net);
                connection.Close();
            }  
            return JsonConvert.SerializeObject(xxx, Formatting.None);
        } catch (Exception e) { return e.Message; }
    }

    [WebMethod]
    public string SearchProducts(int limit, int page, string category, string search, Distinct filter, string group, string type, string sort, string order) {
        try {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<Categories.SelectedCategories> sc = categories.GetCategories();
            Price.PriceCoeff priceCoeff = pr.GetCoeff();
            ProductsData xxx = new ProductsData();
            bool isWhere = false;
            string groupQuery = "";
            switch (type) {
                case "brand":
                    if(group != "all") {
                        groupQuery = string.Format("WHERE brand_code = '{0}'", group);
                        isWhere = true;
                    }
                break;
                case "gender":
                    groupQuery = string.Format("WHERE gender_code = '{0}'", group);
                    isWhere = true;
                    break;
                case "isnew":
                    groupQuery = string.Format("WHERE isnew = '{0}'", group);
                    isWhere = true;
                    break;
                case "outlet":
                    groupQuery = string.Format("WHERE outlet = '{0}'", group);
                    isWhere = true;
                    break;
            }
            if (string.IsNullOrEmpty(groupQuery)) {
                groupQuery = "WHERE CAST(price_min AS DOUBLE) > 0";
                isWhere = true;
            } else {
                groupQuery = string.Format("{0} AND CAST(price_min AS DOUBLE) > 0", groupQuery);
            }

            /*  //TODO
            if (priceFilter > 0) {
                groupQuery = string.Format("WHERE CAST(price_min AS DOUBLE) > {0}", priceFilter);
                isWhere = true;
            } else {
                groupQuery = string.Format("{0} AND CAST(price_min AS DOUBLE) > {1}", groupQuery, priceFilter);
            }
            */

            //TODO
            string sqlCategoryQuery = "";
            if (!string.IsNullOrEmpty(category)) {
                sqlCategoryQuery = string.Format("AND category_code = '{0}'", category);
                isWhere = true;
            }
            
            string sqlSearchQuery = "";
            string sqlFilterQuery = "";
            
            if (!string.IsNullOrWhiteSpace(search)) {
                sqlSearchQuery = string.Format(@"
                    {1} (style LIKE '%{0}%' 
                    OR brand LIKE '%{0}%' OR shortdesc_en LIKE '%{0}%' OR shortdesc_hr LIKE '%{0}%') ", search, isWhere == true ? "AND" : "WHERE");
                isWhere = true;
            }
            else {
                if(filter != null) {
                    StringBuilder sqlBrandFilter = new StringBuilder();
                    if (filter.brand.Count > 0) {
                        if (filter.brand.Count(a => a.isselected == true) > 0) {
                            isWhere = true;
                            sqlBrandFilter.AppendLine("AND (");
                            int count = 0;
                            foreach (Brand b in filter.brand) {
                                if (b.isselected == true) {
                                    sqlBrandFilter.AppendLine(string.Format(@"{0} brand_code = '{1}'", count > 0 && count < filter.brand.Count ? "OR" : "", b.code));
                                    count += 1;
                                }
                            }
                            sqlBrandFilter.AppendLine(" )");
                        }
                    }

                    StringBuilder sqlGenderFilter = new StringBuilder();
                    if (filter.gender.Count > 0) {
                        if (filter.gender.Count(a => a.isselected == true) > 0) {
                            isWhere = true;
                            sqlGenderFilter.AppendLine("AND (");
                            int count = 0;
                            foreach (Gender g in filter.gender) {
                                if (g.isselected == true) {
                                    sqlGenderFilter.AppendLine(string.Format(@"{0} gender_code = '{1}'", count > 0 && count < filter.gender.Count ? "OR" : "", g.code));
                                    count += 1;
                                }
                            }
                            sqlGenderFilter.AppendLine(" )");
                        }
                    }
                    sqlFilterQuery = sqlBrandFilter + "" + sqlGenderFilter;
                }
            }
            List<ProductData> xx = new List<ProductData>();
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + productDataBase))) {
                connection.Open();
                string sql = string.Format(@"SELECT style, imageurl, isnew, category_code, outlet, isnew, supplier, brand, shortdesc_en, gender_en, brand_code, gender_code, shortdesc_hr, price_min FROM style
                                            {5} {2} {3} {4} {6} LIMIT {0}  OFFSET {1}", limit, (page - 1) * limit, sqlCategoryQuery, sqlSearchQuery, sqlFilterQuery, groupQuery, OrderBy(sort, order));
                using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                    using (SQLiteDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            ProductData x = GetProductData(reader, priceCoeff);
                            if (sc.Exists(a => a.code.ToLower() == x.category_code.ToLower())) {
                                if (sc.Find(a => a.code.ToLower() == x.category_code.ToLower()).isselected == true) {
                                    xx.Add(x);
                                }
                            }
                        }
                    }    
                }
                xxx.products = xx;
                xxx.response.count = GetCount(string.Format(@"
                                    SELECT COUNT(style) FROM style {0} {1} {2} {3}", groupQuery, sqlCategoryQuery, sqlSearchQuery, sqlFilterQuery), connection);
                connection.Close();
            }
            xxx.response.maxPrice = xx.Count > 0 ? xx.Max(a => a.price_min.net) : 0;
            xxx.response.time = DateTime.Now.ToString();
            xxx.response.responseTime = stopwatch.Elapsed.TotalSeconds;
            return JsonConvert.SerializeObject(xxx, Formatting.None);
        } catch (Exception e) { return e.Message; }
    }

    [WebMethod]
    public string GetProduct(string style, string color) {
        try {
            Price.PriceCoeff priceCoeff = pr.GetCoeff();
            ProductData x = new ProductData();
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + productDataBase))) {
                connection.Open();
                string sqlQuery = string.IsNullOrWhiteSpace(color) ? "GROUP BY p.style" : string.Format("AND p.colorname = '{0}'", color.Replace("%20", " "));
                string sql = string.Format(@"SELECT p.sku, p.colorname, p.size, p.style, p.brand, p.modelimageurl, p.shortdesc_en, p.longdesc_en, p.gender_en, p.category_en, p.colorhex, p.colorgroup_id, p.isnew, st.sizes, st.colors, s.price, p.colorimageurl, p.packshotimageurl, st.carelabels_en, st.carelabellogos, p.category_code, p.brand_code, st.specimageurl, p.gender_code, st.outlet, st.isnew, t.shortdesc_hr, t.longdesc_hr, p.supplier, MIN(s.price) FROM product p
                                            LEFT OUTER JOIN style st
                                            ON p.style = st.style                                    
                                            LEFT OUTER JOIN stock s
                                            ON p.sku = s.sku
                                            LEFT OUTER JOIN translation t
                                            ON p.style = t.style
                                            WHERE p.style = '{0}' {1}", style, sqlQuery);
                using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                    using (SQLiteDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            x.supplier = reader.GetValue(28) == DBNull.Value ? "" : reader.GetString(28);
                            x.sku = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                            x.colorname = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                            x.size = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2);
                            x.style = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3);
                            x.brand = reader.GetValue(4) == DBNull.Value ? "" : reader.GetString(4);
                            x.modelimageurl = reader.GetValue(5) == DBNull.Value ? "" : GetImgUrl(reader.GetString(5), x.supplier);
                            x.shortdesc_en = reader.GetValue(6) == DBNull.Value ? "" : reader.GetString(6);
                            x.longdesc_en = reader.GetValue(7) == DBNull.Value ? null : reader.GetString(7).Split(reader.GetString(7).Contains("|") ? '|' : ';');
                            x.gender_en = reader.GetValue(8) == DBNull.Value ? "" : reader.GetString(8);
                            x.category_en = reader.GetValue(9) == DBNull.Value ? "" : reader.GetString(9);
                            x.colorhex = reader.GetValue(10) == DBNull.Value ? "" : reader.GetString(10);
                            x.colorgroup_id = reader.GetValue(11) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(11));
                            x.isnew = reader.GetValue(25) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(25));
                            x.sizes = reader.GetValue(13) == DBNull.Value ? "" : reader.GetString(13);
                            x.colors = reader.GetValue(14) == DBNull.Value ? "" : reader.GetString(14);
                            x.uttprice = reader.GetValue(15) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(15));
                            x.colorimageurl = reader.GetValue(16) == DBNull.Value ? "" : GetImgUrl(reader.GetString(16), x.supplier);
                            x.packshotimageurl = reader.GetValue(17) == DBNull.Value ? null : GetPackshotImageList(reader.GetString(17), x.supplier);
                            x.carelabel = GetCareLabel(reader.GetValue(18) == DBNull.Value ? "" : reader.GetString(18), reader.GetValue(19) == DBNull.Value ? "" : reader.GetString(19));
                            x.category_code = reader.GetValue(20) == DBNull.Value ? "" : reader.GetString(20);
                            x.brand_code = reader.GetValue(21) == DBNull.Value ? "" : reader.GetString(21);

                            x.price_min = pr.GetPrice(
                                priceCoeff,
                                x.category_code,
                                x.brand_code,
                                x.style,
                                reader.GetValue(29) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(29)));

                            //x.price_min = pr.GetPrice(
                            //    priceCoeff,
                            //    reader.GetValue(20) == DBNull.Value ? "" : reader.GetString(20),
                            //    reader.GetValue(21) == DBNull.Value ? "" : reader.GetString(21),
                            //    x.style,
                            //    GetUttPrice(connection, x.style, "MIN"));

                            //x.price_max = pr.GetPrice(
                            //    reader.GetValue(20) == DBNull.Value ? "" : reader.GetString(20),
                            //    reader.GetValue(21) == DBNull.Value ? "" : reader.GetString(21),
                            //    x.style,
                            //    GetUttPrice(connection, x.style, "MAX"));
                            List<Stock> st = new List<Stock>();
                            x.stock = st;
                            x.size_specification = GetSizeSpecification(connection, x.style);
                            x.specimageurl = reader.GetValue(22) == DBNull.Value ? "" : reader.GetString(22);
                            x.gender_code = reader.GetValue(23) == DBNull.Value ? "" : reader.GetString(23);
                            x.piecesPerBox = GetPiecesPerBox(connection, x.style);
                            x.outlet = reader.GetValue(24) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(24));
                            x.isnew = reader.GetValue(25) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(25));
                            x.shortdesc_hr = reader.GetValue(26) == DBNull.Value ? "" : reader.GetString(26);
                            x.longdesc_hr = reader.GetValue(27) == DBNull.Value ? null : reader.GetString(27).Split(';');
                        }
                    } 
                }
                connection.Close();
            }
            return JsonConvert.SerializeObject(x, Formatting.None);
        } catch (Exception e) { return e.Message; }
    }

    [WebMethod]
    public string GetProductColorImg(string style, string color) {
        try {
            ProductData x = new ProductData();
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + productDataBase))) {
                connection.Open();
                string sqlQuery = string.Format("colorname = '{0}'", color.Replace("%20", " "));
                string sql = string.Format(@"SELECT DISTINCT supplier, packshotimageurl, modelimageurl FROM product WHERE {1} AND style = '{0}'", style, sqlQuery);
                using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                    using (SQLiteDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            x.supplier = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                            x.packshotimageurl = reader.GetValue(1) == DBNull.Value ? null : GetPackshotImageList(reader.GetString(1), x.supplier);
                            x.modelimageurl = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2);
                            List<Stock> st = new List<Stock>();
                            x.stock = st;
                        }
                    }
                }
                connection.Close();
            }
            return JsonConvert.SerializeObject(x, Formatting.None);
        } catch (Exception e) { return e.Message; }
    }

    [WebMethod]
    public string GetStock(string style, int limit, int offset) {
        try {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + productDataBase));
            connection.Open();
            List<Stock> x = GetStock(connection, style, limit, offset);
            connection.Close();
            return JsonConvert.SerializeObject(x, Formatting.None);
        } catch (Exception e) { return e.Message; }
    }

    [WebMethod]
    public string TranslateProductsFromJson() {
        try {
            Translate t = new Translate();
            string lang = "hr";
            string[] ss = t.Translations(lang);
            string keyTitle = null;
            string title = null;
            string sql = "";
            using (var connection = new SQLiteConnection(@"Data Source=" + Server.MapPath("~/App_Data/" + productDataBase))) {
                connection.Open();
                using (var command = new SQLiteCommand(connection)) {
                    using (var transaction = connection.BeginTransaction()) {
                        if (ss != null) {
                            foreach (string s in ss) {
                                string[] _s = s.Split(':');
                                if (_s.Count() == 2) {
                                    keyTitle = t.KeyTitle(_s).Replace("'", "").ToUpper();
                                    title = t.Title(s).Replace("'", "").ToUpper();
                                    sql = string.Format(@"UPDATE translation SET shortdesc_hr = '{0}'
                                                        WHERE shortdesc_en = '{1}'"
                                                        , title, keyTitle);
                                    command.CommandText = sql;
                                    command.ExecuteNonQuery();
                                }
                            }
                            transaction.Commit();
                            connection.Close();
                        }
                    }
                }
            }
            return JsonConvert.SerializeObject("Update completed successfully", Formatting.None);
        } catch (Exception e) { return e.Message; }
    }

    [WebMethod]
    public string LoadProductsTranslation() {
        try {
            return JsonConvert.SerializeObject(GetTranslations(), Formatting.None);
        } catch (Exception e) {
            return JsonConvert.SerializeObject(e.Message, Formatting.None);
        }
    }

    [WebMethod]
    public string UpdateTranslation(Translation translation) {
        try {
            using (var connection = new SQLiteConnection(@"Data Source=" + Server.MapPath("~/App_Data/" + productDataBase))) {
                connection.Open();
                using (var command = new SQLiteCommand(connection)) {
                    using (var transaction = connection.BeginTransaction()) {
                        string sql = string.Format(@"UPDATE translation SET shortdesc_hr = '{0}' WHERE shortdesc_en = '{1}'"
                                                    , translation.shortdesc_hr, translation.shortdesc_en);
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                        sql = string.Format(@"UPDATE translation SET longdesc_hr = '{0}' WHERE longdesc_en = '{1}'"
                                                    , translation.longdesc_hr, translation.longdesc_en);
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                }
                connection.Close();
            }
            return JsonConvert.SerializeObject("Update completed successfully", Formatting.None);
        } catch (Exception e) {
            return JsonConvert.SerializeObject(e.Message, Formatting.None);
        }
    }
    #endregion WebMethods

    #region Methods
    private string GetJsonFile(string folder, string filename) {
        string path = string.Format(@"~/App_Data/{0}/{1}.json", folder, filename);
        string json = "";
        if (File.Exists(Server.MapPath(path))) {
            json = File.ReadAllText(Server.MapPath(path));
        } return json;
    }

    private string RequestData(string url) {
        // Create a request for the URL.  
        WebRequest request = WebRequest.Create(url);
        // If required by the server, set the credentials.  
        request.Credentials = CredentialCache.DefaultCredentials;
        // Get the response.  
        WebResponse response = request.GetResponse();
        // Display the status.  
        Console.WriteLine(((HttpWebResponse)response).StatusDescription);
        // Get the stream containing content returned by the server.  
        Stream dataStream = response.GetResponseStream();
        // Open the stream using a StreamReader for easy access.  
        StreamReader reader = new StreamReader(dataStream);
        // Read the content.  
        string responseFromServer = reader.ReadToEnd();
        // Display the content.  
        Console.WriteLine(responseFromServer);
        // Clean up the streams and the response.  
        reader.Close();
        response.Close();
        return responseFromServer;
    }

    public List<Stock> GetStockUtt() {
        try {
            var stopwatch = new Stopwatch();
            string supplier = "utt";
            string sql = "";
            List<Stock> stock = new List<Stock>();
            stock = JsonConvert.DeserializeObject<List<Stock>>(RequestData("https://www.utteam.com/api/dataexport/b102f37bc6e73a7d59e12828a92f26f3?action=stock&format=json&variables=&fields=style,color,size,sku,uttstock,suppstock,price,specialprice,specialstart,specialend,currency,uom"));
            db.CreateDataBase(productDataBase, db.stock);
            using (var connection = new SQLiteConnection(
               @"Data Source=" + Server.MapPath("~/App_Data/" + productDataBase))) {
                connection.Open();
                using (var command = new SQLiteCommand(connection)) {
                    using (var transaction = connection.BeginTransaction()) {
                        string sql_delete = string.Format("DELETE FROM stock WHERE supplier = '{0}';", supplier);
                        command.CommandText = sql_delete;
                        command.ExecuteNonQuery();
                        foreach (Stock s in stock) {
                           sql = string.Format(@"INSERT INTO stock (style, color, size, sku, uttstock, suppstock, price, specialprice, specialstart, specialend, currency, uom, supplier)
                                                VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}')"
                                                 , s.style, s.color, s.size, s.sku, s.uttstock, s.suppstock, s.price, s.specialprice
                                                 , s.specialstart, s.specialend, s.currency, s.uom, supplier);
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                }
                connection.Close();
            } return stock;
        } catch (Exception e) {
            return null;
        }
    }

    public List<Stock> GetSkuStockUtt(List<Cart.NewCart> cart) {
        try {
            var stopwatch = new Stopwatch();
            List<Stock> stock = new List<Stock>();
            string sku = "";
            foreach (Cart.NewCart item in cart) {
                foreach(Stock variant in item.data) {
                    sku = sku + variant.sku + ",";
                }
            }
            string query = string.Format(@"https://www.utteam.com/api/dataexport/b102f37bc6e73a7d59e12828a92f26f3?action=stock&format=json&variables=sku:{0}&fields=style,color,size,sku,uttstock,suppstock,price,specialprice,specialstart,specialend,currency,uom", sku);
            stock = JsonConvert.DeserializeObject<List<Stock>>(RequestData(query));
            return stock;
        } catch (Exception e) {
            return null;
        }
    }

    private List<Stock> GetStock(SQLiteConnection connection, string style, int limit, int offset) {
        try {
            Price.PriceCoeff priceCoeff = pr.GetCoeff();
            string sql = string.Format(@"SELECT s.style, s.color, s.size, s.sku, s.uttstock, s.suppstock, s.price, s.specialprice, s.specialstart, s.specialend, s.currency, s.uom, p.colorhex, p.modelimageurl, p.shortdesc_en, p.colorimageurl, p.packshotimageurl, p.category_code, p.brand_code, p.weight, p.colorswatch, p.outlet, p.caseqty FROM stock s
                                        LEFT OUTER JOIN product p
                                        ON s.sku = p.sku                      
                                        WHERE s.style = '{0}' LIMIT {1} OFFSET {2}", style, limit, offset);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<Stock> xx = new List<Stock>();
            while (reader.Read()) {
                Stock x = new Stock();
                x.style = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                x.color = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                x.size = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2);
                x.sku = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3);
                x.uttstock = reader.GetValue(4) == DBNull.Value ? "" : reader.GetString(4);
                x.suppstock = reader.GetValue(5) == DBNull.Value ? "" : reader.GetString(5);
                x.price = reader.GetValue(6) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(6));
                x.specialprice = reader.GetValue(7) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(7));
                x.specialstart = reader.GetValue(8) == DBNull.Value ? "" : reader.GetString(8);
                x.specialend = reader.GetValue(9) == DBNull.Value ? "" : reader.GetString(9);
                x.currency = reader.GetValue(10) == DBNull.Value ? "" : reader.GetString(10);
                x.uom = reader.GetValue(11) == DBNull.Value ? "" : reader.GetString(11);
                x.colorhex = reader.GetValue(12) == DBNull.Value ? "" : reader.GetString(12);
                x.modelimageurl = reader.GetValue(13) == DBNull.Value ? "" : reader.GetString(13);
                x.shortdesc_en = reader.GetValue(14) == DBNull.Value ? "" : reader.GetString(14);
                x.colorimageurl = reader.GetValue(15) == DBNull.Value ? "" : reader.GetString(15);
                x.packshotimageurl = reader.GetValue(16) == DBNull.Value ? null : reader.GetString(16).Replace(" /", "/").Split('|');
                x.category_code = reader.GetValue(17) == DBNull.Value ? "" : reader.GetString(17);
                x.brand_code = reader.GetValue(18) == DBNull.Value ? "" : reader.GetString(18);
                x.weight = reader.GetValue(19) == DBNull.Value ? "" : reader.GetString(19);
                x.colorswatch = reader.GetValue(20) == DBNull.Value ? "" : reader.GetString(20);
                x.outlet = reader.GetValue(21) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(21));
                x.caseqty = reader.GetValue(22) == DBNull.Value ? "" : reader.GetString(22);
                x.quantity = 0;
                x.myprice = pr.GetPrice(priceCoeff, x.category_code, x.brand_code, x.style, x.price);
                xx.Add(x);
            }
            return xx;
        } catch (Exception e) {
            return null;
        }
    }

    //-----------------TODO group stock by color
    [WebMethod]
    public string GetStockGroupedByColor(string style) {
        try {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + productDataBase));
            connection.Open();
            Object x = GetStockGroupedByColor(connection, style);
            connection.Close();
            return JsonConvert.SerializeObject(x, Formatting.None);
        } catch (Exception e) { return e.Message; }
    }

    private Object GetStockGroupedByColor(SQLiteConnection connection, string style) {
        try {
            Price.PriceCoeff priceCoeff = pr.GetCoeff();
            string sql = string.Format(@"SELECT s.style, s.color, s.size, s.sku, s.uttstock, s.suppstock, s.price, s.specialprice, s.specialstart, s.specialend, s.currency, s.uom, p.colorhex, p.modelimageurl, p.shortdesc_en, p.colorimageurl, p.packshotimageurl, p.category_code, p.brand_code, p.weight, p.colorswatch, p.outlet, p.caseqty, p.supplier FROM stock s
                                        LEFT OUTER JOIN product p
                                        ON s.sku = p.sku                      
                                        WHERE s.style = '{0}'", style);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<Stock> xx = new List<Stock>();
            while (reader.Read()) {
                Stock x = new Stock();
                x.supplier = reader.GetValue(23) == DBNull.Value ? "" : reader.GetString(23);
                x.style = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                x.color = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                x.size = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2);
                x.sku = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3);
                x.uttstock = reader.GetValue(4) == DBNull.Value ? "" : reader.GetString(4);
                x.suppstock = reader.GetValue(5) == DBNull.Value ? "" : reader.GetString(5);
                x.price = reader.GetValue(6) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(6));
                x.specialprice = reader.GetValue(7) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(7));
                x.specialstart = reader.GetValue(8) == DBNull.Value ? "" : reader.GetString(8);
                x.specialend = reader.GetValue(9) == DBNull.Value ? "" : reader.GetString(9);
                x.currency = reader.GetValue(10) == DBNull.Value ? "" : reader.GetString(10);
                x.uom = reader.GetValue(11) == DBNull.Value ? "" : reader.GetString(11);
                x.colorhex = reader.GetValue(12) == DBNull.Value ? "" : reader.GetString(12);
                x.modelimageurl = reader.GetValue(13) == DBNull.Value ? "" : GetImgUrl(reader.GetString(13), x.supplier);
                x.shortdesc_en = reader.GetValue(14) == DBNull.Value ? "" : reader.GetString(14);
                x.colorimageurl = reader.GetValue(15) == DBNull.Value ? "" : GetImgUrl(reader.GetString(15), x.supplier);
                x.packshotimageurl = reader.GetValue(16) == DBNull.Value ? null : GetPackshotImageList(reader.GetString(16), x.supplier);
                x.category_code = reader.GetValue(17) == DBNull.Value ? "" : reader.GetString(17);
                x.brand_code = reader.GetValue(18) == DBNull.Value ? "" : reader.GetString(18);
                x.weight = reader.GetValue(19) == DBNull.Value ? "" : reader.GetString(19);
                x.colorswatch = reader.GetValue(20) == DBNull.Value ? "" : reader.GetString(20);
                x.outlet = reader.GetValue(21) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(21));
                x.caseqty = reader.GetValue(22) == DBNull.Value ? "" : reader.GetString(22);
                x.quantity = 0;
                x.myprice = pr.GetPrice(priceCoeff, x.category_code, x.brand_code, x.style, x.price);
                xx.Add(x);
            }

            var aa = new Object();
            aa = (from p in xx
                  group p by new { p.color, p.colorhex }
                             into g
                  select new {
                      color = g.Key.color,
                      colorhex = g.Key.colorhex,
                      stock = xx.Where(a => a.color == g.Key.color)
                                .OrderByDescending(a => a.size == "XS")
                                .ThenByDescending(a => a.size == "S")
                                .ThenByDescending(a => a.size == "M")
                                .ThenByDescending(a => a.size == "L")
                                .ThenByDescending(a => a.size == "XL")
                                .ThenBy(a => a.size)
                                .ToList()
                  }).ToList();

            return aa;
        } catch (Exception e) {
            return null;
        }
    }

    public class GroupedStock {
        string color { get; set; }
        List<Stock> stock = new List<Stock>();
    }
    //--------------------------------------


    private List<ColorGroup> GetColorChild(List<ColorGroup> allColorGroups, string colorfamily_en) {
        List<ColorGroup> xx = new List<ColorGroup>();
        foreach(ColorGroup c in allColorGroups) {
            if(c.colorfamily_en == colorfamily_en) {
                ColorGroup x = new ColorGroup();
                x.colorfamily_en = c.colorfamily_en;
                x.colorfamily_hu = c.colorfamily_hu;
                x.colorgroupname = c.colorgroupname;
                x.colorgroup_id = c.colorgroup_id;
                xx.Add(x);
            }
        } return xx;
    }

    private List<Category> GetDistinctCategories() {
        try {
            Categories categories = new Categories();
            List<Categories.SelectedCategories> sc = categories.GetCategories();
            List<Category> xx = new List<Category>();
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + productDataBase))) {
                connection.Open();
                string sql = string.Format(@"SELECT category_en, category_code, count(style) FROM style GROUP BY category_en");
                using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                    using (SQLiteDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            Category x = new Category();
                            x.title = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                            x.code = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                            x.count = reader.GetValue(2) == DBNull.Value ? 0 : reader.GetInt32(2);
                            x.isselected = sc.Exists(a => a.code.ToLower() == x.code.ToLower()) ? sc.Find(a => a.code.ToLower() == x.code.ToLower()).isselected : false;
                            x.order = sc.Exists(a => a.code.ToLower() == x.code.ToLower()) ? sc.Find(a => a.code.ToLower() == x.code.ToLower()).order : 0;
                            if (x.isselected) {
                                xx.Add(x);
                            }
                        }
                    }    
                } 
                connection.Close();
            }   
            return xx;
        } catch (Exception e) {
            return new List<Category>();
        }
    }

    private Distinct GetDistinct(SQLiteConnection connection, string productCategory, string group, string type) {
        string query = "";
        switch (type) {
            case "brand":
                if (group != "all") {
                    query = string.Format("WHERE s.brand_code = '{0}'", group);
                }
                break;
            case "gender":
                query = string.Format("WHERE s.gender_code = '{0}'", group);
                break;
            case "isnew":
                query = string.Format("WHERE s.isnew = '{0}'", group);
                break;
        }

        //SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + productDataBase));
        //  using (connection) {
        //  connection.Open();
        if (!string.IsNullOrEmpty(productCategory)) {
            if (string.IsNullOrEmpty(query)) {
                query = string.Format("WHERE s.category_code = '{0}'", productCategory);
            } else {
                query = string.Format("{0} AND s.category_code = '{1}'", query, productCategory);
            }
        }
        string sql = string.Format(@"
                                    SELECT DISTINCT s.brand, s.brand_code FROM style s {0}", query);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<Brand> brands = new List<Brand>();
            while (reader.Read()) {
                Brand brand = new Brand();
                brand.title = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                brand.code = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                brand.isselected = false;
                if (!string.IsNullOrEmpty(brand.title)) {
                    brands.Add(brand);
                }
            }

            List<ColorGroup> allColorGroups = JsonConvert.DeserializeObject<List<ColorGroup>>(GetJsonFile("json", "colorgrouping"));

            List<ColorGroup> colorGroups = JsonConvert.DeserializeObject<List<ColorGroup>>(GetJsonFile("json", "colorgrouping"))
                                        .GroupBy(a => a.colorfamily_en).Select(g => g.First()).ToList();

            List<DistColorGroup> distinctColorGroups = new List<DistColorGroup>();
            foreach (ColorGroup colorGroup in colorGroups) {
                DistColorGroup distinctColorGroup = new DistColorGroup();
                distinctColorGroup.colorfamily_en = colorGroup.colorfamily_en;
                distinctColorGroup.colorchild = GetColorChild(allColorGroups, colorGroup.colorfamily_en);
                distinctColorGroups.Add(distinctColorGroup);
            }


            sql = string.Format(@"
                                SELECT DISTINCT p.size FROM product p
                                LEFT OUTER JOIN style s
                                ON p.style = s.style 
                                {0}", query);
            command = new SQLiteCommand(sql, connection);
            reader = command.ExecuteReader();
            List<Size> sizes = new List<Size>();
            while (reader.Read()) {
                Size size = new Size();
                size.title = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                size.isselected = false;
                 if (!string.IsNullOrEmpty(size.title)) {
                sizes.Add(size);
                }
            }
   

        sql = string.Format(@"SELECT DISTINCT s.gender_en, s.gender_code FROM style s {0}", query);
            command = new SQLiteCommand(sql, connection);
            reader = command.ExecuteReader();
            List<Gender> genders = new List<Gender>();
            while (reader.Read()) {
                Gender gender = new Gender();
                gender.title = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                gender.code = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                gender.isselected = false;
                if (!string.IsNullOrEmpty(gender.title)) {
                    genders.Add(gender);
                }
            }

        Distinct xx = new Distinct();
            xx = new Distinct();
            xx.brand = brands;
            xx.colorGroup = distinctColorGroups;
            xx.size = sizes;
            xx.gender = genders;

          //  connection.Close();
       // }
        
        return xx;
    }

    /*  //OLD
    private Distinct GetDistinct(SQLiteConnection connection, string productCategory, string group, string type) {
        string query = "";
        switch (type) {
            case "brand":
                if (group != "all") {
                    query = string.Format("WHERE p.brand_code = '{0}'", group);
                }
                break;
            case "gender":
                query = string.Format("WHERE p.gender_code = '{0}'", group);
                break;
            case "isnew":
                query = string.Format("WHERE st.isnew = '{0}'", group);
                break;
        }

        //SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + productDataBase));
      //  using (connection) {
          //  connection.Open();
            if (!string.IsNullOrEmpty(productCategory)) {
                query = string.Format("WHERE p.category_code = '{0}'", productCategory);
            }
            string sql = string.Format(@"
                                    SELECT DISTINCT p.brand, p.brand_code FROM product p
                                    LEFT OUTER JOIN style st
                                    ON p.style = st.style    
                                    {0}", query);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<Brand> brands = new List<Brand>();
            while (reader.Read()) {
                Brand brand = new Brand();
                brand.title = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                brand.code = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                brand.isselected = false;
                brands.Add(brand);
            }

            List<ColorGroup> allColorGroups = JsonConvert.DeserializeObject<List<ColorGroup>>(GetJsonFile("json", "colorgrouping"));

            List<ColorGroup> colorGroups = JsonConvert.DeserializeObject<List<ColorGroup>>(GetJsonFile("json", "colorgrouping"))
                                        .GroupBy(a => a.colorfamily_en).Select(g => g.First()).ToList();

            List<DistColorGroup> distinctColorGroups = new List<DistColorGroup>();
            foreach (ColorGroup colorGroup in colorGroups) {
                DistColorGroup distinctColorGroup = new DistColorGroup();
                distinctColorGroup.colorfamily_en = colorGroup.colorfamily_en;
                distinctColorGroup.colorchild = GetColorChild(allColorGroups, colorGroup.colorfamily_en);
                distinctColorGroups.Add(distinctColorGroup);
            }

            sql = string.Format(@"
                                SELECT DISTINCT p.size FROM product p
                                LEFT OUTER JOIN style st
                                ON p.style = st.style 
                                {0}", query);
            command = new SQLiteCommand(sql, connection);
            reader = command.ExecuteReader();
            List<Size> sizes = new List<Size>();
            while (reader.Read()) {
                Size size = new Size();
                size.title = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                size.isselected = false;
                sizes.Add(size);
            }

            sql = string.Format(@"
                                SELECT DISTINCT p.gender_en, p.gender_code FROM product p
                                LEFT OUTER JOIN style st
                                ON p.style = st.style 
                                {0}", query);
            command = new SQLiteCommand(sql, connection);
            reader = command.ExecuteReader();
            List<Gender> genders = new List<Gender>();
            while (reader.Read()) {
                Gender gender = new Gender();
                gender.title = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                gender.code = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                gender.isselected = false;
                genders.Add(gender);
            }

            Distinct xx = new Distinct();
            xx = new Distinct();
            xx.brand = brands;
            xx.colorGroup = distinctColorGroups;
            xx.size = sizes;
            xx.gender = genders;

          //  connection.Close();
       // }
        
        return xx;
    }
    */

    private List<CareLabel> GetCareLabel(string careLabels, string logos ) {
        string[] labelList = careLabels.Split('|');
        string[] logoList = logos.Split('|');
        List<CareLabel> xx = new List<CareLabel>();
        int idx = 0;
        foreach (var s in labelList) {
            CareLabel x = new CareLabel();
            x.carelabels_en = s;
            x.carelabellogos = logoList[idx];
            idx += 1;
            xx.Add(x);
        }
        return xx;
    }

    private int GetCount(string sql, SQLiteConnection connection ) {
        int count = 0;
        using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
            using (SQLiteDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    count = reader.GetValue(0) == DBNull.Value ? 0 : reader.GetInt32(0);
                }
            };
        };
        return count;
    }

    /*
    private double GetUttPrice(SQLiteConnection connection, string style, string type) {
        try {
            string sql = string.Format(@"SELECT {0}(s.price) FROM stock s
                                        WHERE s.style = '{1}'", type, style);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            double x = 0;
            while (reader.Read()) {
                x = reader.GetValue(0) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(0));
            }
            return x;
        } catch (Exception e) {
            return 0;
        }
    }
    */


    private string OrderBy(string sort, string order) {
        string sql = "";
        if (!string.IsNullOrEmpty(sort)) {
            if (sort == "price") {
                //sql = string.Format(@"ORDER BY p.supplier DESC, CAST(s.{0} AS DOUBLE) {1}", sort, order);
                sql = string.Format(@"ORDER BY CAST(price_min AS DOUBLE) {0}", order);
            } else {
                sql = string.Format(@"ORDER BY supplier DESC, {0} {1}", sort, order);
                //sql = string.Format(@"ORDER BY p.supplier DESC, p.{0} {1}", sort, order);
            }
        }
        return sql;
    }

    private List<SizeSpecification> GetSizeSpecification(SQLiteConnection connection, string style) {
        try {
            string sql = string.Format("SELECT size, name_en, value FROM size WHERE style = '{0}'", style);
            List<SizeSpecification> xx = new List<SizeSpecification>();
            using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                using (SQLiteDataReader reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        SizeSpecification x = new SizeSpecification();
                        x.size = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                        x.name_en = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                        x.value = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2);
                        xx.Add(x);
                    }
                }
            } 
            return xx;
        } catch (Exception e) {
            return new List<SizeSpecification>();
        }
    }

    private List<CaseQty> GetPiecesPerBox(SQLiteConnection connection, string style) {
        try {
            string sql = string.Format("SELECT DISTINCT size, caseqty FROM product WHERE style = '{0}'", style);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<CaseQty> xx = new List<CaseQty>();
            while (reader.Read()) {
                CaseQty x = new CaseQty();
                x.size = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                x.qty = reader.GetValue(1) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(1));
                xx.Add(x);
            }
            var aa = new List<CaseQty>();
            aa = xx.Where(a => a.size != "")
                    .OrderByDescending(a => a.size == "XS")
                    .ThenByDescending(a => a.size == "S")
                    .ThenByDescending(a => a.size == "M")
                    .ThenByDescending(a => a.size == "L")
                    .ThenByDescending(a => a.size == "XL")
                    .ThenBy(a => a.size)
                    .ToList();

            return aa;
        } catch (Exception e) {
            return new List<CaseQty>();
        }
    }

    private int GetOutlet(SQLiteConnection connection, string style) {
        try {
            string sql = string.Format("SELECT outlet FROM product WHERE style = '{0}' AND outlet = '{1}' LIMIT 1", style, 1);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            int x = 0;
            while (reader.Read()) {
                x = reader.GetValue(0) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(0));
            }
            return x;
        } catch (Exception e) {
            return 0;
        }
    }

    private void SaveDdb(List<Product> xx, List<Stock> yy, List<Style> distinctStyle) {
         string sql_delete = "";
            string sql = "";
            db.CreateDataBase(productDataBase, db.product);
            db.CreateDataBase(productDataBase, db.style);
            db.CreateDataBase(productDataBase, db.stock);
            db.CreateDataBase(productDataBase, db.translation);
            List<Translation> tt = GetTranslations();
            using (var connection = new SQLiteConnection(@"Data Source=" + Server.MapPath("~/App_Data/" + productDataBase))) {
                connection.Open();
                using (var command = new SQLiteCommand(connection)) {
                    using (var transaction = connection.BeginTransaction()) {
                        sql_delete = string.Format("DELETE FROM product WHERE supplier = '{0}';", xx[0].supplier);
                        command.CommandText = sql_delete;
                        command.ExecuteNonQuery();
                        foreach (Product p in xx) {
                                sql = string.Format(@"INSERT OR REPLACE INTO product (sku, colorname, size, style, brand, modelimageurl, shortdesc_en, longdesc_en, gender_en, category_en, colorhex, colorgroup_id, isnew, colorimageurl, packshotimageurl, category_code, brand_code, gender_code, weight, colorswatch, outlet, caseqty, supplier)
                                                    VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}')"
                                                    , p.sku, p.colorname, p.size, p.style, p.brand.Replace("'", ""), p.modelimageurl, p.shortdesc_en.Replace("'", ""), p.longdesc_en.Replace("'", ""), p.gender_en.Replace("'", "")
                                                    , p.category_en.Replace("&", "and"), p.colorhex, p.colorgroup_id, p.isnew, p.colorimageurl, p.packshotimageurl
                                                    , p.category_en.Replace("&", "And").Replace(" ", ""), p.brand.Replace("&", "And").Replace(" ", "").Replace("'", "")
                                                    , p.gender_en.Replace(" ", "").Replace("'", ""), p.weight, p.colorswatch, p.outlet, p.caseqty, p.supplier);
                                command.CommandText = sql;
                                command.ExecuteNonQuery();
                            }
                        sql_delete = string.Format("DELETE FROM stock WHERE supplier = '{0}';", yy[0].supplier);
                        command.CommandText = sql_delete;
                        command.ExecuteNonQuery();
                        foreach (Stock s in yy) {
                                sql = string.Format(@"INSERT INTO stock (style, color, size, sku, uttstock, suppstock, price, specialprice, specialstart, specialend, currency, uom, supplier)
                                                    VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}')"
                                                    , s.style, s.color, s.size, s.sku, s.uttstock, s.suppstock, s.price
                                                    , s.specialprice, s.specialstart , s.specialend, s.currency, s.uom, s.supplier);
                                command.CommandText = sql;
                                command.ExecuteNonQuery();
                            }
                        sql_delete = string.Format("DELETE FROM style WHERE supplier = '{0}';", distinctStyle[0].supplier);
                        command.CommandText = sql_delete;
                        command.ExecuteNonQuery();

                        foreach (Style s in distinctStyle) {
                            var prod = xx.Find(a => a.style == s.style);
                            double price_min = yy.Where(a => a.style == s.style).Min(a => a.price);
                            var tran = tt.Find(a => a.style == s.style);
                            sql = string.Format(@"INSERT OR REPLACE INTO style (style, gsmweight, sizes, colors, outlet, coo, imageurl, altimageurl, fabric_en, cut_en, details_en, carelabels_en, carelabellogos, category_en, category_code, specimageurl, isnew, supplier, brand, shortdesc_en, gender_en, brand_code, gender_code, shortdesc_hr, price_min)
                                                    VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}')"
                                                , s.style, s.gsmweight, s.sizes, s.colors, s.outlet, s.coo, s.imageurl, s.altimageurl, s.fabric_en, s.cut_en
                                                , s.details_en, s.carelabels_en, s.carelabellogos, s.category_en.Replace("&", "and")
                                                , s.category_en.Replace("&", "And").Replace(" ", ""), s.specimageurl, s.isnew, s.supplier
                                                , prod.brand.Replace("'", ""), prod.shortdesc_en.Replace("'", ""), prod.gender_en.Replace(" ", "").Replace("'", ""), prod.brand.Replace("&", "And").Replace(" ", "").Replace("'", ""), prod.gender_en.Replace(" ", "").Replace("'", "")
                                                , tran != null ? tran.shortdesc_hr : "", price_min);
                            command.CommandText = sql;
                            command.ExecuteNonQuery();

                            string sql_tran = string.Format(@"INSERT OR IGNORE INTO translation (style, shortdesc_en, longdesc_en, category_en, supplier)
                                                    VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')"
                                               , s.style, prod.shortdesc_en.Replace("'", ""), prod.longdesc_en.Replace("'", "")
                                               , s.category_en.Replace("&", "and"), s.supplier);
                            command.CommandText = sql_tran;
                            command.ExecuteNonQuery();
                        }




                    /*
                    foreach (Style s in distinctStyle) {
                                sql = string.Format(@"INSERT OR REPLACE INTO style (style, gsmweight, sizes, colors, outlet, coo, imageurl, altimageurl, fabric_en, cut_en, details_en, carelabels_en, carelabellogos, category_en, category_code, specimageurl, isnew, supplier)
                                                    VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}')"
                                                    , s.style, s.gsmweight, s.sizes, s.colors, s.outlet, s.coo, s.imageurl, s.altimageurl, s.fabric_en, s.cut_en
                                                    , s.details_en, s.carelabels_en, s.carelabellogos, s.category_en.Replace("&", "and")
                                                    , s.category_en.Replace("&", "And").Replace(" ", ""), s.specimageurl, s.isnew, s.supplier);
                                command.CommandText = sql;
                                command.ExecuteNonQuery();
                            }
                        foreach (Product p in xx) {
                            sql = string.Format(@"INSERT OR IGNORE INTO translation (sku, shortdesc_en, longdesc_en, category_en, supplier)
                                                VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')"
                                                , p.sku, p.shortdesc_en.Replace("'", ""), p.longdesc_en.Replace("'", "")
                                                , p.category_en.Replace("&", "and"), p.supplier);
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }
                        */


                        transaction.Commit();
                    }
                }
                connection.Close();
            }
    }

    private string GetColor(string sku) {
        string[] x = sku.Split('/');
        string color = "";
        if (x.Length > 1) {
            color = ""; // x[x.Length-1];
        } else {
            if (x[0].Length > 7) {
                color = x[0].Substring(5, 2);
            }
        }
        return color;
    }

    private string GetSize(string sku) {
        string[] x = sku.Split('/');
        string size = "";
        if (x.Length > 1) {
            size = x[x.Length-1];
        } else {
            if(sku.Length > 6) {
                size = sku.Substring(7);
            }
        }
        return size;
    }

    private string GetStyle(string sku) {
        string style = sku;
        string[] x = sku.Split('/');
        if (x.Length > 1) {
            style = x[0];
        } else {
            if (x[0].Length > 6) {
                style = x[0].Substring(0, 5);
            }
        }
        return style;
    }

    private string GetGender(string sku) {
        string[] x = sku.Split('/');
        string gender = "";
        if (x.Length == 4) {
            if (x[1] == "ŽM") { gender = "Womens Clothing"; }
            if (x[1] == "MM") { gender = "Mens Clothing"; }
        }
        return gender;
    }

    private string GetSizes(List<Product> products) {
        string sizes = null; 
        if(products.Count > 0) {
            List<string> xx = new List<string>();
            foreach (Product p in products) {
                if(p.size != null) {
                    xx.Add(p.size);
                }
            }
            if (xx.Count > 0) {
                List<string> distinct = xx.Distinct().ToList();
                sizes = string.Join<string>("|", distinct);
            }
        }
        return sizes;
    }

    private string GetColors(List<Product> products) {
        string colors = null; 
        if(products.Count > 0) {
            List<string> xx = new List<string>();
            foreach (Product p in products) {
                if (p.colorname != null) {
                    xx.Add(p.colorname);
                }
            }
            if (xx.Count > 0) {
                List<string> distinct = xx.Distinct().ToList();
                colors = string.Join<string>("|", distinct);
            }
        }
        return colors;
    }

    private string GetColorHex(string color) {

        string x = "";
        switch (color) {
            case "YE": x = "#f4f142"; break;
            case "OR": x = "#f47742"; break;
            case "MF": x = "#f47742"; break;
            case "KG": x = "#17a82a"; break;
            case "0K": x = "#f47742"; break;
            case "LB": x = "#69c2db"; break;
            case "RB": x = "#1f0a7f"; break;
            case "RD": x = "#f21202"; break;
            case "HG": x = "#afb5b4"; break;
            case "NY": x = "#2d2c30"; break;
            case "BK": x = "#2d2d2d"; break;
            case "WH": x = "#fff"; break;
        }

        return x;

        //TODO:
//        50
//10
//MF
//0K
//0L
//YE
//OR
//KG
//LB
//GX
//BX
//XX
//YX
//DX
//RX
//CX
//8X
//TC
//TS
//TZ
//TP
//BI
//PI
//JI
//RD
//0H
//0R
//0N
//0B
//0W
//RB
//HG
//NY
//BK
//WH
//00
//L4
//LF
//LC
//LT
//MC
//MT
//H4
//JB
//JC
//JN
//PN
//PC
//PB
//TY
//PS
//JS
//OW
//JP
//PP
//BS
//BP
//G4
//9X
//3X
//- O
//ŽN
//MN
//BE
//PW
//BC
//BW
//JW
//OG
//BB
//IN
//MM
//DG
//FG
//JG
//JR
//BG
//BR
//PG
//PR
//ŽW
//ŽO
//ŽB
//MW
//MO
//MB
//LS
//HS
//HO
//WP
//WB
//WJ
//UW
//UB
//ŽG
//MG
//ko
//7X
//7 -
//8 -
//6 -
//2X
//0X
//GJ
//GP
//GB
//OB
//OY
//NB
//NG
    }

    private string[] GetPackshotImageUrl(SQLiteConnection connection, string value, string style) {
        if (string.IsNullOrEmpty(value)) {
            string sql = string.Format(@"SELECT packshotimageurl from product WHERE style = '{0}' AND packshotimageurl <> '' LIMIT 1", style);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            string[] packshotimageurl = null;
            while (reader.Read()) {
                packshotimageurl = reader.GetValue(0) == DBNull.Value ? null : reader.GetString(0).Replace(" /", "/").Split('|');
            }
            return packshotimageurl;
        } else {
            return value.Replace(" /", "/").Split('|');
        }
    }

    private string GetImgUrl(string img, string supplier) {
        string path = img;
        if (!string.IsNullOrEmpty(img)) {
            switch (supplier.ToLower()) {
                case "utt": path = string.Format("./assets/img/{0}/products{1}", supplier, img);
                    break;
                case "cc": path = string.Format("./assets/img/{0}/products/{1}", supplier, img);
                    break;
                case "lacuna": path = img.Replace("http://", "https://");
                    break;
            }
        }
        return path;
    }

    private string[] GetPackshotImageList(string img, string supplier) {
        if (!string.IsNullOrEmpty(img)) {
            string[] list = null;
            list = img.Replace(" /", "/").Split('|');
            string[] list_ = new string[list.Count()];
            int i = 0;
            foreach (string s in list) {
                list_[i] = GetImgUrl(s, supplier);
                i++;
            }
            return list_;
        } else {
            return null;
        }
    }

    private string GetCategory(string category) {
        string x = null;
        switch (category) {
            case "T-Shirts":
            case "Shirts, T-Shirts":
            case "Shirts":
            case "Sweats, T-Shirts":
                x = "T-Shirt";
                break;
            case "Workwear":
            case "Shoes, Workwear":
            case "Sweats, Workwear":
            case "Polos, Workwear":
            case "Fleece, Workwear":
            case "Jackets, Workwear":
            case "T-Shirts, Workwear":
            case "Polos, Sweats, Workwear":
            case "Underwear, Workwear":
            case "Shirts, Workwear":
            case "Accessories, business, Shirts, Workwear":
            case "Bags, Workwear":
            case "Accessories, Shirts, Workwear":
            case "Accessories, Workwear":
            case "Caps and Headwear, Workwear":
            case "Accessories, Shoes":
                x = "Workwear";
                break;
            case "Pants, Sports":
            case "Accessories, Sports":
            case "Sports":
            case "Sports, Underwear":
            case "Pants, Underwear":
            case "Bags, Sports":
            case "Polos, Sports":
            case "Jackets, Sports":
            case "Frottier, Sports":
            case "Sports, Sweats":
            case "Shirts, Sports":
            case "Jackets, Pants, Sports":
            case "Accessories, Caps and Headwear, Sports":
                x = "Sport";
                break;
            case "Polos":
            case "Polos, T-Shirts":
            case "Polos, Sweats":
                x = "Poloshirt";
                break;
            case "Sweats":
            case "Pullover":
                x = "Sweatshirt";
                break;
            case "business, Jackets":
            case "business, Pants":
            case "Accessories, business":
            case "Accessories, business, Shirts":
            case "business":
                x = "Corporate Wear";
                break;
            case "Pants":
            case "Pants, Sports, Sweats":
            case "Accessories":
            case "business, Underwear":
            case "Pants, Sweats":
            case "Underwear":
            case "Pants, Sports, Underwear":
            case "Accessories, Sports, Underwear":
                x = "Trousers and Accessories";
                break;
            case "Frottier":
                x = "Towel";
                break;
            case "Jackets":
                x = "Jacket";
                break;
            case "Caps and Headwear":
            case "Accessories, Caps and Headwear":
            case "Caps and Headwear, Sports":
            case "Caps and Headwear, Fleece":
                x = "Cap";
                break;
            case "Bags":
            case "Umbrellas":
                x = "Bag";
                break;
            case "Baby":
            case "Baby, T-Shirts":
            case "Baby, Frottier":
                x = "Kid";
                break;
            case "Fleece":
            case "Accessories, Fleece":
            case "Fleece, Jackets":
            case "Fleece, Sweats":
                x = "Polar and Softshell";
                break;

            default:
                x = category;
                break;
        }
        return x;
    }

    private bool BrandToExclude(string brand) {
        bool x = false;
        switch (brand) {
            case "Gildan Activewear":
            case "Kariban":
            case "KiMood":
            case "Premier":
            case "SOL's Bags":
            case "SOL's Collection":
                x = true;
                break;
            default:
                x = false;
                break;
        }
        return x;
    }

    private bool CategoryToExclude(string category) {
        bool x = false;
        switch (category) {
            case "Sales Support":
            case "Shirts, Traditional Costumes":
            case "Fleece, Traditional Costumes":
            case "Polos, Traditional Costumes":
            case "Caps and Headwear, Traditional Costumes":
            case "Accessories, Traditional Costumes":
            case "Caps and Headwear, Sales Support":
            case "Pullover, Traditional Costumes":
            case "Pants, Traditional Costumes":
            case "Sales Support, Shirts":
            case "Accessories, Pants, Traditional Costumes":
                x = true;
                break;
            default:
                x = false;
                break;
        }
        return x;
    }

    private List<Translation> GetTranslations() {
        List<Translation> xx = new List<Translation>();
        using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + productDataBase))) {
            connection.Open();
            string sql = "select style, shortdesc_en, shortdesc_hr, longdesc_en, longdesc_hr from translation";
            using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                using (SQLiteDataReader reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        Translation x = new Translation();
                        x.style = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                        x.shortdesc_en = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                        x.shortdesc_hr = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2);
                        x.longdesc_en = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3);
                        x.longdesc_hr = reader.GetValue(4) == DBNull.Value ? "" : reader.GetString(4);
                        xx.Add(x);
                    }
                }
            }
            connection.Close();
        }
        return xx; 
    }

    private ProductData GetProductData(SQLiteDataReader reader, Price.PriceCoeff priceCoeff) {
        ProductData x = new ProductData();
        x.supplier = reader.GetValue(6) == DBNull.Value ? "" : reader.GetString(6);
        x.style = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
        x.imageurl = reader.GetValue(1) == DBNull.Value ? "" : GetImgUrl(reader.GetString(1), x.supplier);
        x.brand = reader.GetValue(7) == DBNull.Value ? "" : reader.GetString(7);
        x.shortdesc_en = reader.GetValue(8) == DBNull.Value ? "" : reader.GetString(8);
        x.gender_en = reader.GetValue(9) == DBNull.Value ? "" : reader.GetString(9);
        x.isnew = reader.GetValue(2) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(2));
        x.category_code = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3);
        x.brand_code = reader.GetValue(10) == DBNull.Value ? "" : reader.GetString(10);
        x.gender_code = reader.GetValue(11) == DBNull.Value ? "" : reader.GetString(11);
        x.stock = new List<Stock>();
        x.outlet = reader.GetValue(4) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(4));  //  GetOutlet(connection, x.style);
        x.isnew = reader.GetValue(5) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(5));
        x.shortdesc_hr = reader.GetValue(12) == DBNull.Value ? "" : reader.GetString(12);
        double price_min = reader.GetValue(13) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(13));
        x.price_min = pr.GetPrice(priceCoeff, x.category_code, x.brand_code, x.style, price_min);
        return x;
    }
    #endregion Methods

}
