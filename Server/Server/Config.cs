using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Server
{


    class Config
    {

        public static SortedDictionary<string, string> configSettings;

        public static void LoadSettings()
        {

            configSettings = new SortedDictionary<string, string>();
            string configFilePath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Config.settings";
            if (!File.Exists(configFilePath)) return;

            string[] settings = File.ReadAllLines(configFilePath);
            foreach(string setting in settings)
            {
                string[] s = setting.Split('=');
                configSettings[s[0]] = s[1];
            }

        }

    }

}
