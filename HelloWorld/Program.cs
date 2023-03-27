using System;

namespace HelloWorld
{
    public class Program
    {
        public static void PrintMessage(string msg, bool toUpper = false)
        {
            Console.WriteLine(toUpper? msg.ToUpper() : msg);
        }

        public static void Main(string[] args)
        {
            PrintMessage("Hello, World!", true);
        }
    }
}
