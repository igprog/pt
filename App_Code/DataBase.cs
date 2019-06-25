using System;
using System.Web;
using System.IO;
using System.Data.SQLite;

/// <summary>
/// DataBase
/// </summary>
namespace Igprog {
    public class DataBase {

        public DataBase() {
        }

        //tables
        public string users = "users";
        public string product = "product";
        public string style = "style";
        public string stock = "stock";
        public string translation = "translation";
        public string orders = "orders";
        public string featured = "featured";
        public string size = "size";
        public string invoice = "invoice";

        #region CreateTable (products.ddb)

        //TODO
        //colorimageurl
        //packshotimageurl

        //TODO: supplier: "utt" | shortdesc_hr | longdesc_hr | category_hr | supplier
        public void Product(string path){
            string sql = @"
                CREATE TABLE IF NOT EXISTS product
                (sku NVARCHAR(50) PRIMARY KEY,
                colorname NVARCHAR(50),
                size NVARCHAR(50),
                style NVARCHAR(50),
                brand NVARCHAR(50),
                modelimageurl NVARCHAR(50),
                shortdesc_en NVARCHAR(50),
                longdesc_en NVARCHAR(200),
                gender_en NVARCHAR(50),
                category_en NVARCHAR(50),
                colorhex NVARCHAR(50),
                colorgroup_id NVARCHAR(50),
                isnew NVARCHAR(50),
                colorimageurl NVARCHAR(50),
                packshotimageurl NVARCHAR(50),
                category_code NVARCHAR(50),
                brand_code NVARCHAR(50),
                gender_code NVARCHAR(50),
                weight NVARCHAR(50),
                colorswatch NVARCHAR(50),
                outlet NVARCHAR(50),
                caseqty NVARCHAR(50),
                supplier NVARCHAR(50))";
            //string sql = @"
            //    DROP TABLE IF EXISTS product;
            //    CREATE TABLE IF NOT EXISTS product
            //    (sku NVARCHAR(50) PRIMARY KEY,
            //    colorname NVARCHAR(50),
            //    size NVARCHAR(50),
            //    style NVARCHAR(50),
            //    brand NVARCHAR(50),
            //    modelimageurl NVARCHAR(50),
            //    shortdesc_en NVARCHAR(50),
            //    longdesc_en NVARCHAR(200),
            //    gender_en NVARCHAR(50),
            //    category_en NVARCHAR(50),
            //    colorhex NVARCHAR(50),
            //    colorgroup_id NVARCHAR(50),
            //    isnew NVARCHAR(50),
            //    colorimageurl NVARCHAR(50),
            //    packshotimageurl NVARCHAR(50),
            //    category_code NVARCHAR(50),
            //    brand_code NVARCHAR(50),
            //    gender_code NVARCHAR(50),
            //    weight NVARCHAR(50),
            //    colorswatch NVARCHAR(50),
            //    outlet NVARCHAR(50),
            //    caseqty NVARCHAR(50),
            //    supplier NVARCHAR(50))";
            CreateTable(path, sql);
        }

        public void Style(string path){
            string sql = @"
                CREATE TABLE IF NOT EXISTS style
                (style NVARCHAR(50) PRIMARY KEY,
                gsmweight NVARCHAR(50),
                sizes NVARCHAR(50),
                colors NVARCHAR(50),
                outlet NVARCHAR(50),
                coo NVARCHAR(50),
                imageurl NVARCHAR(50),
                altimageurl NVARCHAR(50),
                fabric_en NVARCHAR(50),
                cut_en NVARCHAR(50),
                details_en NVARCHAR(50),
                carelabels_en NVARCHAR(50),
                carelabellogos NVARCHAR(50),
                category_en NVARCHAR(50),
                category_code NVARCHAR(50),
                specimageurl NVARCHAR(50),
                isnew NVARCHAR(50),
                supplier NVARCHAR(50),
                brand NVARCHAR(50),
                shortdesc_en NVARCHAR(50),
                gender_en NVARCHAR(50),
                brand_code NVARCHAR(50),
                gender_code NVARCHAR(50),
                shortdesc_hr NVARCHAR(50),
                price_min NVARCHAR(50))";




            //string sql = @"
            //    CREATE TABLE IF NOT EXISTS style
            //    (style NVARCHAR(50) PRIMARY KEY,
            //    gsmweight NVARCHAR(50),
            //    sizes NVARCHAR(50),
            //    colors NVARCHAR(50),
            //    outlet NVARCHAR(50),
            //    coo NVARCHAR(50),
            //    imageurl NVARCHAR(50),
            //    altimageurl NVARCHAR(50),
            //    fabric_en NVARCHAR(50),
            //    cut_en NVARCHAR(50),
            //    details_en NVARCHAR(50),
            //    carelabels_en NVARCHAR(50),
            //    carelabellogos NVARCHAR(50),
            //    category_en NVARCHAR(50),
            //    category_code NVARCHAR(50),
            //    specimageurl NVARCHAR(50),
            //    isnew NVARCHAR(50),
            //    supplier NVARCHAR(50))";
            CreateTable(path, sql);
        }

