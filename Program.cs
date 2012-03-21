/// Here is Ted...
/// Ted doesn't do much in an office...
/// He is nonetheless very useful...

using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Ted
{
    class Program
    {
        static void Main(string[] args)
        {
            bool unregisterMode = false;
            bool debugMode = false;
            List<string> dllList = new List<string>();

            if (args.Length == 0)
            {
                Console.Error.WriteLine("Ted has nothing to do. Press any key.");
                Console.ReadLine();
                Environment.Exit(1);
            }

            // Make sure all file exists.
            foreach (string dll in args)
            {
                // This is the unregister mode switch.
                if (dll.Equals("-u"))
                {
                    unregisterMode = true;
                    continue;
                }
                // This is the debug mode switch.
                if (dll.Equals("-d"))
                {
                    debugMode = true;
                    continue;
                }

                // Make sure each file we are going to register exists.
                if (File.Exists(dll)) dllList.Add(dll);
                else
                {
                    Console.Error.WriteLine("Error: File " + dll + " does not exists. Press any key.");
                    Console.ReadLine();
                    Environment.Exit(1);
                }
            }

            // Iterate over all DLLs and register each of them.
            foreach (string dll in dllList) 
            {
                try
                {
                    string dllFullPath = Path.GetFullPath(dll);
                    Assembly assemblyDll = Assembly.LoadFile(dllFullPath);
                    RegistrationServices regSrv = new RegistrationServices();

                    if (!unregisterMode)
                        regSrv.RegisterAssembly(assemblyDll, AssemblyRegistrationFlags.SetCodeBase);
                    else
                        regSrv.UnregisterAssembly(assemblyDll);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error registering " + dll + ".");
                    if (debugMode) Console.Error.WriteLine(ex);
                }
            }
        }
    }
}
