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
    //Translate t = new Translate();

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

    #endregion Class

    #region WebMethods

    [WebMethod]
    public string ExportCsv(string file) {
        try {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double time = 0;
            List<Product> xx = new List<Product>();
            Product x = new Product();
            // string path_ = @"C:\Users\Dragan\Documents\Igor\my\pt\files\products.csv";  //Path.GetFullPath(path)

            //string filePath = System.IO.Path.GetFullPath("TestFile.txt");
            //StreamReader sr = new StreamReader(filePath);

            string path = Server.MapPath(string.Format("~/upload/{0}.csv", file)); // Path.GetFullPath(path);

            using (var reader = new StreamReader(path)) {
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
                        x.supplier = values[19];
                        xx.Add(x);
                    }
                }
            }

            string sql_delete = "";
            string sql = "";
            db.CreateDataBase(productDataBase, db.product);
            db.CreateDataBase(productDataBase, db.translation);
            using (var connection = new SQLiteConnection(@"Data Source=" + Server.MapPath("~/App_Data/" + productDataBase))) {
                connection.Open();
                using (var command = new SQLiteCommand(connection)) {
                    using (var transaction = connection.BeginTransaction()) {
                        sql_delete = string.Format("DELETE FROM product WHERE supplier = '{0}';", xx[0].supplier);
                        command.CommandText = sql_delete;
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

                        foreach (Product p in xx) {
                            sql = string.Format(@"INSERT OR IGNORE INTO translation (sku, shortdesc_en, longdesc_en, category_en, supplier)
                                                VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')"
                                                , p.sku, p.shortdesc_en.Replace("'", ""), p.longdesc_en.Replace("'", "")
                                                , p.category_en.Replace("&", "and"), p.supplier);
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                }
                connection.Close();
            }
            time = stopwatch.Elapsed.TotalSeconds;
            return string.Format(@"{0} items updated successfully in {1} seconds.", xx.Count(), time);
        } catch (Exception e) {
            return e.Message;
        }
    }

    /*
    [WebMethod]
    public string ExportExcel(string path) {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        double time = 0;
        try {
            ReadExcel re = new ReadExcel();
            List<ReadExcel.ExcelData> data = re.getExcelFile(path);
            List<Product> xx = new List<Product>();
            int i = 1;
            string val = null;
            Product x = new Product();
            foreach (var d in data) {
                if (d.row > 1) {
                    val = d.val;
                    if (string.IsNullOrEmpty(val)) {
                        val = null;
                    }
                    if (i == 1 && val == null) {
                        break;
                    } else                 {
                        if (i == 1) { x.sku = val; }
                        if (i == 2) { x.colorname = val; }
                        if (i == 3) { x.size = val; }
                        if (i == 4) { x.style = val; }
                        if (i == 5) { x.brand = val; }
                        if (i == 6) { x.modelimageurl = val; }
                        if (i == 7) { x.shortdesc_en = val; }
                        if (i == 8) { x.longdesc_en = val; }
                        if (i == 9) { x.gender_en = val; }
                        if (i == 10) { x.category_en = val; }
                        if (i == 11) { x.colorhex = val; }
                        if (i == 12) { x.colorgroup_id = Convert.ToInt32(val); }
                        if (i == 13) { x.isnew = Convert.ToInt32(val); }
                        if (i == 14) { x.colorimageurl = val; }
                        if (i == 15) { x.packshotimageurl = val; }
                        if (i == 16) { x.weight = val; }
                        if (i == 17) { x.colorswatch = val; }
                        if (i == 18) { x.outlet = Convert.ToInt32(val); }
                        if (i == 19) { x.caseqty = val; }
                        if (i == 20) { x.supplier = val; }

                        if (i == 20) {
                            xx.Add(x);
                            x = new Product();
                            i = 1;
                        } else {
                            i++;
                        }
                    }
                }
            }

            string sql_delete = "";
            string sql = "";
            db.CreateDataBase(productDataBase, db.product);
            db.CreateDataBase(productDataBase, db.translation);
            using (var connection = new SQLiteConnection(@"Data Source=" + Server.MapPath("~/App_Data/" + productDataBase))) {
                connection.Open();
                using (var command = new SQLiteCommand(connection)) {
                    using (var transaction = connection.BeginTransaction()) {
                        sql_delete = string.Format("DELETE FROM product WHERE supplier = '{0}';", xx[0].supplier);
                        command.CommandText = sql_delete;
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

                        foreach (Product p in xx) {
                            sql = string.Format(@"INSERT OR IGNORE INTO translation (sku, shortdesc_en, longdesc_en, category_en, supplier)
                                                VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')"
                                                , p.sku, p.shortdesc_en.Replace("'", ""), p.longdesc_en.Replace("'", "")
                                                , p.category_en.Replace("&", "and"), p.supplier);
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                }
                connection.Close();
            }
            time = stopwatch.Elapsed.TotalSeconds;
            return String.Format(@"{0} items updated successfully in {1} seconds.", xx.Count(), time);
        } catch(Exception e) {
            return JsonConvert.SerializeObject(e.Message, Formatting.None);
        }
    }
    */

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
            products = JsonConvert.DeserializeObject<List<Product>>(GetDataUtt("https://www.utteam.com/api/dataexport/b102f37bc6e73a7d59e12828a92f26f3?action=product&format=json&variables=&fields=sku,colorname,size,style,brand,modelimageurl,shortdesc_en,longdesc_en,gender_en,category_en,colorhex,colorgroup_id,isnew,colorimageurl,packshotimageurl,weight,colorswatch,outlet,caseqty"));
            List<Stock> stock = new List<Stock>();
            stock = JsonConvert.DeserializeObject<List<Stock>>(GetDataUtt("https://www.utteam.com/api/dataexport/b102f37bc6e73a7d59e12828a92f26f3?action=stock&format=json&variables=&fields=style,color,size,sku,uttstock,suppstock,price,specialprice,specialstart,specialend,currency,uom"));
            List<Style> style = new List<Style>();
            style = JsonConvert.DeserializeObject<List<Style>>(GetDataUtt("https://www.utteam.com/api/dataexport/b102f37bc6e73a7d59e12828a92f26f3?action=style&format=json&variables=&fields=style,gsmweight,gender_en,sizes,colors,outlet,coo,imageurl,altimageurl,fabric_en,cut_en,details_en,carelabels_en,carelabellogos,category_en,specimageurl,isnew"));
            List<SizeSpecification> size = new List<SizeSpecification>();
            size = JsonConvert.DeserializeObject<List<SizeSpecification>>(GetDataUtt("https://utteam.com/api/dataexport/b102f37bc6e73a7d59e12828a92f26f3?action=sizespecs&format=json&variables=&fields=style,size,name_en,value"));
            uttTime = stopwatch.Elapsed.TotalSeconds;

            //TOOD: delete data only if supplier is utt

            db.CreateDataBase(productDataBase, db.product);
            db.CreateDataBase(productDataBase, db.style);
            db.CreateDataBase(productDataBase, db.stock);
            db.CreateDataBase(productDataBase, db.size);
            db.CreateDataBase(productDataBase, db.translation);

            using (var connection = new SQLiteConnection(@"Data Source=" + Server.MapPath("~/App_Data/" + productDataBase))) {
                connection.Open();
                using (var command = new SQLiteCommand(connection)) {
                    using (var transaction = connection.BeginTransaction()) {
                        sql_delete = string.Format("DELETE FROM product WHERE supplier = '{0}';", supplier);
                        command.CommandText = sql_delete;
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

                         foreach (Style s in style) {
                            sql = string.Format(@"INSERT OR REPLACE INTO style (style, gsmweight, sizes, colors, outlet, coo, imageurl, altimageurl, fabric_en, cut_en, details_en, carelabels_en, carelabellogos, category_en, category_code, specimageurl, isnew, supplier)
                                                VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}')"
                                                , s.style, s.gsmweight, s.sizes, s.colors, s.outlet, s.coo, s.imageurl, s.altimageurl, s.fabric_en, s.cut_en
                                                , s.details_en, s.carelabels_en, s.carelabellogos, s.category_en.Replace("&", "and")
                                                , s.category_en.Replace("&", "And").Replace(" ", ""), s.specimageurl, s.isnew, supplier);
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }

                        foreach (Stock s in stock) {
                            sql = string.Format(@"INSERT INTO stock (style, color, size, sku, uttstock, suppstock, price, specialprice, specialstart, specialend, currency, uom, supplier)
                                                VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}')"
                                                , s.style, s.color, s.size, s.sku, s.uttstock, s.suppstock, s.price
                                                , s.specialprice, s.specialstart , s.specialend, s.currency, s.uom, supplier);
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }

                        foreach (SizeSpecification s in size) {
                            sql = string.Format(@"INSERT INTO size (style, size, name_en, value, supplier)
                                                VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')"
                                                , s.style, s.size, s.name_en, s.value, supplier);
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


                        foreach (Product p in products) {
                            sql = string.Format(@"INSERT OR IGNORE INTO translation (sku, shortdesc_en, longdesc_en, category_en, supplier)
                                                VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')"
                                                , p.sku, p.shortdesc_en.Replace("'", ""), p.longdesc_en.Replace("'", "")
                                                , p.category_en.Replace("&", "and"), supplier);
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                }
                connection.Close();
            } return (String.Format(@"{0} items downloaded from UTT in {1} seconds. Insert into products.ddb in {2} seconds.", products.Count(), uttTime, (stopwatch.Elapsed.TotalSeconds - uttTime)));
            //return ("Update completed successfully. Get from UTT: " + uttTime + " seconds. Insert Into SQL: " + (stopwatch.Elapsed.TotalSeconds - uttTime) + " seconds.");
        } catch (Exception e) {
            uttTime = stopwatch.Elapsed.TotalSeconds;
            return String.Format("ERROR: {0} ({1} seconds)", e.Message, uttTime);
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
    public string GetDistinctFilters(string category) {
        try {
            Distinct x = GetDistinct(category, null, null);
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
            using (SQLiteConnection connection = new SQLiteConnection(
                string.Format("Data Source={0}", Server.MapPath(string.Format("~/App_Data/{0}", productDataBase))))) {
                connection.Open();
                string sql = string.Format(@"SELECT p.sku, p.colorname, p.size, p.style, p.brand, p.modelimageurl, p.shortdesc_en, p.longdesc_en, p.gender_en, p.category_en, p.colorhex, p.colorgroup_id, p.isnew, st.sizes, st.colors, s.price, p.colorimageurl, p.packshotimageurl, p.category_code, p.brand_code, p.gender_code, st.outlet, st.isnew, t.shortdesc_hr, t.longdesc_hr FROM product p
                                        LEFT OUTER JOIN style st
                                        ON p.style = st.style
                                        LEFT OUTER JOIN stock s
                                        ON p.sku = s.sku
                                        LEFT OUTER JOIN translation t
                                        ON p.sku = t.sku
                                        WHERE p.category_code = '{1}'
                                        GROUP BY p.style
                                        {2}
                                        LIMIT {0}", limit, category, OrderBy(sort, order));
                using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                    SQLiteDataReader reader = command.ExecuteReader();
                    List<ProductData> xx = new List<ProductData>();
                    while (reader.Read()) {
                        ProductData x = new ProductData();
                       // x.sku = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                       // x.colorname = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                       // x.size = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2);
                        x.style = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3);
                        x.brand = reader.GetValue(4) == DBNull.Value ? "" : reader.GetString(4);
                        x.modelimageurl = reader.GetValue(5) == DBNull.Value ? "" : reader.GetString(5);
                        x.shortdesc_en = reader.GetValue(6) == DBNull.Value ? "" : reader.GetString(6);
                        // x.longdesc_en = reader.GetValue(7) == DBNull.Value ? null : reader.GetString(7).Split(';');
                        x.gender_en = reader.GetValue(8) == DBNull.Value ? "" : reader.GetString(8);
                       // x.category_en = reader.GetValue(9) == DBNull.Value ? "" : reader.GetString(9);
                       // x.colorhex = reader.GetValue(10) == DBNull.Value ? "" : reader.GetString(10);
                       // x.colorgroup_id = reader.GetValue(11) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(11));
                        x.isnew = reader.GetValue(22) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(22));
                        x.sizes = reader.GetValue(13) == DBNull.Value ? "" : reader.GetString(13);
                       // x.colors = reader.GetValue(14) == DBNull.Value ? "" : reader.GetString(14);
                        x.uttprice = reader.GetValue(15) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(15));
                       // x.colorimageurl = reader.GetValue(16) == DBNull.Value ? "" : reader.GetString(16);
                        x.packshotimageurl = reader.GetValue(17) == DBNull.Value ? null : reader.GetString(17).Replace(" /", "/").Split('|');
                        x.category_code = reader.GetValue(18) == DBNull.Value ? "" : reader.GetString(18);
                        x.brand_code = reader.GetValue(19) == DBNull.Value ? "" : reader.GetString(19);
                        x.gender_code = reader.GetValue(20) == DBNull.Value ? "" : reader.GetString(20);
                        Price pr = new Price();
                        x.price_min = pr.GetPrice(x.category_code, x.brand_code, x.style, GetUttPrice(connection, x.style, "MIN"));
                        //x.price_max = pr.GetPrice(x.category_code, x.brand_code, x.style, GetUttPrice(connection, x.style, "MAX"));
                        x.stock = new List<Stock>();
                        x.outlet = reader.GetValue(21) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(21));  //  GetOutlet(connection, x.style);
                        x.isnew = reader.GetValue(22) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(22));
                        x.shortdesc_hr = reader.GetValue(23) == DBNull.Value ? "" : reader.GetString(23);
                        x.longdesc_hr = reader.GetValue(24) == DBNull.Value ? null : reader.GetString(24).Split(';');
                        xx.Add(x);
                    }
                    xxx.products = xx;
                    xxx.distinct = GetDistinct(category, null, null);
                    xxx.response.time = DateTime.Now.ToString();
                    xxx.response.responseTime = stopwatch.Elapsed.TotalSeconds;
                    xxx.response.count = GetCount(string.Format(@"
                                SELECT COUNT(DISTINCT p.style) FROM product p WHERE p.category_code = '{0}'", category), connection);
                    xxx.response.maxPrice = xx.Max(a => a.price_min.net);
                }
                connection.Close();
            }
            return JsonConvert.SerializeObject(xxx, Formatting.None);
        } catch (Exception e) { return e.Message; }
    }

    [WebMethod]
    public string GetProductsByGroup(int limit, string group, string type) {
        try {
            List<Categories.SelectedCategories> sc = categories.GetCategories();
            string groupQuery = "";
            switch (type) {
                case "brand":
                    if(group != "all") {
                        groupQuery = string.Format("WHERE p.brand_code = '{0}'", group);
                    }
                break;
                case "gender":
                    groupQuery = string.Format("WHERE p.gender_code = '{0}'", group);
                    break;
                case "isnew":
                    groupQuery = string.Format("WHERE st.isnew = '{0}'", group);
                    break;
                case "outlet":
                    groupQuery = string.Format("WHERE st.outlet = '{0}'", group);
                    break;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + productDataBase));
            connection.Open();
            string sql = string.Format(@"SELECT p.sku, p.colorname, p.size, p.style, p.brand, p.modelimageurl, p.shortdesc_en, p.longdesc_en, p.gender_en, p.category_en, p.colorhex, p.colorgroup_id, p.isnew, st.sizes, st.colors, s.price, p.colorimageurl, p.packshotimageurl, p.category_code, p.brand_code, p.gender_code, st.outlet, st.isnew, t.shortdesc_hr, t.longdesc_hr FROM product p
                                        LEFT OUTER JOIN style st
                                        ON p.style = st.style
                                        LEFT OUTER JOIN stock s
                                        ON p.sku = s.sku
                                        LEFT OUTER JOIN translation t
                                        ON p.sku = t.sku
                                        {1}
                                        GROUP BY p.style
                                        LIMIT {0}", limit, groupQuery);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            ProductsData xxx = new ProductsData();
            List<ProductData> xx = new List<ProductData>();
            while (reader.Read()) {
                ProductData x = new ProductData();
                x.sku = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                x.colorname = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                x.size = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2);
                x.style = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3);
                x.brand = reader.GetValue(4) == DBNull.Value ? "" : reader.GetString(4);
                x.modelimageurl = reader.GetValue(5) == DBNull.Value ? "" : reader.GetString(5);
                x.shortdesc_en = reader.GetValue(6) == DBNull.Value ? "" : reader.GetString(6);
                x.longdesc_en = reader.GetValue(7) == DBNull.Value ? null : reader.GetString(7).Split(';');
                x.gender_en = reader.GetValue(8) == DBNull.Value ? "" : reader.GetString(8);
                x.category_en = reader.GetValue(9) == DBNull.Value ? "" : reader.GetString(9);
                x.colorhex = reader.GetValue(10) == DBNull.Value ? "" : reader.GetString(10);
                x.colorgroup_id = reader.GetValue(11) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(11));
                x.isnew = reader.GetValue(22) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(22));
                x.sizes = reader.GetValue(13) == DBNull.Value ? "" : reader.GetString(13);
                x.colors = reader.GetValue(14) == DBNull.Value ? "" : reader.GetString(14);
                x.uttprice = reader.GetValue(15) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(15));
                x.colorimageurl = reader.GetValue(16) == DBNull.Value ? "" : reader.GetString(16);
                x.packshotimageurl = reader.GetValue(17) == DBNull.Value ? null : reader.GetString(17).Replace(" /", "/").Split('|');
                x.category_code = reader.GetValue(18) == DBNull.Value ? "" : reader.GetString(18);
                x.brand_code = reader.GetValue(19) == DBNull.Value ? "" : reader.GetString(19);
                x.gender_code = reader.GetValue(20) == DBNull.Value ? "" : reader.GetString(20);
                Price pr = new Price();
                x.price_min = pr.GetPrice(x.category_code, x.brand_code, x.style, GetUttPrice(connection, x.style, "MIN"));
                //x.price_max = pr.GetPrice(x.category_code, x.brand_code, x.style, GetUttPrice(connection, x.style, "MAX"));
                List<Stock> st = new List<Stock>();
                x.stock = st;
                x.outlet = reader.GetValue(21) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(21));
                x.isnew = reader.GetValue(22) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(22));
                x.shortdesc_hr = reader.GetValue(23) == DBNull.Value ? "" : reader.GetString(23);
                x.longdesc_hr = reader.GetValue(24) == DBNull.Value ? null : reader.GetString(24).Split(';');
                if (sc.Find(a => a.code == x.category_code).isselected == true) {
                    xx.Add(x);
                }
            }
            xxx.products = xx;
            xxx.distinct = GetDistinct(null, group, type);
            xxx.response.time = DateTime.Now.ToString();
            xxx.response.responseTime = stopwatch.Elapsed.TotalSeconds;
            xxx.response.count = GetCount(string.Format(@"
                                                        SELECT COUNT(DISTINCT p.style) FROM product p
                                                        LEFT OUTER JOIN style st
                                                        ON p.style = st.style
                                                        {0}", groupQuery), connection);
            xxx.response.maxPrice = xx.Max(a => a.price_min.net);
            connection.Close();
            return JsonConvert.SerializeObject(xxx, Formatting.None);
        } catch (Exception e) { return e.Message; }
    }

    [WebMethod]
    public string SearchProducts(int limit, int page, string category, string search, Distinct filter, string group, string type, string sort, string order) {
        try {
            Stopwatch stopwatch = new Stopwatch();
            List<Categories.SelectedCategories> sc = categories.GetCategories();
            stopwatch.Start();
            bool isWhere = false;
            string groupQuery = "";
            switch (type) {
                case "brand":
                    if(group != "all") {
                        groupQuery = string.Format("WHERE p.brand_code = '{0}'", group);
                        isWhere = true;
                    }
                break;
                case "gender":
                    groupQuery = string.Format("WHERE p.gender_code = '{0}'", group);
                    isWhere = true;
                    break;
                case "isnew":
                    groupQuery = string.Format("WHERE st.isnew = '{0}'", group);
                    isWhere = true;
                    break;
                case "outlet":
                    groupQuery = string.Format("WHERE st.outlet = '{0}'", group);
                    isWhere = true;
                    break;
            }

            //TODO
            string sqlCategoryQuery = "";
            /*
            if (!string.IsNullOrEmpty(category)) {
                sqlCategoryQuery = string.Format("WHERE p.category_code = '{0}'", category);
                isWhere = true;
            }
            */
            string sqlSearchQuery = "";
            string sqlFilterQuery = "";
            
            if (!string.IsNullOrWhiteSpace(search)) {
                // TODO: 
                sqlSearchQuery = string.Format(@"
                    {1} p.sku LIKE '%{0}%' OR p.style LIKE '%{0}%' 
                    OR p.brand LIKE '%{0}%' OR p.shortdesc_en LIKE '%{0}%' OR p.longdesc_en LIKE '%{0}%'
                    OR t.shortdesc_hr LIKE '%{0}%' OR t.longdesc_hr LIKE '%{0}%' OR t.category_hr LIKE '%{0}%' ", search, isWhere == true ? "AND" : "WHERE");
                //sqlSearchQuery = string.Format(@"
                //    {1} p.sku LIKE '%{0}%' OR p.style LIKE '%{0}%' 
                //    OR p.brand LIKE '%{0}%' OR p.shortdesc_en 
                //    LIKE '%{0}%' OR p.longdesc_en LIKE '%{0}%' ", search, isWhere == true ? "AND" : "WHERE");
                isWhere = true;
            }
            else {
                if(filter != null) {
                    StringBuilder sqlColorfilter = new StringBuilder();
                    if (filter.colorGroup.Count > 0) {
                        if (filter.colorGroup.Count(a => a.isselected == true) > 0) {
                            bool isOr = false;
                            sqlColorfilter.AppendLine(string.Format("{0} (", isWhere == true ? "AND" : "WHERE"));
                            isWhere = true;
                            foreach (DistColorGroup cg in filter.colorGroup) {
                                if (cg.isselected == true) {
                                    if (isOr == true) {
                                        sqlColorfilter.AppendLine("OR (");
                                    } else {
                                        sqlColorfilter.AppendLine(" (");
                                    }
                                    int count = 0;
                                    foreach (var cc in cg.colorchild) {
                                        sqlColorfilter.AppendLine(string.Format(@"{0} p.colorname = '{1}'", count > 0 && count < cg.colorchild.Count ? "OR" : "", cc.colorgroupname));
                                        count += 1;
                                    }
                                    sqlColorfilter.AppendLine(" )");
                                    isOr = true;
                                }
                            }
                            sqlColorfilter.AppendLine(" )");
                        }
                    }

                    StringBuilder sqlSizeFilter = new StringBuilder();
                    if (filter.size.Count > 0) {
                        if (filter.size.Count(a => a.isselected == true) > 0) {
                            isWhere = true;
                            sqlSizeFilter.AppendLine("AND (");
                            int count = 0;
                            foreach (Size s in filter.size) {
                                if (s.isselected == true) {
                                    sqlSizeFilter.AppendLine(string.Format(@"{0} p.size = '{1}'", count > 0 && count < filter.size.Count ? "OR" : "", s.title));
                                    count += 1;
                                }
                            }
                            sqlSizeFilter.AppendLine(" )");
                        }
                    }

                    StringBuilder sqlBrandFilter = new StringBuilder();
                    if (filter.brand.Count > 0) {
                        if (filter.brand.Count(a => a.isselected == true) > 0) {
                            isWhere = true;
                            sqlBrandFilter.AppendLine("AND (");
                            int count = 0;
                            foreach (Brand b in filter.brand) {
                                if (b.isselected == true) {
                                    sqlBrandFilter.AppendLine(string.Format(@"{0} p.brand_code = '{1}'", count > 0 && count < filter.brand.Count ? "OR" : "", b.code));
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
                                    sqlGenderFilter.AppendLine(string.Format(@"{0} p.gender_code = '{1}'", count > 0 && count < filter.gender.Count ? "OR" : "", g.code));
                                    count += 1;
                                }
                            }
                            sqlGenderFilter.AppendLine(" )");
                        }
                    }
                    sqlFilterQuery = sqlColorfilter + " " + sqlSizeFilter + " " + sqlBrandFilter + "" + sqlGenderFilter;
                }
            }

            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + productDataBase));
            connection.Open();

            string sql = string.Format(@"SELECT p.sku, p.colorname, p.size, p.style, p.brand, p.modelimageurl, p.shortdesc_en, p.longdesc_en, p.gender_en, p.category_en, p.colorhex, p.colorgroup_id, p.isnew, st.sizes, st.colors, s.price, p.colorimageurl, p.packshotimageurl, p.category_code, p.brand_code, p.gender_code, st.outlet, st.isnew, t.shortdesc_hr, t.longdesc_hr FROM product p
                                        LEFT OUTER JOIN style st
                                        ON p.style = st.style
                                        LEFT OUTER JOIN stock s
                                        ON p.sku = s.sku
                                        LEFT OUTER JOIN translation t
                                        ON p.sku = t.sku
                                        {5}
                                        {2} {3} {4}
                                        GROUP BY p.style
                                        {6}
                                        LIMIT {0} OFFSET {1}", limit, (page - 1) * limit, sqlCategoryQuery, sqlSearchQuery, sqlFilterQuery, groupQuery, OrderBy(sort, order));

            //string sql = string.Format(@"SELECT p.sku, p.colorname, p.size, p.style, p.brand, p.modelimageurl, p.shortdesc_en, p.longdesc_en, p.gender_en, p.category_en, p.colorhex, p.colorgroup_id, p.isnew, st.sizes, st.colors, s.price, p.colorimageurl, p.packshotimageurl, p.category_code, p.brand_code, p.gender_code, st.outlet, st.isnew FROM product p
            //                            LEFT OUTER JOIN style st
            //                            ON p.style = st.style
            //                            LEFT OUTER JOIN stock s
            //                            ON p.sku = s.sku
            //                            {5}
            //                            {2} {3} {4}
            //                            GROUP BY p.style
            //                            {6}
            //                            LIMIT {0} OFFSET {1}", limit, (page - 1) * limit, sqlCategoryQuery, sqlSearchQuery, sqlFilterQuery, groupQuery, OrderBy(sort, order));
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            ProductsData xxx = new ProductsData();
            List<ProductData> xx = new List<ProductData>();
            while (reader.Read()) {
                ProductData x = new ProductData();
                x.sku = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                x.colorname = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                x.size = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2);
                x.style = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3);
                x.brand = reader.GetValue(4) == DBNull.Value ? "" : reader.GetString(4);
                x.modelimageurl = reader.GetValue(5) == DBNull.Value ? "" : reader.GetString(5);
                x.shortdesc_en = reader.GetValue(6) == DBNull.Value ? "" : reader.GetString(6);
                x.longdesc_en = reader.GetValue(7) == DBNull.Value ? null : reader.GetString(7).Split(';');
                x.gender_en = reader.GetValue(8) == DBNull.Value ? "" : reader.GetString(8);
                x.category_en = reader.GetValue(9) == DBNull.Value ? "" : reader.GetString(9);
                x.colorhex = reader.GetValue(10) == DBNull.Value ? "" : reader.GetString(10);
                x.colorgroup_id = reader.GetValue(11) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(11));
                x.isnew = reader.GetValue(22) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(22));
                x.sizes = reader.GetValue(13) == DBNull.Value ? "" : reader.GetString(13);
                x.colors = reader.GetValue(14) == DBNull.Value ? "" : reader.GetString(14);
                x.uttprice = reader.GetValue(15) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(15));
                x.colorimageurl = reader.GetValue(16) == DBNull.Value ? "" : reader.GetString(16);
                x.packshotimageurl = reader.GetValue(17) == DBNull.Value ? null : reader.GetString(17).Replace(" /", "/").Split('|');
                x.category_code = reader.GetValue(18) == DBNull.Value ? "" : reader.GetString(18);
                x.brand_code = reader.GetValue(19) == DBNull.Value ? "" : reader.GetString(19);
                x.gender_code = reader.GetValue(20) == DBNull.Value ? "" : reader.GetString(20);
                Price pr = new Price();
                x.price_min = pr.GetPrice(x.category_code, x.brand_code, x.style, GetUttPrice(connection, x.style, "MIN"));
                //x.price_max = pr.GetPrice(x.category_code, x.brand_code, x.style, GetUttPrice(connection, x.style, "MAX"));
                List<Stock> st = new List<Stock>();
                x.stock = st;
                x.outlet = reader.GetValue(21) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(21));
                x.isnew = reader.GetValue(22) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(22));
                x.shortdesc_hr = reader.GetValue(23) == DBNull.Value ? "" : reader.GetString(23);
                x.longdesc_hr = reader.GetValue(24) == DBNull.Value ? null : reader.GetString(24).Split(';');
                if (sc.Find(a => a.code == x.category_code).isselected == true) {
                    xx.Add(x);
                }
            }

            xxx.products = xx;
            xxx.distinct = !string.IsNullOrWhiteSpace(sqlSearchQuery) ? GetDistinct(category, null, null) : null;
            xxx.response.time = DateTime.Now.ToString();
            xxx.response.responseTime = stopwatch.Elapsed.TotalSeconds;
            xxx.response.count = GetCount(string.Format(@"
                                        SELECT COUNT(DISTINCT p.style) FROM product p
                                        LEFT OUTER JOIN style st
                                        ON p.style = st.style
                                        LEFT OUTER JOIN translation t
                                        ON p.sku = t.sku
                                        {0} {1} {2} {3}"
                            , groupQuery, sqlCategoryQuery, sqlSearchQuery, sqlFilterQuery), connection);
            xxx.response.maxPrice = xx.Count > 0 ? xx.Max(a => a.price_min.net) : 0;
            connection.Close();
            return JsonConvert.SerializeObject(xxx, Formatting.None);
        } catch (Exception e) { return e.Message; }
    }

    [WebMethod]
    public string GetProduct(string style, string color) {
        try {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + productDataBase));
            connection.Open();
            string sqlQuery = string.IsNullOrWhiteSpace(color) ? "GROUP BY p.style" : string.Format("AND p.colorname = '{0}'", color.Replace("%20", " "));
            string sql = string.Format(@"SELECT p.sku, p.colorname, p.size, p.style, p.brand, p.modelimageurl, p.shortdesc_en, p.longdesc_en, p.gender_en, p.category_en, p.colorhex, p.colorgroup_id, p.isnew, st.sizes, st.colors, s.price, p.colorimageurl, p.packshotimageurl, st.carelabels_en, st.carelabellogos, p.category_code, p.brand_code, st.specimageurl, p.gender_code, st.outlet, st.isnew FROM product p
                                        LEFT OUTER JOIN style st
                                        ON p.style = st.style                                    
                                        LEFT OUTER JOIN stock s
                                        ON p.sku = s.sku
                                        WHERE p.style = '{0}' {1}", style, sqlQuery);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            ProductData x = new ProductData();
            while (reader.Read()) {
                x.sku = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                x.colorname = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                x.size = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2);
                x.style = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3);
                x.brand = reader.GetValue(4) == DBNull.Value ? "" : reader.GetString(4);
                x.modelimageurl = reader.GetValue(5) == DBNull.Value ? "" : reader.GetString(5);
                x.shortdesc_en = reader.GetValue(6) == DBNull.Value ? "" : reader.GetString(6);
                x.longdesc_en = reader.GetValue(7) == DBNull.Value ? null : reader.GetString(7).Split(';');
                x.gender_en = reader.GetValue(8) == DBNull.Value ? "" : reader.GetString(8);
                x.category_en = reader.GetValue(9) == DBNull.Value ? "" : reader.GetString(9);
                x.colorhex = reader.GetValue(10) == DBNull.Value ? "" : reader.GetString(10);
                x.colorgroup_id = reader.GetValue(11) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(11));
                x.isnew = reader.GetValue(25) == DBNull.Value ? 0 : Convert.ToInt32(reader.GetString(25));
                x.sizes = reader.GetValue(13) == DBNull.Value ? "" : reader.GetString(13);
                x.colors = reader.GetValue(14) == DBNull.Value ? "" : reader.GetString(14);
                x.uttprice = reader.GetValue(15) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(15));
                x.colorimageurl = reader.GetValue(16) == DBNull.Value ? "" : reader.GetString(16);
                x.packshotimageurl = reader.GetValue(17) == DBNull.Value ? null : reader.GetString(17).Replace(" /", "/").Split('|');
                x.carelabel = GetCareLabel(reader.GetValue(18) == DBNull.Value ? "" : reader.GetString(18), reader.GetValue(19) == DBNull.Value ? "" : reader.GetString(19));
                x.category_code = reader.GetValue(20) == DBNull.Value ? "" : reader.GetString(20);
                x.brand_code = reader.GetValue(21) == DBNull.Value ? "" : reader.GetString(21);
                Price pr = new Price();
                x.price_min = pr.GetPrice(
                    reader.GetValue(20) == DBNull.Value ? "" : reader.GetString(20),
                    reader.GetValue(21) == DBNull.Value ? "" : reader.GetString(21),
                    x.style,
                    GetUttPrice(connection, x.style, "MIN"));
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
            }
            connection.Close();
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
    public string TranslateProducts() {
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
            return JsonConvert.SerializeObject("Update completed successfully", Formatting.None); ;
        } catch (Exception e) { return e.Message; }
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

    private string GetDataUtt(string url) {
        // Create a request for the URL.  
        WebRequest request = WebRequest.Create(url);
        //WebRequest request = WebRequest.Create(
        //  "https://www.utteam.com/api/dataexport/b102f37bc6e73a7d59e12828a92f26f3?action=product&format=json&variables=brand:GILDAN;isnew:1&fields=sku,colorname,size,style,barcode,brand,modelimageurl,colorimageurl,packshotimageurl,shortdesc_hu,longdesc_hu,shortdesc_en,longdesc_en,gsmweight,gender_hu,category_hu,gender_en,category_en,colorhex,colorswatch,colorpantone,colorcmyk,colorrgb,colorlegend_name_hu,colorlegend_name_en,coo,colorgroup_id,isnew");
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
            stock = JsonConvert.DeserializeObject<List<Stock>>(GetDataUtt("https://www.utteam.com/api/dataexport/b102f37bc6e73a7d59e12828a92f26f3?action=stock&format=json&variables=&fields=style,color,size,sku,uttstock,suppstock,price,specialprice,specialstart,specialend,currency,uom"));
            db.CreateDataBase(productDataBase, db.stock);
            using (var connection = new SQLiteConnection(
               @"Data Source=" + Server.MapPath("~/App_Data/" + productDataBase))) {
                connection.Open();
                using (var command = new SQLiteCommand(connection)) {
                    using (var transaction = connection.BeginTransaction()) {
                        foreach (Stock s in stock) {
                           sql = string.Format(@"INSERT INTO stock (style, color, size, sku, uttstock, suppstock, price, specialprice, specialstart, specialend, currency, uom, supplier)
                                                VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}')"
                                                 , s.style, s.color, s.size, s.sku, s.uttstock, s.suppstock, s.price, s.specialprice
                                                 , s.specialstart, s.specialend, s.currency, s.uom, supplier);
                            command.CommandText = sql;

                            //command.CommandText = @"INSERT INTO stock VALUES 
                            //            (@style, @color, @size, @sku, @uttstock, @suppstock, @price, @specialprice, @specialstart, @specialend, @currency, @uom)";
                            //command.Parameters.Add(new SQLiteParameter("style", s.style));
                            //command.Parameters.Add(new SQLiteParameter("color", s.color));
                            //command.Parameters.Add(new SQLiteParameter("size", s.size));
                            //command.Parameters.Add(new SQLiteParameter("sku", s.sku));
                            //command.Parameters.Add(new SQLiteParameter("uttstock", s.uttstock));
                            //command.Parameters.Add(new SQLiteParameter("suppstock", s.suppstock));
                            //command.Parameters.Add(new SQLiteParameter("price", s.price));
                            //command.Parameters.Add(new SQLiteParameter("specialprice", s.specialprice));
                            //command.Parameters.Add(new SQLiteParameter("specialstart", s.specialstart));
                            //command.Parameters.Add(new SQLiteParameter("specialend", s.specialend));
                            //command.Parameters.Add(new SQLiteParameter("currency", s.currency));
                            //command.Parameters.Add(new SQLiteParameter("uom", s.uom));
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
            stock = JsonConvert.DeserializeObject<List<Stock>>(GetDataUtt(query));
            return stock;
        } catch (Exception e) {
            return null;
        }
    }

    private List<Stock> GetStock(SQLiteConnection connection, string style, int limit, int offset) {
        try {  
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
                Price pr = new Price();
                x.myprice = pr.GetPrice( x.category_code, x.brand_code, x.style, x.price);
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
            string sql = string.Format(@"SELECT s.style, s.color, s.size, s.sku, s.uttstock, s.suppstock, s.price, s.specialprice, s.specialstart, s.specialend, s.currency, s.uom, p.colorhex, p.modelimageurl, p.shortdesc_en, p.colorimageurl, p.packshotimageurl, p.category_code, p.brand_code, p.weight, p.colorswatch, p.outlet, p.caseqty FROM stock s
                                        LEFT OUTER JOIN product p
                                        ON s.sku = p.sku                      
                                        WHERE s.style = '{0}'", style);
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
                Price pr = new Price();
                x.myprice = pr.GetPrice(x.category_code, x.brand_code, x.style, x.price);
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

            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + productDataBase));
            connection.Open();
            string sql = string.Format(@"SELECT category_en, category_code, count(style) FROM style GROUP BY category_en");
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<Category> xx = new List<Category>();
            while (reader.Read()) {
                Category x = new Category();
                x.title = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                x.code = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                x.count = reader.GetValue(2) == DBNull.Value ? 0 : reader.GetInt32(2);
                x.isselected = sc.Find(a => a.code == x.code).isselected;
                x.order = sc.Find(a => a.code == x.code).order;
                if (x.isselected) {
                    xx.Add(x);
                }
            }
            connection.Close();
            return xx;
        }
        catch (Exception e) {
            return new List<Category>();
        }
    }

    private Distinct GetDistinct(string productCategory, string group, string type) {
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

        SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + productDataBase));
        connection.Open();
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
        xx.brand = brands;
        xx.colorGroup = distinctColorGroups;
        xx.size = sizes;
        xx.gender = genders;

        connection.Close();
        return xx;
    }

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
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read()) {
            count = reader.GetValue(0) == DBNull.Value ? 0 : reader.GetInt32(0);
        }
        return count;
    }

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

    private string OrderBy(string sort, string order) {
        string sql = "";
        if (!string.IsNullOrEmpty(sort)) {
            if (sort == "price") {
                sql = string.Format(@"ORDER BY CAST(s.{0} AS INTEGER) {1}", sort, order);
            } else {
                sql = string.Format(@"ORDER BY p.{0} {1}", sort, order);
            }
        }
        return sql;
    }

    private List<SizeSpecification> GetSizeSpecification(SQLiteConnection connection, string style) {
        try {
            string sql = string.Format("SELECT size, name_en, value FROM size WHERE style = '{0}'", style);
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<SizeSpecification> xx = new List<SizeSpecification>();
            while (reader.Read()) {
                SizeSpecification x = new SizeSpecification();
                x.size = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                x.name_en = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                x.value = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2);
                xx.Add(x);
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
    #endregion Methods



}
