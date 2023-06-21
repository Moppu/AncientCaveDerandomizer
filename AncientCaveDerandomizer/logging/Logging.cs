using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using AncientCaveDerandomizer.config;

namespace AncientCaveDerandomizer.logging
{
    // Combined utility class and logging interface for sending messages around.
    public abstract class Logging
    {
        static List<Logging> allLoggers = new List<Logging>();
        static List<Logging> allDebugLoggers = new List<Logging>();
        public static List<MethodBase> debugMethods = new List<MethodBase>();
        public static void AddLogger(Logging logging)
        {
            allLoggers.Add(logging);
        }
        public static void AddDebugLogger(Logging logging)
        {
            allDebugLoggers.Add(logging);
            AddLogger(logging);
        }

        // implementation method
        public abstract void logMessage(String msg);
        public abstract void forceLogFlush();

        // log a message
        public static void log(String msg)
        {
            foreach (Logging logging in allLoggers)
            {
                logging.logMessage(msg);
            }
        }

        public static void flush()
        {
            foreach (Logging logging in allLoggers)
            {
                logging.forceLogFlush();
            }
        }

        public static bool debugEnabled = false;

        static List<string> logLevelsEncountered = new List<string>();
        // debug log if logging enabled for the calling method
        public static void logDebug(String msg, String debugType)
        {
            if (!Logging.debugEnabled)
                return;
            string debugEnabledStr = null;
            bool debugEnabled = false;
            // first see if it's cached here in the list
            if (logLevelsEncountered.Contains(debugType))
            {
                debugEnabled = true;
            }
            else
            {
                if (AppConfig.APPCONFIG != null)
                {
                    debugEnabledStr = AppConfig.APPCONFIG.getStringProperty("debugLogging." + debugType);
                }
                if (debugEnabledStr == null)
                {
                    if (debugType == "general")
                    {
                        if (AppConfig.APPCONFIG != null)
                        {
                            AppConfig.APPCONFIG.setConfigProperty("debugLogging." + debugType, "yes");
                            if (!logLevelsEncountered.Contains(debugType))
                            {
                                logLevelsEncountered.Add(debugType);
                            }
                        }
                        debugEnabled = true;
                    }
                    else
                    {
                        if (AppConfig.APPCONFIG != null)
                        {
                            AppConfig.APPCONFIG.setConfigProperty("debugLogging." + debugType, "yes");
                        }
                        debugEnabled = false;
                    }
                }
                else
                {
                    debugEnabled = debugEnabledStr == "yes";
                }
            }
            if (debugEnabled)
            {
                if (!logLevelsEncountered.Contains(debugType))
                {
                    logLevelsEncountered.Add(debugType);
                }

                foreach (Logging logging in allDebugLoggers)
                {
                    logging.logMessage("[DEBUG." + debugType + "] " + msg/* + "   [" + sf.GetMethod() + ":" + sf.GetFileLineNumber() + "]"*/);
                }
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                System.Diagnostics.StackFrame sf = st.GetFrame(1);
                if (debugMethods.Contains(sf.GetMethod()))
                {
                    log(msg);
                }
            }
        }

        // use reflection to enable debugging for the calling method
        public static void enableDebugForThisMethod()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
            System.Diagnostics.StackFrame sf = st.GetFrame(1);
            debugMethods.Add(sf.GetMethod());
        }
    }
}
