using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using Newtonsoft.Json;
using Igprog;

/// <summary>
/// Categories
/// </summary>
[WebService(Namespace = "https://promo-tekstil.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class Categories : System.Web.Services.WebService {
    string categoriesFile = "categories";
    public Categories() {
    }

    public class SelectedCategories {
        public string code { get; set; }
        public bool isselected { get; set; }
        public int order { get; set; }
    }

    [WebMethod]
    public string Get() {
        return JsonConvert.SerializeObject(GetCategories());
    }

    [WebMethod]
    public string Save(List<SelectedCategories> x) {
        return WriteJsonFile(categoriesFile, JsonConvert.SerializeObject(x, Formatting.None));
    }

    public List<SelectedCategories> GetCategories() {
        try {
            List<SelectedCategories> sc = new List<SelectedCategories>();
            string path = string.Format("~/App_Data/json/{0}.json", categoriesFile);
            string json = null;
            if (File.Exists(Server.MapPath(path))) {
                json = File.ReadAllText(Server.MapPath(path));
            }
            return JsonConvert.DeserializeObject<List<SelectedCategories>>(json).ToList();
        } catch (Exception e) {
            return null;
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







}
