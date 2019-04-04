using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.IO;


namespace Pipeclient
{
   public class Client
    {
        private string message = string.Empty;
        public void Run(string pipeName)
        {
            using(NamedPipeClientStream client = new NamedPipeClientStream(pipeName))
            {
                Console.WriteLine( "forsøger at forbinde...");
                client.Connect();
                Console.WriteLine("\nForbundet bro!");
                using(StreamWriter writer = new StreamWriter(client))
                {
                    writer.AutoFlush = true;
                    Console.WriteLine("*** tast exit når du vil stoppe***");
                    do
                    {
                        Console.WriteLine("Indtast landekode og vekselskurs");
                        message = Console.ReadLine();
                        writer.WriteLine(message);
                        Console.WriteLine($"\n {message} sendt til serveren du!");
                    } while (message.ToUpper() != "EXIT");

                    
                }
            }
        }
    }
}
