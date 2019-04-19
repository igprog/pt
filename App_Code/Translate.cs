using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// Translate
/// </summary>
namespace Igprog {
    public class Translate {
        public Translate() {
        }

        public string Tran(string title, string lang) {
            try {
                string path = string.Format("~/assets/json/translations/{0}/main.json", lang);
                string path1 = HttpContext.Current.Server.MapPath(path);
                if (File.Exists(HttpContext.Current.Server.MapPath(path))) {
                    string json = File.ReadAllText(HttpContext.Current.Server.MapPath(path));
                    string[] ss = Regex.Split(json, ",\r\n");
                    foreach (string s in ss) {
                        string[] _s = s.Split(':');
                        if (_s.Count() == 2) {
                            if(KeyTitle(_s) == title.ToLower()) {
                                title = Title(s);
                            }
                        }
                    }
                    return title;
                } else {
                    return title;
                }
            } catch (Exception e) { return (title); }
        }

        public string[] Translations(string lang) {
            string[] ss = null;
            string path = string.Format("~/assets/json/translations/{0}/main.json", lang);
            string path1 = HttpContext.Current.Server.MapPath(path);
            if (File.Exists(HttpContext.Current.Server.MapPath(path))) {
                string json = File.ReadAllText(HttpContext.Current.Server.MapPath(path));
                ss = Regex.Split(json, ",\r\n");
            }
            return ss;
        }

        public string KeyTitle(string[] _s) {
            return _s[0].Replace("\"", "").Replace("\r", "").Replace("\n", "").Replace("{\r\n", "").Replace("{", "").Trim().ToLower().ToString();
        }

        public string Title(string s) {
            return s.Split(':')[1].Replace("\"", "").Replace("\r\n}", "").Trim().ToString();
        }
    }

}
