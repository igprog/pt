using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Data.SQLite;
using System.IO;
using Igprog;

/// <summary>
///Order
/// </summary>
[WebService(Namespace = "https://promo-tekstil.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class Orders : System.Web.Services.WebService {
    string dataBase = ConfigurationManager.AppSettings["AppDataBase"];
    string productDataBase = ConfigurationManager.AppSettings["ProductDataBase"];
    DataBase db = new DataBase();
    string orderOptionsFile = "orderoptions";
    Translate T = new Translate();
    Invoice i = new Invoice();

    public Orders() { 
    }
    public class NewOrder {
        public string orderId { get; set; }
        public string userId { get; set; }

        public List<Item> items = new List<Item>();
        public double netPrice { get; set; }
        public double grossPrice { get; set; }
        public string currency { get; set; }
        public string orderDate { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string companyName { get; set; }
        public string address { get; set; }
        public string postalCode { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string pin { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string deliveryFirstName { get; set; }
        public string deliveryLastName { get; set; }
        public string deliveryCompanyName { get; set; }
        public string deliveryAddress { get; set; }
        public string deliveryPostalCode { get; set; }
        public string deliveryCity { get; set; }
        public string deliveryCountry { get; set; }

        public CodeTitle deliveryType = new CodeTitle();

        public CodeTitle paymentMethod = new CodeTitle();
        public string note { get; set; }
        public string number { get; set; }

        public CodeTitle status = new CodeTitle();
        public string countryCode { get; set; }
        public bool sendToPrint { get; set; }
       // public double deliveryPrice { get; set; }
       // public double discount { get; set; }
       // public double totalWithDiscount { get; set; }
        //  public double total { get; set; }
        public PriceTotal price = new PriceTotal();

        public DiscountCoeff discount = new DiscountCoeff(); //TOOD

        public string invoice { get; set; }
        public string invoiceId { get; set; }

    }

    public class Item {
        public string style { get; set; }
        public string brand { get; set; }
        public string shortdesc_en { get; set; }
        public string shortdesc_hr { get; set; }
        public string sku { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public string color { get; set; }
        public string size { get; set; }
        public string supplier { get; set; }
        public double discount { get; set; }
    }

      public class PriceTotal {
        public double net { get; set; } // amount without vat
        public double gross { get; set; }  // amount with vat
        public double discount { get; set; } // discount (net * discountCoeff)
        public double vat { get; set; }  // vat
        public double netWithDiscount { get; set; } // amount without vat minus discount
        public double netWithDiscountPlusVat { get; set; } // net minus discount + vat
        public double delivery { get; set; } // delivery price
        public double total { get; set; }  // total

    }

    public class OrderOption {
        public double deliveryprice;
        public List<CodeTitle> deliverytype = new List<CodeTitle>();
        public List<CodeTitle> paymentmethod = new List<CodeTitle>();
        public List<CodeTitle> orderstatus = new List<CodeTitle>();
        public List<DiscountCoeff> discountcoeff = new List<DiscountCoeff>();
        public List<Bank> bank = new List<Bank>();
    }

    public class CodeTitle {
        public string code { get; set; }
        public string title { get; set; }
    }

    public class DiscountCoeff {
        public string title { get; set; }
        public string description { get; set; }
        public double coeff { get; set; }
        public string img { get; set; }
    }

    public class Bank {
        public string code { get; set; }
        public string title { get; set; }
        public string link { get; set; }
    }


    #region WebMethods
    [WebMethod]
    public string Init() {
        NewOrder x = new NewOrder();
        x.orderId = null;
        x.userId = null;
        x.items = new List<Item>();
        x.netPrice = 0;
        x.grossPrice = 0;
        x.currency = null;
        x.orderDate = null;
        x.firstName = null;
        x.lastName = null;
        x.companyName = null;
        x.address = null;
        x.postalCode = null;
        x.city = null;
        x.country = null;
        x.pin = null;
        x.phone = null;
        x.email = null;
        x.deliveryFirstName = null;
        x.deliveryLastName = null;
        x.deliveryCompanyName = null;
        x.deliveryAddress = null;
        x.deliveryPostalCode = null;
        x.deliveryCity = null;
        x.deliveryCountry = null;
        x.deliveryType = new CodeTitle();
        x.paymentMethod = new CodeTitle();
        x.note = null;
        x.number = null;
        x.status = new CodeTitle();
        x.countryCode = null;
        x.sendToPrint = false;
        //x.deliveryPrice = 0;
        //x.discount = 0;
        //x.totalWithDiscount = 0;
        x.price = new PriceTotal(); // 0;
        x.discount = new DiscountCoeff();
        x.invoice = null;
        x.invoiceId = null;
        return JsonConvert.SerializeObject(x, Formatting.None);
    }

    [WebMethod]
    public string Load() {
        try {
            OrderOption orderOptions = GetOrderOptions();
            db.CreateDataBase(dataBase, db.orders);
            db.CreateDataBase(dataBase, db.invoice);
            string sql = @"SELECT o.orderId, o.userId, o.items, o.netPrice, o.grossPrice, o.currency, o.orderDate, o.deliveryFirstName, o.deliveryLastName, o.deliveryCompanyName, o.deliveryAddress, o.deliveryPostalCode, o.deliveryCity, o.deliveryCountry, o.deliveryType, o.paymentMethod,
                        u.firstName, u.lastName, u.companyName, u.address, u.postalCode, u.city, u.country, u.pin, u.phone, u.email,
                        o.note, o.number, o.status, o.countryCode, o.sendToPrint, o.deliveryPrice, o.discount, o.total, u.discountCoeff, i.number, i.invoiceId                     
                        FROM orders o
                        LEFT OUTER JOIN users u
                        ON o.userId = u.userId
                        LEFT OUTER JOIN invoice i
                        ON o.orderId = i.orderId
                        ORDER BY o.rowid DESC";
            List<NewOrder> xx = new List<NewOrder>();
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + dataBase))) {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                    using (SQLiteDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            NewOrder x = new NewOrder();
                            x.orderId = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                            x.userId = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                            x.items = GetItems(reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2));
                            x.netPrice = reader.GetValue(3) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(3));
                            x.grossPrice = reader.GetValue(4) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(4));
                            x.currency = reader.GetValue(5) == DBNull.Value ? "" : reader.GetString(5);
                            x.orderDate = reader.GetValue(6) == DBNull.Value ? "" : reader.GetString(6);
                            x.deliveryFirstName = reader.GetValue(7) == DBNull.Value ? "" : reader.GetString(7);
                            x.deliveryLastName = reader.GetValue(8) == DBNull.Value ? "" : reader.GetString(8);
                            x.deliveryCompanyName = reader.GetValue(9) == DBNull.Value ? "" : reader.GetString(9);
                            x.deliveryAddress = reader.GetValue(10) == DBNull.Value ? "" : reader.GetString(10);
                            x.deliveryPostalCode = reader.GetValue(11) == DBNull.Value ? "" : reader.GetString(11);
                            x.deliveryCity = reader.GetValue(12) == DBNull.Value ? "" : reader.GetString(12);
                            x.deliveryCountry = reader.GetValue(13) == DBNull.Value ? "" : reader.GetString(13);
                            x.deliveryType.code = reader.GetValue(14) == DBNull.Value ? "" : reader.GetString(14);
                            x.deliveryType.title = !string.IsNullOrEmpty(x.deliveryType.code) && orderOptions.deliverytype.Any(a => a.code == x.deliveryType.code) ? orderOptions.deliverytype.Find(a => a.code == x.deliveryType.code).title : "";
                            x.paymentMethod.code = reader.GetValue(15) == DBNull.Value ? "" : reader.GetString(15);
                            x.paymentMethod.title = !string.IsNullOrEmpty(x.paymentMethod.code) && orderOptions.paymentmethod.Any(a => a.code == x.paymentMethod.code) ? orderOptions.paymentmethod.Find(a => a.code == x.paymentMethod.code).title : "";
                            x.firstName = reader.GetValue(16) == DBNull.Value ? "" : reader.GetString(16);
                            x.lastName = reader.GetValue(17) == DBNull.Value ? "" : reader.GetString(17);
                            x.companyName = reader.GetValue(18) == DBNull.Value ? "" : reader.GetString(18);
                            x.address = reader.GetValue(19) == DBNull.Value ? "" : reader.GetString(19);
                            x.postalCode = reader.GetValue(20) == DBNull.Value ? "" : reader.GetString(20);
                            x.city = reader.GetValue(21) == DBNull.Value ? "" : reader.GetString(21);
                            x.country = reader.GetValue(22) == DBNull.Value ? "" : reader.GetString(22);
                            x.pin = reader.GetValue(23) == DBNull.Value ? "" : reader.GetString(23);
                            x.phone = reader.GetValue(24) == DBNull.Value ? "" : reader.GetString(24);
                            x.email = reader.GetValue(25) == DBNull.Value ? "" : reader.GetString(25);
                            x.note = reader.GetValue(26) == DBNull.Value ? "" : reader.GetString(26);
                            x.number = reader.GetValue(27) == DBNull.Value ? "" : reader.GetString(27);
                            x.status.code = reader.GetValue(28) == DBNull.Value ? "" : reader.GetString(28);
                            x.status.title = !string.IsNullOrEmpty(x.status.code) && orderOptions.orderstatus.Any(a => a.code == x.status.code) ? orderOptions.orderstatus.Find(a => a.code == x.status.code).title : "";
                            x.countryCode = reader.GetValue(29) == DBNull.Value ? "" : reader.GetString(29);
                            x.sendToPrint = reader.GetValue(30) == DBNull.Value ? false : Convert.ToBoolean(Convert.ToInt32(reader.GetString(30)));
                            x.price.delivery = reader.GetValue(31) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(31));
                            x.price.discount = reader.GetValue(32) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(32));
                            x.price.total = reader.GetValue(33) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(33));
                            x.discount.coeff = reader.GetValue(34) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(34));
                            x.invoice = reader.GetValue(35) == DBNull.Value ? "" : i.InvoiceFormat(Convert.ToInt32(reader.GetString(35)));
                            x.invoiceId = reader.GetValue(36) == DBNull.Value ? "" : reader.GetString(36);
                            xx.Add(x);
                        }
                    }
                }
                connection.Close();

                xx = GetItemsData(xx);

            }
            return JsonConvert.SerializeObject(xx, Formatting.None);
        } catch (Exception e) { return e.Message; }
    }

    [WebMethod]
    public string Get(string userId) {
        try {
            OrderOption orderOptions = GetOrderOptions();
            db.CreateDataBase(dataBase, db.orders);
            db.CreateDataBase(dataBase, db.invoice);
            string sql = string.Format(@"SELECT o.orderId, o.userId, o.items, o.netPrice, o.grossPrice, o.currency, o.orderDate, o.deliveryFirstName, o.deliveryLastName, o.deliveryCompanyName, o.deliveryAddress, o.deliveryPostalCode, o.deliveryCity, o.deliveryCountry, o.deliveryType, o.paymentMethod,
                        u.firstName, u.lastName, u.companyName, u.address, u.postalCode, u.city, u.country, u.pin, u.phone, u.email,
                        o.note, o.number, o.status, o.countryCode, o.sendToPrint, o.deliveryPrice, o.discount, o.total, u.discountCoeff, i.number, i.invoiceId           
                        FROM orders o
                        LEFT OUTER JOIN users u
                        ON o.userId = u.userId
                        LEFT OUTER JOIN invoice i
                        ON o.orderId = i.orderId
                        WHERE o.userId = '{0}'
                        ORDER BY o.rowid DESC", userId);
            List<NewOrder> xx = new List<NewOrder>();
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + dataBase))) {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                    using (SQLiteDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            NewOrder x = new NewOrder();
                            x.orderId = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                            x.userId = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                            x.items = GetItems(reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2));
                            x.netPrice = reader.GetValue(3) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(3));
                            x.grossPrice = reader.GetValue(4) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(4));
                            x.currency = reader.GetValue(5) == DBNull.Value ? "" : reader.GetString(5);
                            x.orderDate = reader.GetValue(6) == DBNull.Value ? "" : reader.GetString(6);
                            x.deliveryFirstName = reader.GetValue(7) == DBNull.Value ? "" : reader.GetString(7);
                            x.deliveryLastName = reader.GetValue(8) == DBNull.Value ? "" : reader.GetString(8);
                            x.deliveryCompanyName = reader.GetValue(9) == DBNull.Value ? "" : reader.GetString(9);
                            x.deliveryAddress = reader.GetValue(10) == DBNull.Value ? "" : reader.GetString(10);
                            x.deliveryPostalCode = reader.GetValue(11) == DBNull.Value ? "" : reader.GetString(11);
                            x.deliveryCity = reader.GetValue(12) == DBNull.Value ? "" : reader.GetString(12);
                            x.deliveryCountry = reader.GetValue(13) == DBNull.Value ? "" : reader.GetString(13);
                            x.deliveryType.code = reader.GetValue(14) == DBNull.Value ? "" : reader.GetString(14);
                            x.deliveryType.title = !string.IsNullOrEmpty(x.deliveryType.code) && orderOptions.deliverytype.Any(a => a.code == x.deliveryType.code) ? orderOptions.deliverytype.Find(a => a.code == x.deliveryType.code).title : "";
                            x.paymentMethod.code = reader.GetValue(15) == DBNull.Value ? "" : reader.GetString(15);
                            x.paymentMethod.title = !string.IsNullOrEmpty(x.paymentMethod.code) && orderOptions.paymentmethod.Any(a => a.code == x.paymentMethod.code) ? orderOptions.paymentmethod.Find(a => a.code == x.paymentMethod.code).title : "";
                            x.firstName = reader.GetValue(16) == DBNull.Value ? "" : reader.GetString(16);
                            x.lastName = reader.GetValue(17) == DBNull.Value ? "" : reader.GetString(17);
                            x.companyName = reader.GetValue(18) == DBNull.Value ? "" : reader.GetString(18);
                            x.address = reader.GetValue(19) == DBNull.Value ? "" : reader.GetString(19);
                            x.postalCode = reader.GetValue(20) == DBNull.Value ? "" : reader.GetString(20);
                            x.city = reader.GetValue(21) == DBNull.Value ? "" : reader.GetString(21);
                            x.country = reader.GetValue(22) == DBNull.Value ? "" : reader.GetString(22);
                            x.pin = reader.GetValue(23) == DBNull.Value ? "" : reader.GetString(23);
                            x.phone = reader.GetValue(24) == DBNull.Value ? "" : reader.GetString(24);
                            x.email = reader.GetValue(25) == DBNull.Value ? "" : reader.GetString(25);
                            x.note = reader.GetValue(26) == DBNull.Value ? "" : reader.GetString(26);
                            x.number = reader.GetValue(27) == DBNull.Value ? "" : reader.GetString(27);
                            x.status.code = reader.GetValue(28) == DBNull.Value ? "" : reader.GetString(28);
                            x.status.title = !string.IsNullOrEmpty(x.status.code) && orderOptions.orderstatus.Any(a => a.code == x.status.code) ? orderOptions.orderstatus.Find(a => a.code == x.status.code).title : "";
                            x.countryCode = reader.GetValue(29) == DBNull.Value ? "" : reader.GetString(29);
                            x.sendToPrint = reader.GetValue(30) == DBNull.Value ? false : Convert.ToBoolean(Convert.ToInt32(reader.GetString(30)));
                            x.price.delivery = reader.GetValue(31) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(31));
                            x.price.discount = reader.GetValue(32) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(32));
                            x.price.total = reader.GetValue(33) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(33));
                            x.discount.coeff = reader.GetValue(34) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(34));
                            x.invoice = reader.GetValue(35) == DBNull.Value ? "" : i.InvoiceFormat(Convert.ToInt32(reader.GetString(35)));
                            x.invoiceId = reader.GetValue(36) == DBNull.Value ? "" : reader.GetString(36);
                            // x.total = x.totalWithDiscount + x.deliveryPrice;
                            xx.Add(x);
                        }
                    }  
                } 
                connection.Close();

                xx = GetItemsData(xx);
            }
            return JsonConvert.SerializeObject(xx, Formatting.None);
        } catch (Exception e) { return e.Message; }
    }

     [WebMethod]
    public string GetOrderByOrderId(string orderId) {
        try {
            OrderOption orderOptions = GetOrderOptions();
            db.CreateDataBase(dataBase, db.orders);
            db.CreateDataBase(dataBase, db.invoice);
            NewOrder x = new NewOrder();
            string sql = string.Format(@"SELECT o.orderId, o.userId, o.items, o.netPrice, o.grossPrice, o.currency, o.orderDate, o.deliveryFirstName, o.deliveryLastName, o.deliveryCompanyName, o.deliveryAddress, o.deliveryPostalCode, o.deliveryCity, o.deliveryCountry, o.deliveryType, o.paymentMethod,
                        u.firstName, u.lastName, u.companyName, u.address, u.postalCode, u.city, u.country, u.pin, u.phone, u.email,
                        o.note, o.number, o.status, o.countryCode, o.sendToPrint, o.deliveryPrice, o.discount, o.total, u.discountCoeff, i.number, i.invoiceId           
                        FROM orders o
                        LEFT OUTER JOIN users u
                        ON o.userId = u.userId
                        LEFT OUTER JOIN invoice i
                        ON o.orderId = i.orderId
                        WHERE o.orderId = '{0}'
                        ORDER BY o.rowid DESC", orderId);
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + dataBase))) {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                    using (SQLiteDataReader reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            x.orderId = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                            x.userId = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                            x.items = GetItems(reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2));
                            x.netPrice = reader.GetValue(3) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(3));
                            x.grossPrice = reader.GetValue(4) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(4));
                            x.currency = reader.GetValue(5) == DBNull.Value ? "" : reader.GetString(5);
                            x.orderDate = reader.GetValue(6) == DBNull.Value ? "" : reader.GetString(6);
                            x.deliveryFirstName = reader.GetValue(7) == DBNull.Value ? "" : reader.GetString(7);
                            x.deliveryLastName = reader.GetValue(8) == DBNull.Value ? "" : reader.GetString(8);
                            x.deliveryCompanyName = reader.GetValue(9) == DBNull.Value ? "" : reader.GetString(9);
                            x.deliveryAddress = reader.GetValue(10) == DBNull.Value ? "" : reader.GetString(10);
                            x.deliveryPostalCode = reader.GetValue(11) == DBNull.Value ? "" : reader.GetString(11);
                            x.deliveryCity = reader.GetValue(12) == DBNull.Value ? "" : reader.GetString(12);
                            x.deliveryCountry = reader.GetValue(13) == DBNull.Value ? "" : reader.GetString(13);
                            x.deliveryType.code = reader.GetValue(14) == DBNull.Value ? "" : reader.GetString(14);
                            x.deliveryType.title = !string.IsNullOrEmpty(x.deliveryType.code) && orderOptions.deliverytype.Any(a => a.code == x.deliveryType.code) ? orderOptions.deliverytype.Find(a => a.code == x.deliveryType.code).title : "";
                            x.paymentMethod.code = reader.GetValue(15) == DBNull.Value ? "" : reader.GetString(15);
                            x.paymentMethod.title = !string.IsNullOrEmpty(x.paymentMethod.code) && orderOptions.paymentmethod.Any(a => a.code == x.paymentMethod.code) ? orderOptions.paymentmethod.Find(a => a.code == x.paymentMethod.code).title : "";
                            x.firstName = reader.GetValue(16) == DBNull.Value ? "" : reader.GetString(16);
                            x.lastName = reader.GetValue(17) == DBNull.Value ? "" : reader.GetString(17);
                            x.companyName = reader.GetValue(18) == DBNull.Value ? "" : reader.GetString(18);
                            x.address = reader.GetValue(19) == DBNull.Value ? "" : reader.GetString(19);
                            x.postalCode = reader.GetValue(20) == DBNull.Value ? "" : reader.GetString(20);
                            x.city = reader.GetValue(21) == DBNull.Value ? "" : reader.GetString(21);
                            x.country = reader.GetValue(22) == DBNull.Value ? "" : reader.GetString(22);
                            x.pin = reader.GetValue(23) == DBNull.Value ? "" : reader.GetString(23);
                            x.phone = reader.GetValue(24) == DBNull.Value ? "" : reader.GetString(24);
                            x.email = reader.GetValue(25) == DBNull.Value ? "" : reader.GetString(25);
                            x.note = reader.GetValue(26) == DBNull.Value ? "" : reader.GetString(26);
                            x.number = reader.GetValue(27) == DBNull.Value ? "" : reader.GetString(27);
                            x.status.code = reader.GetValue(28) == DBNull.Value ? "" : reader.GetString(28);
                            x.status.title = !string.IsNullOrEmpty(x.status.code) && orderOptions.orderstatus.Any(a => a.code == x.status.code) ? orderOptions.orderstatus.Find(a => a.code == x.status.code).title : "";
                            x.countryCode = reader.GetValue(29) == DBNull.Value ? "" : reader.GetString(29);
                            x.sendToPrint = reader.GetValue(30) == DBNull.Value ? false : Convert.ToBoolean(Convert.ToInt32(reader.GetString(30)));
                            x.price.delivery = reader.GetValue(31) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(31));
                            x.price.discount = reader.GetValue(32) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(32));
                            x.price.total = reader.GetValue(33) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(33));
                            x.discount.coeff = reader.GetValue(34) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(34));
                            x.invoice = reader.GetValue(35) == DBNull.Value ? "" : i.InvoiceFormat(Convert.ToInt32(reader.GetString(35)));
                            x.invoiceId = reader.GetValue(36) == DBNull.Value ? "" : reader.GetString(36);
                        }
                    }  
                } 
                connection.Close();
                x = GetItemsData(x);
            }
            return JsonConvert.SerializeObject(x, Formatting.None);
        } catch (Exception e) { return e.Message; }
    }

    [WebMethod]
    public string Save(Users.NewUser user, NewOrder order, List<Cart.NewCart> cart, string lang, bool sendToPrint) {
            try {
            //    OrderOption orderOptions = JsonConvert.DeserializeObject<OrderOption>(GetOrderOptions());
            OrderOption orderOptions = GetOrderOptions();
            db.CreateDataBase(dataBase, db.orders);
            order.orderId = order.orderId == null ? Convert.ToString(Guid.NewGuid()) : order.orderId;
                    /***************check stock**************
                    Products p = new Products();
                    List<Products.Stock> stock = new List<Products.Stock>();
                    stock = p.GetSkuStockUtt(cart);
                    ***************************************/
            Price pr = new Price();
            foreach (Cart.NewCart item in cart) {
                foreach(Products.Stock variant in item.data) {
                    /***************check stock**************
                    double uttstock = Convert.ToDouble(stock.Where(a => a.sku == variant.sku).Select(a => a.uttstock).FirstOrDefault());
                    double suppstock = Convert.ToDouble(stock.Where(a => a.sku == variant.sku).Select(a => a.suppstock).FirstOrDefault());
                    if (uttstock + suppstock < variant.quantity) {
                        throw new Exception(T.Tran("no stock for", lang) + " " + variant.style + ", sku:" + variant.sku);
                    }
                     ***************************************/
                    Item i = new Item();
                    i.sku = variant.sku;
                    i.price = Math.Round(variant.myprice.net * pr.GetCoeff().eurorate, 2);
                    i.quantity = variant.quantity;
                    i.color = variant.color;
                    i.size = variant.size;
                    i.style = variant.style;
                    i.shortdesc_en = variant.shortdesc_en;
                    i.supplier = variant.supplier;
                    order.items.Add(i);
                    order.netPrice += Math.Round(variant.myprice.net * pr.GetCoeff().eurorate * variant.quantity, 2);
                    order.grossPrice += Math.Round(variant.myprice.gross * pr.GetCoeff().eurorate * variant.quantity, 2);
                }
            }

            order.userId = user.userId;
            order.firstName = user.firstName;
            order.lastName = user.lastName;
            order.companyName = user.companyName;
            order.address = user.address;
            order.postalCode = user.postalCode;
            order.city = user.city;
            order.country = user.country.Name;
            order.pin = user.pin;
            order.phone = user.phone;
            order.email = user.email;
            order.deliveryFirstName = user.deliveryFirstName;
            order.deliveryLastName = user.deliveryLastName;
            order.deliveryCompanyName = user.deliveryCompanyName;
            order.deliveryAddress = user.deliveryAddress;
            order.deliveryPostalCode = user.deliveryPostalCode;
            order.deliveryCity = user.deliveryCity;
            order.deliveryCountry = user.deliveryCountry.Name;
            order.deliveryType.code = user.deliveryType;
            order.deliveryType.title = !string.IsNullOrEmpty(order.deliveryType.code) ? orderOptions.deliverytype.Where(a => a.code == order.deliveryType.code).FirstOrDefault().title : "";
            order.paymentMethod.code = user.paymentMethod;
            order.paymentMethod.title = !string.IsNullOrEmpty(order.paymentMethod.code) ? orderOptions.paymentmethod.Where(a => a.code == order.paymentMethod.code).FirstOrDefault().title : "";
            order.status.code = "0";
            order.status.title = !string.IsNullOrEmpty(order.status.code) ? orderOptions.orderstatus.Where(a => a.code == order.status.code).FirstOrDefault().title : "";
            order.countryCode = user.deliveryCountry.Code;

            //order.deliveryPrice = order.netPrice < 1000 ? 35 : 0;
            //order.discount = order.grossPrice * user.discount.coeff;
            //order.totalWithDiscount = order.grossPrice - order.discount;
           // order.total = order.totalWithDiscount + order.discount;
            order.sendToPrint = sendToPrint;
            order.discount.coeff = user.discount.coeff;

            string sql = @"INSERT OR REPLACE INTO orders VALUES  
                       (@orderId, @userId, @items, @netPrice, @grossPrice, @currency, @orderDate, @deliveryFirstName, @deliveryLastName, @deliveryCompanyName, @deliveryAddress, @deliveryPostalCode, @deliveryCity, @deliveryCountry, @deliveryType, @paymentMethod, @note, @number, @status, @countryCode, @sendToPrint, @deliveryPrice, @discount, @total)";
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + dataBase))) {
                connection.Open();
                order.number = !string.IsNullOrEmpty(order.number) ? order.number : getNewOrderNumber(connection);
                using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                    command.Parameters.Add(new SQLiteParameter("orderId", order.orderId));
                    command.Parameters.Add(new SQLiteParameter("userId", order.userId));
                    command.Parameters.Add(new SQLiteParameter("items", SetItems(order)));
                    command.Parameters.Add(new SQLiteParameter("netPrice", order.netPrice));
                    command.Parameters.Add(new SQLiteParameter("grossPrice", order.grossPrice));
                    command.Parameters.Add(new SQLiteParameter("currency", order.currency));
                    command.Parameters.Add(new SQLiteParameter("orderDate", order.orderDate));
                    command.Parameters.Add(new SQLiteParameter("deliveryFirstName", order.deliveryFirstName));
                    command.Parameters.Add(new SQLiteParameter("deliveryLastName", order.deliveryLastName));
                    command.Parameters.Add(new SQLiteParameter("deliveryCompanyName", order.deliveryCompanyName));
                    command.Parameters.Add(new SQLiteParameter("deliveryAddress", order.deliveryAddress));
                    command.Parameters.Add(new SQLiteParameter("deliveryPostalCode", order.deliveryPostalCode));
                    command.Parameters.Add(new SQLiteParameter("deliveryCity", order.deliveryCity));
                    command.Parameters.Add(new SQLiteParameter("deliveryCountry", order.deliveryCountry));
                    command.Parameters.Add(new SQLiteParameter("deliveryType", order.deliveryType.code));
                    command.Parameters.Add(new SQLiteParameter("paymentMethod", order.paymentMethod.code));
                    command.Parameters.Add(new SQLiteParameter("note", order.note));
                    command.Parameters.Add(new SQLiteParameter("number", order.number));
                    command.Parameters.Add(new SQLiteParameter("status", order.status.code));
                    command.Parameters.Add(new SQLiteParameter("countryCode", order.countryCode));
                    command.Parameters.Add(new SQLiteParameter("sendToPrint", order.sendToPrint));
                    command.Parameters.Add(new SQLiteParameter("deliveryPrice", order.price.delivery));
                    command.Parameters.Add(new SQLiteParameter("discount", order.price.discount));
                    command.Parameters.Add(new SQLiteParameter("total", order.price.total));
                    command.ExecuteNonQuery();
                } 
                connection.Close();
            }

            PrintPdf p = new PrintPdf();
            string offerPdf = p.OfferPdf(order, false, lang);
            
            /*
             * TODO
            Mail m = new Mail();
            m.SendOrder(x);
            */
            //return ("OK");
            return JsonConvert.SerializeObject(order, Formatting.None);
        } catch (Exception e) { return e.Message; }
    }

    [WebMethod]
    public string Update(NewOrder order) {
            try {
            db.CreateDataBase(dataBase, db.orders);
            string sql = @"UPDATE orders
                        SET userId = @userId , items = @items, netPrice = @netPrice, grossPrice = @grossPrice, currency = @currency, orderDate = @orderDate,
                        deliveryFirstName = @deliveryFirstName, deliveryLastName = @deliveryLastName, deliveryCompanyName = @deliveryCompanyName, deliveryAddress = @deliveryAddress,
                        deliveryPostalCode = @deliveryPostalCode, deliveryCity = @deliveryCity, deliveryCountry = @deliveryCountry,
                        deliveryType = @deliveryType, paymentMethod = @paymentMethod, note = @note, number = @number, status = @status,
                        countryCode = @countryCode, sendToPrint = @sendToPrint, deliveryPrice = @deliveryPrice, discount = @discount, @totalWithDiscount = totalWithDiscount
                        WHERE orderId = @orderId";
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + dataBase))) {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                command.Parameters.Add(new SQLiteParameter("orderId", order.orderId));
                command.Parameters.Add(new SQLiteParameter("userId", order.userId));
                command.Parameters.Add(new SQLiteParameter("items", SetItems(order)));
                command.Parameters.Add(new SQLiteParameter("netPrice", order.netPrice));
                command.Parameters.Add(new SQLiteParameter("grossPrice", order.grossPrice));
                command.Parameters.Add(new SQLiteParameter("currency", order.currency));
                command.Parameters.Add(new SQLiteParameter("orderDate", order.orderDate));
                command.Parameters.Add(new SQLiteParameter("deliveryFirstName", order.deliveryFirstName));
                command.Parameters.Add(new SQLiteParameter("deliveryLastName", order.deliveryLastName));
                command.Parameters.Add(new SQLiteParameter("deliveryCompanyName", order.deliveryCompanyName));
                command.Parameters.Add(new SQLiteParameter("deliveryAddress", order.deliveryAddress));
                command.Parameters.Add(new SQLiteParameter("deliveryPostalCode", order.deliveryPostalCode));
                command.Parameters.Add(new SQLiteParameter("deliveryCity", order.deliveryCity));
                command.Parameters.Add(new SQLiteParameter("deliveryCountry", order.deliveryCountry));
                command.Parameters.Add(new SQLiteParameter("deliveryType", order.deliveryType.code));
                command.Parameters.Add(new SQLiteParameter("paymentMethod", order.paymentMethod.code));
                command.Parameters.Add(new SQLiteParameter("note", order.note));
                command.Parameters.Add(new SQLiteParameter("number", order.number));
                command.Parameters.Add(new SQLiteParameter("status", order.status.code));
                command.Parameters.Add(new SQLiteParameter("countryCode", order.countryCode));
                command.Parameters.Add(new SQLiteParameter("sendToPrint", order.sendToPrint));
                command.Parameters.Add(new SQLiteParameter("deliveryPrice", order.price.delivery));
                command.Parameters.Add(new SQLiteParameter("discount", order.price.discount));
                command.Parameters.Add(new SQLiteParameter("totalWithDiscount", order.price.total));
                command.ExecuteNonQuery();
                connection.Close();
            }
            return JsonConvert.SerializeObject(order, Formatting.None);
        } catch (Exception e) { return e.Message; }
    }

    [WebMethod]
    public string Delete(string id) {
        try {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + dataBase));
            connection.Open();
            string sql = @"DELETE FROM orders WHERE orderId = @orderId";
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.Add(new SQLiteParameter("orderId", id));
            command.ExecuteNonQuery();
            connection.Close();
            return "OK";
        } catch (Exception e) { return (e.Message); }
    }

    [WebMethod]
    public string GetOrderOptionsJson() {
        return JsonConvert.SerializeObject(GetOrderOptions(), Formatting.None);
    }

    [WebMethod]
    public string SaveOrderOptions(OrderOption x) {
        return WriteJsonFile(orderOptionsFile, JsonConvert.SerializeObject(x, Formatting.None));
    }

    [WebMethod]
    public string PostUtt(string data) {
        // Create a request using a URL that can receive a post. 
       // WebRequest request = WebRequest.Create("https://utteam.com/api/rest/order/accountstatus?");
        WebRequest request = WebRequest.Create("https://utteam.com/api/rest/order/stockcheck");
        // Set the Method property of the request to POST.
        request.Method = "POST";
        string postData = data;
        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(postData);
        // Set the ContentType property of the WebRequest.
        request.ContentType = "application/json"; // "application/x-www-form-urlencoded";
        // Set the ContentLength property of the WebRequest.
        request.ContentLength = byteArray.Length;
        // Get the request stream.
        Stream dataStream = request.GetRequestStream();
        // Write the data to the request stream.
        dataStream.Write(byteArray, 0, byteArray.Length);
        // Close the Stream object.
        dataStream.Close();
        // Get the response.
        WebResponse response = request.GetResponse();
        // Display the status.
        Console.WriteLine(((HttpWebResponse)response).StatusDescription);
        // Get the stream containing content returned by the server.
        dataStream = response.GetResponseStream();
        // Open the stream using a StreamReader for easy access.
        StreamReader reader = new StreamReader(dataStream);
        // Read the content.
        string responseFromServer = reader.ReadToEnd();
        // Display the content.
        Console.WriteLine(responseFromServer);
        // Clean up the streams.
        reader.Close();
        dataStream.Close();
        response.Close();
        string x = responseFromServer;
        return x;
    }

    [WebMethod]
    public string GetTotalPrice(List<Cart.NewCart> groupingCart, Users.NewUser user, double course) {
        try {
            OrderOption orderOptions = GetOrderOptions();
            Price p = new Price();
            Price.PriceCoeff priceCoeff = p.GetCoeff();
            PriceTotal x = new PriceTotal();
            foreach (Cart.NewCart c in groupingCart) {
                x.net += Math.Round(c.data.Sum(a => a.myprice.net * a.quantity), 2);
                x.gross += Math.Round(c.data.Sum(a => a.myprice.gross * a.quantity), 2);
            }
            x.discount = Math.Round(x.net * (user != null ? user.discount.coeff : 0), 2);
            //x.vat = (user != null ? (user.deliveryCountry.Code == "HR" ? Math.Round(x.net * 0.25, 2) : 0) : Math.Round(x.net * 0.25, 2));
            x.vat = (user != null ? (user.deliveryCountry.Code == "HR" ? Math.Round(x.net * (priceCoeff.vat - 1), 2) : 0) : Math.Round(x.net * (priceCoeff.vat - 1), 2));
            x.netWithDiscount = x.net - x.discount;
            x.netWithDiscountPlusVat = x.netWithDiscount + x.vat;
            // x.delivery = (x.gross * course) < 1000 ? Math.Round((30 / course), 2) : 0;
            x.delivery = (x.gross * course) < 1000 ? Math.Round((orderOptions.deliveryprice / course), 2) : 0;
            x.total = x.netWithDiscountPlusVat + x.delivery;
            return JsonConvert.SerializeObject(x, Formatting.None);
        } catch (Exception e) {
            return e.Message;
        }
        
    }
    #endregion WebMethods

    #region Methods
    private List<Item> GetItems(string items) {
        List<Item> xx = new List<Item>();
        string[] str = items.Split(';');
        foreach(string s in str) {
            if (!string.IsNullOrEmpty(s)){
                Item x = new Item();
                string[] str_ = s.Split(':');
                x.sku = str_[0];
                x.quantity = Convert.ToInt32(str_[1]);
                if (str_.Length > 2) {
                    x.color = str_[2];
                    x.size = str_[3];
                    if (str_.Length > 4) {
                        x.supplier = str_[4];
                        if (str_.Length > 5) {
                            x.style = str_[5];
                            if (str_.Length > 6) {
                                x.price = Convert.ToDouble(str_[6]);
                                if (str_.Length > 7) {
                                    x.discount = Convert.ToDouble(str_[7]);
                                }
                            }
                        }
                    }
                }
                xx.Add(x);
            }
        }
        return xx;
    }

    private string SetItems(NewOrder  order) {
        string str = "";
        foreach (Item item in order.items) {
            //str = str + item.sku + ":" + item.quantity + ":" + item.color + ":" + item.size + ":" + item.supplier + ";";
            str = str + item.sku + ":" + item.quantity + ":" + item.color + ":" + item.size + ":" + item.supplier + ":" + item.style + ":" + item.price + ":" + order.discount.coeff + ";";
        }
        return str;
    }

    private string getNewOrderNumber(SQLiteConnection connection) {
        string newNumber = "";
        int startNumber = Convert.ToInt32(ConfigurationManager.AppSettings["orderNumberStartsWith"]);
        string sql = "SELECT MAX(rowid) FROM orders";
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        SQLiteDataReader reader = command.ExecuteReader();
        int lastRow = 0;
        while (reader.Read()) {
            lastRow = reader.GetValue(0) == DBNull.Value ? 0 : reader.GetInt32(0);
        }
        newNumber = (lastRow + startNumber).ToString() + "/" + DateTime.Today.Year.ToString();
        return newNumber;
    }

    public void CreateFolder(string path) {
        if (!Directory.Exists(Server.MapPath(path))) {
            Directory.CreateDirectory(Server.MapPath(path));
        }
    }

    public string WriteJsonFile(string filename, string json) {
        try {
            CreateFolder("~/App_Data/json/");
            string path = string.Format(@"~/App_Data/json/{0}.json", filename);
            File.WriteAllText(Server.MapPath(path), json);
            return "OK";
        } catch (Exception e) {
            return e.Message;
        }
    }

    public OrderOption GetOrderOptions() {
        try {
            string path = string.Format("~/App_Data/json/{0}.json", orderOptionsFile);
            string json = null;
            if (File.Exists(Server.MapPath(path))) {
                json = File.ReadAllText(Server.MapPath(path));
            }
            return JsonConvert.DeserializeObject<OrderOption>(json);
        } catch (Exception e) {
            return new OrderOption();
        }
    }

    //TODO: Refactorize
    private List<NewOrder> GetItemsData(List<NewOrder> xx) {
        int idx = 0;
        foreach (NewOrder x in xx) {
            if(x.items.Any(a => a.style == null) == false) {
                List<Item> ii = new List<Item>();
                int idx1 = 0;
                foreach (Item i in x.items) {
                    Item i_ = i.style == null ? i : FillData(i);
                    idx1++;
                    ii.Add(i_);
                }
                xx[idx].items = ii;
            }
            idx++;
        }
        return xx;
    }

    //TODO: Refactorize
    private NewOrder GetItemsData(NewOrder x) {
        if(x.items.Any(a => a.style == null) == false) {
            List<Item> ii = new List<Item>();
            int idx1 = 0;
            foreach (Item i in x.items) {
                Item i_ = i.style == null ? i : FillData(i);
                idx1++;
                ii.Add(i_);
            }
            x.items = ii;
        }
        return x;
    }

    private Item FillData(Item x) {
        string sql = string.Format("SELECT brand, shortdesc_en, shortdesc_hr from style WHERE style = '{0}'", x.style);
        using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath(string.Format("~/App_Data/{0}", productDataBase)))) {
            connection.Open();
            using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                using (SQLiteDataReader reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        x.brand = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                        x.shortdesc_en = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
                        x.shortdesc_hr = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2);
                    }
                }
                connection.Close();
            }
        }
        return x;
    }
    #endregion Methods

}
