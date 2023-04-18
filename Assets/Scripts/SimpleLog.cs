using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Assets.Scripts
{
    public class SimpleLog
    {
        private static readonly string filePath = "C:\\temp\\SuperBlaster.log";
        static SimpleLog()
        {
            Log("\n\n--- new run ---\n");
        }

        public static void Log(string message)
        {
            message = $"{DateTime.Now.ToLongTimeString()}:{message}\n";
            //File.AppendAllText(filePath, message);
        }
    }
}
