using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Configuration;
using Newtonsoft.Json;
using System.Data.SQLite;
using Igprog;

[WebService(Namespace = "https://promo-tekstil.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class Featured : System.Web.Services.WebService {
    string dataBase = ConfigurationManager.AppSettings["ProductDataBase"];
    DataBase db = new DataBase();
    public Featured() {
    }

    #region Class
    public class NewFeatured {
        public string style { get; set; }
        public int roworder { get; set; }

        public CodeTitle type = new CodeTitle();
        public string imageurl { get; set; }
        public string shortdesc_en { get; set; }
        public string brand { get; set; }
    }


    public class FeaturedData {
        public NewFeatured product = new NewFeatured();

        public FeaturedType types = new FeaturedType();
    }

    public class CodeTitle {
        public int code { get; set; }
        public string title { get; set; }
    }

    public class FeaturedType {
        public CodeTitle featured { get; set; }
        public CodeTitle popular { get; set; }
        //public CodeTitle newProduct { get; set; }
    }

    public class Product {
        public string style { get; set; }
        public string modelimageurl { get; set; }
    }

    #endregion Class

    #region WebMathods

    [WebMethod]
    public string Init(){
        FeaturedData xx = new FeaturedData();
        NewFeatured x = new NewFeatured();
        x.style = "";
        x.imageurl = "";
        x.roworder = 0;
        x.type = new CodeTitle();
        x.shortdesc_en = "";
        x.brand = "";
        xx.product = x;
        xx.types = initTypes();
        return JsonConvert.SerializeObject(xx, Formatting.Indented);
    }

    [WebMethod]
    public string Load() {
        try {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + dataBase));
            connection.Open();
            string sql = @"SELECT f.style, f.roworder, f.type, s.imageurl, p.shortdesc_en, p.brand FROM featured f
                        LEFT OUTER JOIN style s
                        ON f.style = s.style
                        LEFT OUTER JOIN product p
                        ON f.style = p.style
                        GROUP BY p.style
                        ORDER BY f.roworder DESC";
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<NewFeatured> xx = new List<NewFeatured>();
            while (reader.Read()) {
                NewFeatured x = new NewFeatured();
                x.style = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                x.roworder = reader.GetValue(1) == DBNull.Value ? 0 : reader.GetInt32(1);
                x.type.code = reader.GetValue(2) == DBNull.Value ? 0 : reader.GetInt32(2);
                x.type.title = getFeaturedTitle(x.type.code);
                x.imageurl = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3);
                x.shortdesc_en = reader.GetValue(4) == DBNull.Value ? "" : reader.GetString(4);
                x.brand = reader.GetValue(5) == DBNull.Value ? "" : reader.GetString(5);
                xx.Add(x);
            }
            connection.Close();
            return JsonConvert.SerializeObject(xx, Formatting.Indented);
        } catch (Exception e) { return e.Message; }
    }

    [WebMethod]
    public string Save(NewFeatured x) {
        db.CreateDataBase(dataBase, db.featured);
        try {
            if (!string.IsNullOrWhiteSpace(x.style)) {
                using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + dataBase))) {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(connection)) {
                        using (var transaction = connection.BeginTransaction()) {
                            command.CommandText = @"INSERT OR REPLACE INTO featured VALUES (@style, @roworder, @type)";
                            command.Parameters.Add(new SQLiteParameter("style", x.style));
                            command.Parameters.Add(new SQLiteParameter("roworder", x.roworder));
                            command.Parameters.Add(new SQLiteParameter("type", x.type.code));
                            command.ExecuteNonQuery();
                            transaction.Commit();
                        }
                    }
                    connection.Close();
                }
                return ("ok");
            } else { return ("enter style"); }
        } catch (Exception e) { return (e.Message); }
    }

    [WebMethod]
    public string Delete(NewFeatured x) {
        try {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + dataBase));
            connection.Open();
            string sql = @"DELETE FROM featured WHERE style = @style";
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.Add(new SQLiteParameter("style", x.style));
            command.ExecuteNonQuery();
            connection.Close();
            return "OK";
        } catch (Exception e) { return (e.Message); }
    }


    [WebMethod]
    public string LoadProducts() {
        try {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + dataBase));
            connection.Open();
            string sql = "SELECT style, modelimageurl FROM product";
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<Product> xx = new List<Product>();
            while (reader.Read()) {
                Product x = new Product();
                x.style = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                x.modelimageurl = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                xx.Add(x);
            }
            connection.Close();
            return JsonConvert.SerializeObject(xx, Formatting.Indented);
        } catch (Exception e) { return e.Message; }
    }

    [WebMethod]
    public string LoadNewProducts() {
        try {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + dataBase));
            connection.Open();
            string sql = "SELECT DISTINCT style, modelimageurl, shortdesc_en, brand FROM product WHERE isnew = 1";
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<Products.Product> xx = new List<Products.Product>();
            while (reader.Read()) {
                Products.Product x = new Products.Product();
                x.style = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                x.modelimageurl = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                x.shortdesc_en = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2);
                x.brand = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3);
                xx.Add(x);
            }
            connection.Close();
            return JsonConvert.SerializeObject(xx, Formatting.Indented);
        } catch (Exception e) { return e.Message; }
    }

    private string getFeaturedTitle(int code) {
        string title = "";
        FeaturedType x = initTypes();
        switch (code) {
            case 0:
                title = x.featured.title;
                break;
            case 1:
                title = x.popular.title;
                break;
            //case 2:
            //    title = x.newProduct.title;
            //    break;
            default:
                break;
        }
        return title;
    }

    FeaturedType initTypes(){
        FeaturedType x = new FeaturedType();
        x.featured = new CodeTitle { code = 0, title = "featured" };
        x.popular = new CodeTitle { code = 1, title = "popular" };
        //x.newProduct = new CodeTitle { code = 2, title = "new" };
        return x;
    }


    #endregion WebMethods


}
