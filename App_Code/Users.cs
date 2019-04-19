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
public class Users : System.Web.Services.WebService {
    string dataBase = ConfigurationManager.AppSettings["AppDataBase"];
    string EncryptionKey = ConfigurationManager.AppSettings["EncryptionKey"];
    DataBase db = new DataBase();

    public Users () {
    }
    public class NewUser {
        public string userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string companyName { get; set; }
        public string address { get; set; }
        public string postalCode { get; set; }
        public string city { get; set; }

        public Country country = new Country();
        public string pin { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string emailConfirm { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string passwordConfirm { get; set; }
        public string ipAddress { get; set; }
        public string deliveryFirstName { get; set; }
        public string deliveryLastName { get; set; }
        public string deliveryCompanyName { get; set; }
        public string deliveryAddress { get; set; }
        public string deliveryPostalCode { get; set; }
        public string deliveryCity { get; set; }

        public Country deliveryCountry = new Country();
        public string deliveryType { get; set; }
        public string paymentMethod { get; set; }

        public Orders.DiscountCoeff discount = new Orders.DiscountCoeff();

    }

    public class Country {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    #region WebMethods
    [WebMethod]
    public string Init() {
        NewUser x = new NewUser();
        x.userId = null;
        x.firstName = null;
        x.lastName = null;
        x.companyName = null;
        x.address = null;
        x.postalCode = null;
        x.city = null;
        x.country = new Country();
        x.pin = null;
        x.phone = null;
        x.email = null;
        x.emailConfirm = null;
        x.userName = null;
        x.password = null;
        x.passwordConfirm = null;
        x.ipAddress = HttpContext.Current.Request.UserHostAddress;
        x.deliveryFirstName = null;
        x.deliveryLastName = null;
        x.deliveryCompanyName = null;
        x.deliveryAddress = null;
        x.deliveryPostalCode = null;
        x.deliveryCity = null;
        x.deliveryCountry = new Country();
        x.deliveryType = null;
        x.paymentMethod = null;
        x.discount = new Orders.DiscountCoeff();

        return JsonConvert.SerializeObject(x, Formatting.Indented);
    }

    [WebMethod]
    public string Load() {
        try {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + dataBase));
            connection.Open();
            string sql = "SELECT userId, firstName, lastName, companyName, address, postalCode, city, country, pin, phone, email, userName, password, iPAddress, deliveryFirstName, deliveryLastName, deliveryCompanyName, deliveryAddress, deliveryPostalCode, deliveryCity, deliveryCountry, deliveryType, paymentMethod, countryCode, discountCoeff FROM users";
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<NewUser> xx = new List<NewUser>();
            while (reader.Read()) {
                NewUser x = new NewUser();
                x = ReadData(reader, x);
                xx.Add(x);
            }
            connection.Close();
            return JsonConvert.SerializeObject(xx, Formatting.Indented);
        } catch (Exception e) { return e.Message; }
    }

