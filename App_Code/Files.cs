using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using Newtonsoft.Json;

/// <summary>
/// Save data to files
/// </summary>
[WebService(Namespace = "http://igprog.hr/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class Files : System.Web.Services.WebService {
    public Files() {
    }

    #region WebMethods
    [WebMethod]
    public string SaveJsonToFile(string foldername, string filename, string json) {
        try {
            string path = "~/App_Data/" + foldername;
            string filepath = path + "/" +  filename + ".json";
            CreateFolder(Server.MapPath(path));
            WriteFile(filepath, json);
            return GetFile(foldername, filename);
        } catch(Exception e) { return ("Error: " + e); }
    }

    [WebMethod]
    public string GetFile(string foldername, string filename) {
        try {
            string path = "~/App_Data/" + foldername;
            string filepath = path + "/" + filename + ".json";
            if (File.Exists(Server.MapPath(filepath))) {
                return File.ReadAllText(Server.MapPath(filepath));
            } else {
                return null;
            }
        } catch (Exception e) { return ("Error: " + e); }
    }
    #endregion WebMethods

    #region Methods
    public void CreateFolder(string path) {
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
    }

    protected void WriteFile(string path, string value) {
        File.WriteAllText(Server.MapPath(path), value);
    }

     public void DeleteFolder(string path) {
        if (Directory.Exists(path)) {
            Directory.Delete(path, true);
        }
    }
    #endregion Methods




}