        public void Stock(string path){
            string sql = @"
                CREATE TABLE IF NOT EXISTS stock
                (style NVARCHAR(50),
                color NVARCHAR(50),
                size NVARCHAR(50),
                sku NVARCHAR(50),
                uttstock NVARCHAR(50),
                suppstock NVARCHAR(50),
                price NVARCHAR(50),
                specialprice NVARCHAR(50),
                specialstart NVARCHAR(50),
                specialend NVARCHAR(50),
                currency NVARCHAR(50),
                uom NVARCHAR(50),
                supplier NVARCHAR(50))";
            //string sql = @"
            //    DROP TABLE IF EXISTS stock;
            //    CREATE TABLE IF NOT EXISTS stock
            //    (style NVARCHAR(50),
            //    color NVARCHAR(50),
            //    size NVARCHAR(50),
            //    sku NVARCHAR(50),
            //    uttstock NVARCHAR(50),
            //    suppstock NVARCHAR(50),
            //    price NVARCHAR(50),
            //    specialprice NVARCHAR(50),
            //    specialstart NVARCHAR(50),
            //    specialend NVARCHAR(50),
            //    currency NVARCHAR(50),
            //    uom NVARCHAR(50),
            //    supplier NVARCHAR(50))";
            CreateTable(path, sql);
        }

        public void Size(string path){
            string sql = @"
                DROP TABLE IF EXISTS size;
                CREATE TABLE IF NOT EXISTS size
                (style NVARCHAR(50),
                size NVARCHAR(50),
                name_en NVARCHAR(50),
                value NVARCHAR(50),
                supplier NVARCHAR(50))";
            CreateTable(path, sql);
        }

        //TODO: new tbl translation sku | shortdesc_hr | longdesc_hr | category_hr
        public void Translation(string path) {
            string sql = @"
                CREATE TABLE IF NOT EXISTS translation
                (style NVARCHAR(50) PRIMARY KEY,
                shortdesc_en NVARCHAR(50),
                shortdesc_hr NVARCHAR(50),
                longdesc_en NVARCHAR(200),
                longdesc_hr NVARCHAR(200),
                category_en NVARCHAR(50),
                category_hr NVARCHAR(50),
                supplier NVARCHAR(50))";
            //string sql = @"
            //    CREATE TABLE IF NOT EXISTS translation
            //    (sku NVARCHAR(50) PRIMARY KEY,
            //    shortdesc_en NVARCHAR(50),
            //    shortdesc_hr NVARCHAR(50),
            //    longdesc_en NVARCHAR(200),
            //    longdesc_hr NVARCHAR(200),
            //    category_en NVARCHAR(50),
            //    category_hr NVARCHAR(50),
            //    supplier NVARCHAR(50))";
            CreateTable(path, sql);
        }

        public void Featured(string path){
            string sql = @"
                CREATE TABLE IF NOT EXISTS featured
                (style NVARCHAR(50) PRIMARY KEY,
                roworder INTEGER,
                type INTEGER)";
            CreateTable(path, sql);
        }
        #endregion

