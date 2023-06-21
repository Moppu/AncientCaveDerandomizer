using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AncientCaveDerandomizer.logging;

namespace AncientCaveDerandomizer.config
{
    // this should probably move to %APPDATA% with the other config item from Form1
    // i think derando isn't actually using it though, and only mana rando uses this thing
    public class AppConfig : PropertyFileReader
    {
        private string filename;
        public static AppConfig APPCONFIG;
        static AppConfig()
        {
            APPCONFIG = new AppConfig();
        }
        public AppConfig()
        {
            this.filename = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "/config.properties"; 
            loadConfigFile();
        }
        public AppConfig(string filename)
        {
            this.filename = filename;
            loadConfigFile();
        }
        Dictionary<string, string> defaultProperties = new Dictionary<string, string>();
        Dictionary<string, string> configProperties = new Dictionary<string, string>();

        public void addDefaultProperty(string key, string value)
        {
            Logging.logDebug("Adding default property: " + key + " -> " + value, "general");
            // -> defaultProperties
            defaultProperties[key] = value;
        }

        private void loadConfigFile()
        {
            Logging.logDebug("Loading config file", "general");
            if (!File.Exists(filename))
            {
                Logging.logDebug("Creating blank config file", "general");
                StreamWriter sw = File.CreateText(filename);
                sw.Close();
            }
            Logging.logDebug("Processing config file", "general");
            configProperties = readFile(filename);
            // -> configProperties
        }

        public void setConfigProperty(string key, string value)
        {
            configProperties[key] = value;
            writePropertyFile(filename, configProperties);
            // -> configProperties
            // -> file
        }
        public string getStringProperty(string key)
        {
            string p = null;
            try
            {
                p = configProperties[key];
            } catch(KeyNotFoundException e) 
            {
                try
                {
                    p = defaultProperties[key];
                }
                catch (KeyNotFoundException ee)
                {
                    p = null;
                }
            }
            return p;
        }

        public int getIntProperty(string key)
        {
            string p = getStringProperty(key);
            if (p != null)
            {
                return Int32.Parse(p);
            }
            return 0;
        }
    }
}
