using System;
using System.IO;

namespace Assets.Scripts
{
    public class SimpleLog
    {
        public static bool IsEnabled = false;
        private static readonly string filePath = "C:\\temp\\SuperBlaster.log";

        static SimpleLog()
        {
            Log("\n\n--- new run ---\n");
        }

        public static void Log(string message)
        {
            if (!IsEnabled) return;
            message = $"{DateTime.Now.ToLongTimeString()}:{message}\n";
            File.AppendAllText(filePath, message);
        }
    }
}