    [WebMethod]
    public string Login(string userName, string password) {
        try {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + dataBase));
            connection.Open();
            string sql = "SELECT userId, firstName, lastName, companyName, address, postalCode, city, country, pin, phone, email, userName, password, iPAddress, deliveryFirstName, deliveryLastName, deliveryCompanyName, deliveryAddress, deliveryPostalCode, deliveryCity, deliveryCountry, deliveryType, paymentMethod, countryCode, discountCoeff FROM users WHERE userName = @userName AND password = @password";
            SQLiteCommand command = new SQLiteCommand(
                  sql, connection);
            command.Parameters.Add(new SQLiteParameter("userName", userName));
            command.Parameters.Add(new SQLiteParameter("password", Encrypt(password)));
            NewUser x = new NewUser();
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                x = ReadData(reader, x);
            }
            connection.Close();
            string json = JsonConvert.SerializeObject(x, Formatting.Indented);
            return json;
        } catch (Exception e) { return e.Message; }
    }

    [WebMethod]
    public string Signup(NewUser x) {
        x.userName = x.email;
        db.CreateDataBase(dataBase, db.users);
        if (Check(x) != false) {
            throw new Exception("the email address you have entered is already registered");
        } else {
            try {
                SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + dataBase));
                x.userId = Convert.ToString(Guid.NewGuid());
                x.password = Encrypt(x.password);
                connection.Open();
                string sql = @"INSERT INTO users VALUES  
                       (@userId, @firstName, @lastName, @companyName, @address, @postalCode, @city, @country, @pin, @phone, @email, @UserName, @password, @iPAddress, @deliveryFirstName, @deliveryLastName, @deliveryCompanyName, @deliveryAddress, @deliveryPostalCode, @deliveryCity, @deliveryCountry, @deliveryType, @paymentMethod, @countryCode, @discountCoeff)";
                SQLiteCommand command = setParameters(sql, connection, x);   //new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
                connection.Close();
               return ("registration completed successfully");
            } catch (Exception e) { return (e.Message); }
        }
    }

    [WebMethod]
    public string Update(NewUser x) {
        try {
            x.password = Encrypt(x.password);
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + dataBase));
            connection.Open();
            string sql = @"UPDATE Users SET  
                             firstName = @firstName, lastName = @lastName, companyName = @companyName, address = @address, postalCode = @postalCode, city = @city, country = @country, pin = @pin, phone = @phone, email = @email, userName = @userName, password = @password, iPAddress = @iPAddress, deliveryFirstName = @deliveryFirstName, deliveryLastName = @deliveryLastName, deliveryCompanyName = @deliveryCompanyName, deliveryAddress = @deliveryAddress, deliveryPostalCode = @deliveryPostalCode, deliveryCity = @deliveryCity, deliveryCountry = @deliveryCountry, deliveryType = @deliveryType, paymentMethod = @paymentMethod,  countryCode = @countryCode, discountCoeff = @discountCoeff 
                             WHERE UserId = @UserId";
            SQLiteCommand command = setParameters(sql, connection, x);
            command.ExecuteNonQuery();
            connection.Close();
            return ("user data saved successfully");
        } catch (Exception e) { return (e.Message); }
    }

    [WebMethod]
    public string Delete(NewUser x) {
        try {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + dataBase));
            connection.Open();
            string sql = @"DELETE FROM users WHERE userId = @userId";
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.Parameters.Add(new SQLiteParameter("userId", x.userId));
            command.ExecuteNonQuery();
            connection.Close();
            return "OK";
        } catch (Exception e) { return (e.Message); }
    }

    [WebMethod]
    public string ForgotPassword(string email) {
        try {
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + dataBase));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT userId, firstName, lastName, companyName, address, postalCode, city, country, pin, phone, email, userName, password, iPAddress, deliveryFirstName, deliveryLastName, deliveryCompanyName, deliveryAddress, deliveryPostalCode, deliveryCity, deliveryCountry, deliveryType, paymentMethod, countryCode, discountCoeff FROM users WHERE email = @email", connection);
            command.Parameters.Add(new SQLiteParameter("email", email));
            NewUser x = new NewUser();
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                x = ReadData(reader, x);
            }
            connection.Close();
            if (x.userName == null) {
                throw new Exception("there is no user registered with that email address");
            }
            return JsonConvert.SerializeObject(x, Formatting.Indented);
        } catch (Exception e) { return e.Message; }
    }

    [WebMethod]
    public string GetCountries() {
        return JsonConvert.SerializeObject(GetCountriesJson(), Formatting.Indented);
    }

    [WebMethod]
    public string SaveCountries(List<Country> x) {
        return WriteJsonFile("countries", JsonConvert.SerializeObject(x, Formatting.Indented));
    }


    #endregion

    #region Methods
    protected string Encrypt(string clearText) {
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create()) {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream()) {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write)) {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    protected string Decrypt(string cipherText) {
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create()) {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream()) {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write)) {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }

    private bool Check(NewUser x) {
        try {
            bool result = false;
            SQLiteConnection connection = new SQLiteConnection("Data Source=" + Server.MapPath("~/App_Data/" + dataBase));
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(
                 "SELECT EXISTS (SELECT userId FROM users WHERE email = @email)", connection);
            command.Parameters.Add(new SQLiteParameter("email", x.email));
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                result = reader.GetBoolean(0);
            }
            connection.Close();
            return result;
        } catch (Exception e) { return false; }
    }

    private NewUser ReadData(SQLiteDataReader reader, NewUser x) {
        Orders.OrderOption orderOptions = GetOrderOptions();
        List<Country> countries = GetCountriesJson();
        x.userId = reader.GetString(0);
        x.firstName = reader.GetValue(1) == DBNull.Value ? "" : reader.GetString(1);
        x.lastName = reader.GetValue(2) == DBNull.Value ? "" : reader.GetString(2);
        x.companyName = reader.GetValue(3) == DBNull.Value ? "" : reader.GetString(3);
        x.address = reader.GetValue(4) == DBNull.Value ? "" : reader.GetString(4);
        x.postalCode = reader.GetValue(5) == DBNull.Value ? "" : reader.GetString(5);
        x.city = reader.GetValue(6) == DBNull.Value ? "" : reader.GetString(6);
        x.country.Name = reader.GetValue(7) == DBNull.Value ? "" : reader.GetString(7);
        x.country.Code = countries.Any(a => a.Name == x.country.Name) ? countries.Find(a => a.Name == x.country.Name).Code : "";
        x.pin = reader.GetValue(8) == DBNull.Value ? "" : reader.GetString(8);
        x.phone = reader.GetValue(9) == DBNull.Value ? "" : reader.GetString(9);
        x.email = reader.GetValue(10) == DBNull.Value ? "" : reader.GetString(10);
        x.userName = reader.GetValue(11) == DBNull.Value ? "" : reader.GetString(11);
        x.password = reader.GetValue(12) == DBNull.Value ? "" : Decrypt(reader.GetString(12));
        x.ipAddress = reader.GetValue(13) == DBNull.Value ? "" : reader.GetString(13);
        x.deliveryFirstName = reader.GetValue(14) == DBNull.Value ? "" : reader.GetString(14);
        x.deliveryLastName = reader.GetValue(15) == DBNull.Value ? "" : reader.GetString(15);
        x.deliveryCompanyName = reader.GetValue(16) == DBNull.Value ? "" : reader.GetString(16);
        x.deliveryAddress = reader.GetValue(17) == DBNull.Value ? "" : reader.GetString(17);
        x.deliveryPostalCode = reader.GetValue(18) == DBNull.Value ? "" : reader.GetString(18);
        x.deliveryCity = reader.GetValue(19) == DBNull.Value ? "" : reader.GetString(19);
        x.deliveryCountry.Name = reader.GetValue(20) == DBNull.Value ? "" : reader.GetString(20);
        x.deliveryType = reader.GetValue(21) == DBNull.Value ? "" : reader.GetString(21);
        x.paymentMethod = reader.GetValue(22) == DBNull.Value ? "" : reader.GetString(22);
        x.deliveryCountry.Code = reader.GetValue(23) == DBNull.Value ? "" : reader.GetString(23);
        x.discount.coeff = reader.GetValue(24) == DBNull.Value ? 0 : Convert.ToDouble(reader.GetString(24));
        x.discount.title = orderOptions.discountcoeff.Any(a => a.coeff == x.discount.coeff) ? orderOptions.discountcoeff.Find(a => a.coeff == x.discount.coeff).title: "";
        x.discount.description = orderOptions.discountcoeff.Any(a => a.coeff == x.discount.coeff) ? orderOptions.discountcoeff.Find(a => a.coeff == x.discount.coeff).description : "";
        x.discount.img = orderOptions.discountcoeff.Any(a => a.coeff == x.discount.coeff) ? orderOptions.discountcoeff.Find(a => a.coeff == x.discount.coeff).img : "";
        return x;
    }
    private SQLiteCommand setParameters(string sql, SQLiteConnection connection, NewUser x) {
        SQLiteCommand command = new SQLiteCommand(sql, connection);
        command.Parameters.Add(new SQLiteParameter("userId", x.userId));
        command.Parameters.Add(new SQLiteParameter("firstName", x.firstName));
        command.Parameters.Add(new SQLiteParameter("lastName", x.lastName));
        command.Parameters.Add(new SQLiteParameter("companyName", x.companyName));
        command.Parameters.Add(new SQLiteParameter("address", x.address));
        command.Parameters.Add(new SQLiteParameter("postalCode", x.postalCode));
        command.Parameters.Add(new SQLiteParameter("city", x.city));
        command.Parameters.Add(new SQLiteParameter("country", x.country.Name));
        command.Parameters.Add(new SQLiteParameter("pin", x.pin));
        command.Parameters.Add(new SQLiteParameter("phone", x.phone));
        command.Parameters.Add(new SQLiteParameter("email", x.email));
        command.Parameters.Add(new SQLiteParameter("userName", x.userName));
        command.Parameters.Add(new SQLiteParameter("password", x.password));
        command.Parameters.Add(new SQLiteParameter("iPAddress", x.ipAddress));
        command.Parameters.Add(new SQLiteParameter("deliveryFirstName", x.deliveryFirstName));
        command.Parameters.Add(new SQLiteParameter("deliveryLastName", x.deliveryLastName));
        command.Parameters.Add(new SQLiteParameter("deliveryCompanyName", x.deliveryCompanyName));
        command.Parameters.Add(new SQLiteParameter("deliveryAddress", x.deliveryAddress));
        command.Parameters.Add(new SQLiteParameter("deliveryPostalCode", x.deliveryPostalCode));
        command.Parameters.Add(new SQLiteParameter("deliveryCity", x.deliveryCity));
        command.Parameters.Add(new SQLiteParameter("deliveryCountry", x.deliveryCountry.Name));
        command.Parameters.Add(new SQLiteParameter("deliveryType", x.deliveryType));
        command.Parameters.Add(new SQLiteParameter("paymentMethod", x.paymentMethod));
        command.Parameters.Add(new SQLiteParameter("countryCode", x.deliveryCountry.Code));
        command.Parameters.Add(new SQLiteParameter("discountCoeff", x.discount.coeff));
        return command;
    }
    #endregion

    #region Methods
    public Orders.OrderOption GetOrderOptions() {
        try {
            string path = "~/App_Data/json/orderoptions.json";
            string json = null;
            if (File.Exists(Server.MapPath(path))) {
                json = File.ReadAllText(Server.MapPath(path));
            }
            return JsonConvert.DeserializeObject<Orders.OrderOption>(json);
        } catch (Exception e) {
            return new Orders.OrderOption();
        }
    }

    public List<Country> GetCountriesJson() {
        try {
            string path = "~/App_Data/json/countries.json";
            string json = null;
            if (File.Exists(Server.MapPath(path))) {
                json = File.ReadAllText(Server.MapPath(path));
            }
            return JsonConvert.DeserializeObject<List<Country>>(json);
        } catch (Exception e) {
            return new List<Country>();
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

    public void CreateFolder(string path) {
        if (!Directory.Exists(Server.MapPath(path))) {
            Directory.CreateDirectory(Server.MapPath(path));
        }
    }



    #endregion Methods

}
