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
        public string number;
        public int year;
        public string orderId;
        public string userId;
    }

    public NewInvoice Save(Orders.NewOrder order) {
        try {
            NewInvoice x = new NewInvoice();
            x.invoiceId = Guid.NewGuid().ToString();
            x.year = DateTime.Now.Year;
            x.orderId = order.orderId;
            x.userId = order.userId;
            db.CreateDataBase(dataBase, db.invoice);
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + HttpContext.Current.Server.MapPath("~/App_Data/" + dataBase))) {
                connection.Open();
                x.number = !string.IsNullOrEmpty(order.invoice) ? order.invoice : getNewInvoiceNumber(connection);
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

    private string getNewInvoiceNumber(SQLiteConnection connection) {
        int startNumber = 1;
        int lastRow = 0;
        string sql = "SELECT MAX(rowid) FROM invoice";
        using (SQLiteCommand command = new SQLiteCommand(sql, connection)) {
            using (SQLiteDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    lastRow = reader.GetValue(0) == DBNull.Value ? 0 : reader.GetInt32(0);
                }
            }
        }
        return string.Format("{0}-1-1", (lastRow + startNumber).ToString());
    }


}