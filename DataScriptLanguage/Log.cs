using System;
using NLogger;

namespace DataScriptLanguage
{
    internal static class Log
    {
        private static Logger CoreLogger;

        public static void Init()
        {
            NLog.SetPattern("%^[%T][%n]:%$ %v");
            CoreLogger = new Logger("DSL");
            CoreLogger.SetLevel(NLog.Level.Fatal);
        }

        public static Logger GetCoreLogger()
        {
            if (CoreLogger == null)
                Init();
            return CoreLogger;
        }
    }
}
