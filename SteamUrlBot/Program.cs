using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SteamUrlBot
{
    class Program
    {
        private static bool DoesProfileExist(string content)
        {
            if (content.Contains("The specified profile could not be found"))
                return false;

            return true;
        }

        public static void Run()
        {
            Console.WriteLine("What string do you want to start from?");

            string startfrom = Console.ReadLine();

            string alphabet = "abcdefghijklmnopqrstuvwxyz1234567890";
            var q = alphabet.Select(x => x.ToString());

            int size = 3;

            for (int i = 0; i < size - 1; i++)
                q = q.SelectMany(x => alphabet, (x, y) => x + y);

            while (q.First() != startfrom)
                q = q.Where(x => x.ToString() != q.First()).ToList();

            Console.WriteLine("\n\n\n");

            if (!File.Exists("allurls.txt"))
                File.Create("allurls.txt");

            if (!File.Exists("urls.txt"))
                File.Create("urls.txt");

            Thread.Sleep(100);

            foreach (var item in q)
            {
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://steamcommunity.com/id/" + item);
                HttpWebResponse myres = (HttpWebResponse)myReq.GetResponse();

                Console.WriteLine(item);

                StreamWriter allurls = new StreamWriter("allurls.txt", true);

                StreamReader sr = new StreamReader(myres.GetResponseStream());

                string content = sr.ReadToEnd();

                sr.Close();

                if (DoesProfileExist(content))
                {
                    StreamWriter urls = new StreamWriter("urls.txt", true);
                    urls.WriteLine(item);
                    urls.Close();

                    Console.WriteLine("FOUND: " + item + "\n");
                }

                else
                    allurls.WriteLine(item);

                allurls.Close();
            }
        }


        static Thread thread_run = new Thread(new ThreadStart(Run));

        static void Main(string[] args)
        {
            Console.WriteLine("Press Any Key To Start...");
            Console.ReadKey();

            thread_run.Start();
        }
    }
}
