using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Reflection;
using System.IO;
using AncientCaveDerandomizer.logging;

namespace AncientCaveDerandomizer.config
{
    public class PropertyFileReader
    {

        public Dictionary<string, string> readByteArray(byte[] data)
        {
            try
            {
                StreamReader sr = new StreamReader(new MemoryStream(data), Encoding.Default);
                return readPropertyStream(sr);
            }
            catch (Exception e)
            {
                //LoggingServer.log(e.ToString());
                Logging.log(e.ToString());
                return new Dictionary<string, string>();
            }
        }

        public Dictionary<string, string> readFile(string filename)
        {
            try
            {
                StreamReader sr = new StreamReader(new FileStream(filename, FileMode.Open), Encoding.Default);
                return readPropertyStream(sr);
            }
            catch (Exception e)
            {
                //LoggingServer.log(e.ToString());
                Logging.log(e.ToString());
                return new Dictionary<string, string>();
            }
        }

        public Dictionary<string, string> readPropertyResourceFile(string resourceName)
        {
            // stuff
            try
            {
                Assembly assemb = Assembly.GetExecutingAssembly();
                string[] names = assemb.GetManifestResourceNames();
                //foreach (string name in names)
                //{
                    //LoggingServer.log(name);
                //}
                //if (names.Contains(resourceName))
                //{
                try
                {
                    Stream stream = assemb.GetManifestResourceStream("Someditc.Resources." + resourceName);
                    StreamReader reader = new StreamReader(stream, Encoding.Default);
                    return readPropertyStream(reader);
                }
                catch (Exception e)
                {
                    return new Dictionary<string, string>();
                }
                //}
                //LoggingServer.log("Resource not found: " + resourceName);
                //return new Dictionary<string, string>();
            }
            catch (Exception e)
            {
                Logging.log(e.ToString());
                return new Dictionary<string,string>();
            }
        }

        private Dictionary<string, string> readPropertyStream(StreamReader sr)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string line = sr.ReadLine();
            while (line != null)
            {
                line = line.Trim();
                if (!line.StartsWith("#"))
                {
                    int equals = line.IndexOf('=');
                    if (equals > 0)
                    {
                        string before = line.Substring(0, equals).Trim();
                        string after = line.Substring(equals + 1).Trim();
                        if (before.Length > 0 && after.Length > 0)
                        {
                            ret[before] = after;
                        }
                    }
                }
                line = sr.ReadLine();
            }
            sr.Close();
            //reader.BaseStream
            return ret;
        }
        protected void writePropertyFile(string filename, Dictionary<string, string> data)
        {
            //LoggingServer.logDebug("filename: " + filename);
            //foreach(string s in data.Values)
            //LoggingServer.logDebug("Writing line: " + s);
            try
            {
                FileStream fs = new FileStream(filename, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                foreach (string key in data.Keys)
                {
                    sw.WriteLine(key + "=" + data[key]);
                }
                sw.Close();
            }
            catch (Exception e)
            {
                Logging.log(e.ToString());
            }
        }
    }
}
