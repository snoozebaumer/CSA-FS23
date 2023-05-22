using System;
using System.Diagnostics;
using System.IO;

namespace StartHttpServer {
    class Program {
        static void Main(string[] args) {
            // daten.txt im Projektverzeichnis StartHttpServer
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            if (args.Length != 0) {
                // daten.txt im angegebenen Verzeichnis
                projectDirectory = Environment.CurrentDirectory + args[0];
            }
            Console.WriteLine("Start SimpleHttpServer...");
            Console.WriteLine("Content: {0}", projectDirectory);
            Process.Start(new ProcessStartInfo() {
                FileName = "dotnet",
                Arguments = "SimpleHttpServer.dll " + "logs.txt"
            });
        }
    }
}