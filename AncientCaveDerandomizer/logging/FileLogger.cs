using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AncientCaveDerandomizer.logging
{
    public class FileLogger : Logging
    {
        string logFilename;
        StreamWriter fileWriter;

        public FileLogger(string logFilename)
        {
            this.logFilename = logFilename;
            fileWriter = new StreamWriter(logFilename, false);
        }

        public override void logMessage(String msg)
        {
            String msg2 = "";
            DateTime d = DateTime.Now;
            string h = d.Hour.ToString();
            if (h.Length == 1)
                h = "0" + h;
            string m = d.Minute.ToString();
            if (m.Length == 1)
                m = "0" + m;
            string s = d.Second.ToString();
            if (s.Length == 1)
                s = "0" + s;
            string dt = h + ":" + m + ":" + s;
            msg2 = dt + " - " + msg;

            fileWriter.WriteLine(msg2);
            fileWriter.Flush();
            fileWriter.BaseStream.Flush();
        }

        public override void forceLogFlush()
        {
        }
    }
}