        #region CreateTable (app.ddb)
        public void Users(string path) {
            string sql = @"CREATE TABLE IF NOT EXISTS users
                        (userId NVARCHER (50),
                        firstName NVARCHAR (50),
                        lastName NVARCHAR (50),
                        companyName NVARCHAR (50),
                        address NVARCHAR (50),
                        postalCode NVARCHAR (50),
                        city NVARCHAR (50),
                        country NVARCHAR (50),
                        pin NVARCHAR (50),
                        phone NVARCHAR (50),
                        email NVARCHAR (50),
                        userName NVARCHAR (50),
                        password NVARCHAR (100),
                        ipAddress NVARCHAR (50),
                        deliveryFirstName NVARCHAR(50),
                        deliveryLastName NVARCHAR(50),
                        deliveryCompanyName NVARCHAR(50),
                        deliveryAddress NVARCHAR(50),
                        deliveryPostalCode NVARCHAR(50),
                        deliveryCity NVARCHAR(50),
                        deliveryCountry NVARCHAR(50),
                        deliveryType NVARCHAR (50),
                        paymentMethod NVARCHAR (50),
                        countryCode NVARCHAR (50),
                        discountCoeff NVARCHAR (50))";
            CreateTable(path, sql);
        }
        public void Orders(string path) {
            string sql = @"CREATE TABLE IF NOT EXISTS orders
                        (orderId NVARCHAR(50) PRIMARY KEY,
                        userId NVARCHAR(50),
                        items NVARCHAR(200),
                        netPrice NVARCHAR(50),
                        grossPrice NVARCHAR(50),
                        currency NVARCHAR(50),
                        orderDate NVARCHAR(50),
                        deliveryFirstName NVARCHAR(50),
                        deliveryLastName NVARCHAR(50),
                        deliveryCompanyName NVARCHAR(50),
                        deliveryAddress NVARCHAR(50),
                        deliveryPostalCode NVARCHAR(50),
                        deliveryCity NVARCHAR(50),
                        deliveryCountry NVARCHAR(50),
                        deliveryType NVARCHAR(50),
                        paymentMethod NVARCHAR(50),
                        note NVARCHAR(200),
                        number NVARCHAR(50),
                        status NVARCHAR(50),
                        countryCode NVARCHAR(50),
                        sendToPrint NVARCHAR(50),
                        deliveryPrice NVARCHAR(50),
                        discount NVARCHAR(50),
                        total NVARCHAR(50))";
            CreateTable(path, sql);
        }

        //TODO:
        public void Invoice(string path) {
            string sql = @"CREATE TABLE IF NOT EXISTS invoice
                        (invoiceId NVARCHAR(50) PRIMARY KEY,
                        number NVARCHAR(50),
                        year NVARCHAR(50),
                        orderId NVARCHAR(50),
                        userId NVARCHAR(50))";
            CreateTable(path, sql);
        }
        #endregion

        public void CreateDataBase(string dataBase, string table) {
            try {
                string path = GetDataBasePath(dataBase);
                string dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                }
                if (!File.Exists(path)) {
                    SQLiteConnection.CreateFile(path);
                }
                CreateTables(table, path);
            } catch (Exception e) { }
        }

        private void CreateTables(string table, string path) {
            switch (table) {
                case "users":
                    Users(path);
                    break;
                case "product":
                    Product(path);
                    break;
                case "style":
                    Style(path);
                    break;
                case "stock":
                    Stock(path);
                    break;
                case "translation":
                    Translation(path);
                    break;
                case "size":
                    Size(path);
                    break;
                case "orders":
                    Orders(path);
                    break;
                case "featured":
                    Featured(path);
                    break;
                case "invoice":
                    Invoice(path);
                    break;
                default:
                    break;
            }
        }

        private void CreateTable(string path, string sql) {
            try {
                if (File.Exists(path)){
                    using(SQLiteConnection connection = new SQLiteConnection("Data Source=" + path)) {
                        connection.Open();
                        using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                            command.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                }
            } catch (Exception e) { }
        }

        public string GetDataBasePath(string dataBase) {
            return HttpContext.Current.Server.MapPath(string.Format(@"~/App_Data/{0}", dataBase));
        }

        //public void AddColumn(string path, string table, string column) {
        //    if (!CheckColumn(path, table, column)) {
        //        string sql = string.Format("ALTER TABLE {0} ADD COLUMN {1} VARCHAR (50)", table, column);
        //        CreateTable(path, sql);
        //    }
        //}

        ///************** Check if column exists ***********/
        //private bool CheckColumn(string path, string table, string column) {
        //    try {
        //        DataBase db = new DataBase();
        //        bool exists = false;
        //        string name = null;
        //        string sql = string.Format("pragma table_info('{0}')", table);
        //        using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + path)) {
        //            connection.Open();
        //            using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
        //                using (SQLiteDataReader reader = command.ExecuteReader()) {
        //                    while (reader.Read()) {
        //                        name = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
        //                        if (name == column) {
        //                            exists = true;
        //                        }
        //                    }
        //                }   
        //            } 
        //            connection.Close();
        //        } 
        //        return exists;
        //    } catch (Exception e) { return false; }
        //}
        ///*************************************************/



    }

}
