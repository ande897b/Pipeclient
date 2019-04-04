using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Pipes;

namespace PipeServer
{
    class Server
    {
        private StreamReader pipeReader;
        private bool running = true;
        private string input = string.Empty;
        private Dictionary<string, Currency> currencies = new Dictionary<string, Currency>();

        public void Run(string pipeName)
        {
            Console.WriteLine("venter på klient");
            using (NamedPipeServerStream server = new NamedPipeServerStream(pipeName))
            {
                //wait for connection
                server.WaitForConnection();
                Console.WriteLine("   ** Klient forbundet");
                using (pipeReader = new StreamReader(server))
                {
                    while (running)
                    {
                        Console.WriteLine("venter på indput");
                        input = GetLineFromClient(pipeReader);
                        running = ManageInput(input);
                    }
                }
            }
        }
        private string GetLineFromClient(StreamReader reader)
        {
            string result = string.Empty;
            while (reader.Peek() >=0)
            {
                result += reader.ReadLine();
            }
            reader.DiscardBufferedData();
            return result.ToUpper();
        }
        private bool ManageInput(string input)
        {
            Console.WriteLine("\nInput var " + input);
           string[] test= input.Split(' ');
            try
            {
                Currency currency = new Currency(test[0], int.Parse(test[1]));
                currencies.Add(test[0], currency);
                ShowCurrencies();
                return !(input.ToUpper().Equals("EXIT"));
            }
            catch (Exception)
            {

                Console.WriteLine("fejl i input, indtast som følgende 'DKK 100'");
                
               
            }
            return true;
        }
        private void ShowCurrencies()
        {
            Console.WriteLine("\nValutakurser: ");
            foreach (Currency c in currencies.Values)
            {
                Console.WriteLine($"{c.CountryCode}, {c.ExchangeRate}");
            }
        }



    }
}
