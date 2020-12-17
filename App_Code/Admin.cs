using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using Newtonsoft.Json;

/// <summary>
/// Admin
/// </summary>
[WebService(Namespace = "https://promo-tekstil.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class Admin : System.Web.Services.WebService {
    string supervisorUserName = ConfigurationManager.AppSettings["AdminUserName"];
    string supervisorPassword = ConfigurationManager.AppSettings["AdminPassword"];
    Files f = new Files();
    string companyinfo = "companyinfo";
    public Admin() {
    }


    public class CompanyInfo {
        public string company;
        public string companylong;
        public string companyfooter;
        public string address;
        public string zipCode;
        public string city;
        public string country;
        public string idnumber;
        public string pin;
        public string iban;
        public string iban1;
        public string swift;
        public string bank;
        public string bankshort;
        public string email;
        public string phone;
        public string operatorcode;
        public string operatorname;
        public string web;
    }

    [WebMethod]
    public bool Login(string username, string password) {
        if(username == supervisorUserName && password == supervisorPassword) {
            return true;
        } else {
            return false;
        }
    }

    [WebMethod]
    public string GetCompanyInfo() {
        CompanyInfo x = new CompanyInfo();
        string json = f.GetFile("json", companyinfo);
        if (json != null) {
            x = JsonConvert.DeserializeObject<CompanyInfo>(json);
        } else {
            x = InitCompanyInfo();
            f.SaveJsonToFile("json", companyinfo, JsonConvert.SerializeObject(x, Formatting.None));
        }
        return JsonConvert.SerializeObject(x, Formatting.None);
    }

    [WebMethod]
    public string SaveCompanyInfo(CompanyInfo x) {
        return f.SaveJsonToFile("json", companyinfo, JsonConvert.SerializeObject(x, Formatting.None));
    }

    private CompanyInfo InitCompanyInfo() {
        CompanyInfo x = new CompanyInfo();
        x.company = null;
        x.companylong = null;
        x.companyfooter = null;
        x.address = null;
        x.zipCode = null;
        x.city = null;
        x.country = null;
        x.idnumber = null;
        x.pin = null;
        x.iban = null;
        x.iban1 = null;
        x.swift = null;
        x.bank = null;
        x.bankshort = null;
        x.email = null;
        x.phone = null;
        x.operatorcode = null;
        x.operatorname = null;
        x.web = null;
        return x;
    }

    public CompanyInfo GetCompanyInfoData() {
        CompanyInfo x = new CompanyInfo();
        string json = f.GetFile("json", companyinfo);
        if (json != null) {
            x = JsonConvert.DeserializeObject<CompanyInfo>(json);
        }
        return x;
    }

}
