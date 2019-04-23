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

[WebService(Namespace = "https://promo-tekstil.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class Price : System.Web.Services.WebService{
    string productDataBase = ConfigurationManager.AppSettings["ProductDataBase"];
    string priceCoeffFile = "pricecoeff";
    public Price() {
    }

    #region Class
    public class NewPrice {
        public double net { get; set; }
        public double gross { get; set; }
    }
    public class PriceCoeff {
        public double vat { get; set; }
        public double eurorate { get; set; }

        public List<Coeff> category = new List<Coeff>();

        public List<Coeff> brand = new List<Coeff>();

        public List<Coeff> style = new List<Coeff>();
    }
    public class Coeff {
        public string code { get; set; }
        public double coeff { get; set; }
        public double value { get; set; }
    }
    #endregion Class

    #region WebMethods
    [WebMethod]
    public string GetPriceCoeff(string filename) {
        string json = GetJsonFile(filename);
        PriceCoeff x = new PriceCoeff();
        if(GetJsonFile(filename) != null) {
            x = JsonConvert.DeserializeObject<PriceCoeff>(GetJsonFile(filename));
        } else {
            x = InitPriceJson();
            WriteJsonFile(priceCoeffFile, JsonConvert.SerializeObject(x, Formatting.None));
        }
        return JsonConvert.SerializeObject(x, Formatting.None);
    }

    [WebMethod]
    public string Save(PriceCoeff x) {
        return WriteJsonFile(priceCoeffFile, JsonConvert.SerializeObject(x, Formatting.None));
    }
    #endregion WebMethods

    #region Methods
    public NewPrice GetPrice(string category_code, string brand_code, string style_code, double price) {
        try {
            PriceCoeff p = new PriceCoeff();
            p = GetCoeff();
            Coeff category = new Coeff();
            if (p.category.Count > 0) {
                category = p.category.Where(a => a.code.ToLower() == category_code.ToLower()).FirstOrDefault();
            }
            if (category == null) {
                category = new Coeff();
                category.coeff = 1;
                category.value = 0;
            }

            Coeff brand = new Coeff();
            if (p.brand.Count > 0) {
                brand = p.brand.Where(a => a.code.ToLower() == brand_code.ToLower()).FirstOrDefault();
            }
            if (brand == null) {
                brand = new Coeff();
                brand.coeff = 1;
                brand.value = 0;
            }

            Coeff style = new Coeff();
            if (p.style.Count > 0) {
                style = p.style.Where(a => a.code.ToLower() == style_code.ToLower()).FirstOrDefault();
            }
            if (style == null) {
                style = new Coeff();
                style.coeff = 1;
                style.value = 0;
            }

            NewPrice x = new NewPrice();
            x.net = price;
            if (category.value > 0) {
                x.net = x.net + category.value;
            } else {
                x.net = x.net * category.coeff;
            }
            if (brand.value > 0) {
                x.net = x.net + brand.value;
            } else {
                x.net = x.net * brand.coeff;
            }
            if (style.value > 0) {
                x.net = x.net + style.value;
            } else {
                x.net = x.net * style.coeff;
            }

            x.net = Math.Round(x.net, 2);
            x.gross = Math.Round((x.net) * p.vat, 2);

            /* OLD
            x.gross = Math.Round((x.net) * p.vat, 2);
            x.net = Math.Round(x.net, 2);
            */
            return x;
        } catch (Exception e) {
            return new NewPrice();
        }
    }

    public PriceCoeff GetCoeff() {
        string json = GetJsonFile(priceCoeffFile);
        PriceCoeff x = new PriceCoeff();
        x = JsonConvert.DeserializeObject<PriceCoeff>(GetJsonFile(priceCoeffFile));
        return x;
    }

    private string GetJsonFile(string filename) {
        try {
            string path = string.Format(@"~/App_Data/json/{0}.json", filename);
            string json = null;
            if (File.Exists(Server.MapPath(path))) {
                json = File.ReadAllText(Server.MapPath(path));
            }
            return json;
        } catch (Exception e) {
            return e.Message;
        }
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

    private PriceCoeff InitPriceJson() {
        try {
            PriceCoeff x = new PriceCoeff();
            x.vat = Convert.ToDouble(ConfigurationManager.AppSettings["vatCoeff"]); // 1.25;
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + productDataBase));
            connection.Open();
            x.category = GetDistinct("category_code", connection);
            x.brand = GetDistinct("brand_code", connection);
            x.style.Add(new Coeff());
            x.eurorate = 7.5;
            connection.Close();
            return x;
        } catch (Exception e) {
            return null;
        }
    }

    private List<Coeff> GetDistinct(string field, SQLiteConnection connection ) {
        SQLiteCommand command = new SQLiteCommand(string.Format("SELECT DISTINCT {0} FROM product", field), connection);
        SQLiteDataReader reader = command.ExecuteReader();
        List<Coeff> xx = new List<Coeff>();
        while (reader.Read()) {
            Coeff x = new Coeff();
            x.code = reader.GetValue(0) == DBNull.Value ? "" : reader.GetString(0);
            x.coeff = 1;
            x.value = 0;
            xx.Add(x);
        }
        return xx;
    }
    #endregion Methods

}
