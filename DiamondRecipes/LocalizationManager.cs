using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Xml;

namespace DiamondRecipes
{
    class LocalizationManager
    {
        private string current_loc = "eng";

        private LocalizationManager()
        {
            current_loc = CultureInfo.InstalledUICulture.ThreeLetterISOLanguageName;
            //current_loc = CultureInfo.CurrentCulture.ThreeLetterISOLanguageName;
            if (current_loc != "ron")
                current_loc = "eng";
        }
        private static LocalizationManager instance;

        public static LocalizationManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new LocalizationManager();
                return instance;
            }

        }

        public string getStringForKey(string key)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("loc\\" + current_loc + ".xml");

            return doc.GetElementsByTagName(key)[0].InnerText;
        }
    }
}
