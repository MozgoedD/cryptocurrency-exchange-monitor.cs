using System;
using System.IO;

namespace CryptoMonitorCore
{
    public class LogWriters
    {
        public static void WriteResponce(string exchName, string Responce)
        {
            string message = $"{DateTime.Now} {exchName}: {Responce} \n";

            File.AppendAllText("Responce.log", message);
        }
    }
}
