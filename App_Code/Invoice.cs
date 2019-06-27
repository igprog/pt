using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Newtonsoft.Json;
using System.Data.SQLite;
using Igprog;

/// <summary>
/// Invoice
/// </summary>
public class Invoice {
    string dataBase = ConfigurationManager.AppSettings["AppDataBase"];
    DataBase db = new DataBase();

    public Invoice() {
    }

     public class NewInvoice {
        public string invoiceId;
        public int number;
        public int year;
        public string orderId;
        public string userId;
    }

    public NewInvoice Save(Orders.NewOrder order) {
        try {
            NewInvoice x = new NewInvoice();
            x.invoiceId = string.IsNullOrEmpty(order.invoiceId) ? Guid.NewGuid().ToString() : order.invoiceId;
            x.year = DateTime.Now.Year;
            x.orderId = order.orderId;
            x.userId = order.userId;
            db.CreateDataBase(dataBase, db.invoice);
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + HttpContext.Current.Server.MapPath("~/App_Data/" + dataBase))) {
                connection.Open();
                x.number = !string.IsNullOrEmpty(order.invoice) ? Convert.ToInt32(order.invoice.Split('-')[0]) : getNewInvoiceNumber(connection);
                string sql = string.Format(@"INSERT OR REPLACE INTO invoice VALUES  
                       ('{0}', '{1}', '{2}', '{3}', '{4}')", x.invoiceId, x.number, x.year, x.orderId, x.userId);
                using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            return x;
        } catch (Exception e) {
            return new NewInvoice();
        }
    }

    //  1/WEB/1/1

     private int getNewInvoiceNumber(SQLiteConnection connection) {
        int x = 1;
        string lastNumber = null;
        string sql = string.Format("SELECT max(number) FROM invoice WHERE year = '{0}'", DateTime.Now.Year);
        using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
            using (SQLiteDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    lastNumber = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
                }
            }
        }
        if(!string.IsNullOrEmpty(lastNumber)) {
            x = Convert.ToInt32(lastNumber) + 1;
        }
        return x;
    }

}