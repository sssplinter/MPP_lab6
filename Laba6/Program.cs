using System;

namespace Laba6
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var logBuffer = new LogBuffer("C:/Users/krist/OneDrive/Рабочий стол/mpp_lab6.txt", 10, 10000);
            for (var index = 1; index <= 50; index++)
            {
                logBuffer.Add(index.ToString());
            }

            logBuffer.Close();
        }
    }
}