using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Data.SQLite;
using Igprog;


[WebService(Namespace = "https://promo-tekstil.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class Cart : System.Web.Services.WebService {

    public Cart() {
    }

    #region Class
    public class NewCart {
        public string style { get; set; }
        public List<Products.Stock> data = new List<Products.Stock>();
    }
    #endregion Class

    #region WebMethods
    [WebMethod]
    public string Init() {
        NewCart x = new NewCart();
        x.style = null;
        x.data = new List<Products.Stock>();
        return JsonConvert.SerializeObject(x, Formatting.Indented);
    }

    [WebMethod]
    public string GroupingCart(List<Products.Stock> cart, Products.ProductData product) {
        try {
            List<NewCart> xx = new List<NewCart>();
            Price pr = new Price();
            foreach (Products.Stock c in cart) {
                if (xx.Any(a => a.style == c.style)) {
                    foreach (NewCart x in xx) {
                        if (c.style == x.style) {
                            if (x.data.Any(a => a.sku == c.sku)) {
                                foreach (Products.Stock d in x.data) {
                                    if (d.sku == c.sku) {
                                        d.quantity = d.quantity + c.quantity;
                                        d.myprice = pr.GetPrice(product.category_code, product.brand_code, c.style, d.price );
                                    }
                                }
                            } else {
                                x.data.Add(c);
                            }
                        }
                    }
                } else {
                    NewCart x = new NewCart();
                    x.style = c.style;
                    x.data.Add(c);
                    xx.Add(x);
                }
            }
            return JsonConvert.SerializeObject(xx, Formatting.Indented);
        } catch (Exception e) {
            return e.Message;
        }
    }
    #endregion WebMethods

}
