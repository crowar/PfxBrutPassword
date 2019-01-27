using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.IO;

namespace PfxBrutPassword
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Download password lists from https://github.com/danielmiessler/SecLists.git and set path");
            var passwordListPath = Console.ReadLine();

            if (!Directory.Exists(passwordListPath))
            {
                return;
            }

            Console.WriteLine("Enter PFX file path");
            var pfxFilePath = Console.ReadLine();

            if (!File.Exists(pfxFilePath))
            {
                return;
            }
            
            Parallel.ForEach(Directory.EnumerateFiles(passwordListPath, "*.txt"), (file, state) =>
            {
                Console.WriteLine($"===> {file}");

                using (StreamReader reader = new StreamReader(file))
                {
                    var needExit = false;

                    while (!reader.EndOfStream || !needExit)
                    {
                        var pfxPassword = reader.ReadLine();

                        try
                        {
                            var cert = new X509Certificate2(pfxFilePath, pfxPassword);

                            Console.WriteLine($" ===>>> Password : {pfxPassword} <<<===");
                            needExit = true;
                        }
                        catch
                        {
                        }

                        if (needExit) {
                            Console.WriteLine("=> Exit");

                            state.Break();
                        }
                    }
                    
                }
            });


            Console.ReadLine();

        }
    }
}
