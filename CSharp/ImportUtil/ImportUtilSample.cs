/*
   Copyright (c) Laserfiche.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.Threading;
using Laserfiche.RepositoryAccess;
using Laserfiche.DocumentServices;

namespace Laserfiche.Samples
{
    class ImportUtilSample
    {
        static void Main(string[] args)
        {
            // repository login information
            string serverName = "MyLaserficheServer", repoName = "MyRepository";
            string username = "MyUserName", password = "MyPassword";

            /* You can find seven examples of XML list files to process at:
             * "..\\List File Examples\\Briefcase Example.xml";
             * "..\\List File Examples\\Electronic Document Example.xml";
             * "..\\List File Examples\\Folders Example.xml";
             * "..\\List File Examples\\Image and Text Example.xml";
             * "..\\List File Examples\\Image Example.xml";
             * "..\\List File Examples\\Multiple Electronic Document Example.xml";
             * "..\\List File Examples\\Template Population Example.xml"; */
            string listFilePath = "My.xml";

            try
            {
                // log into the repository
                RepositoryRegistration repository = new RepositoryRegistration(serverName, repoName);
                using (Session session = new Session())
                {
                    session.LogIn(username, password, repository);

                    // initialize an instance of ImportEngine
                    ImportEngine importEngine = new ImportEngine(session);
                    // configure import engine, the folder and volume to import
                    importEngine.RootPath = "\\";
                    importEngine.VolumeName = "DEFAULT";
                    importEngine.IgnoreErrorAndContinue = true;

                    // begin importing
                    ImportOperation importOp = importEngine.BeginProcess(listFilePath);
                    // wait util the operation is complete, whether or not it is successful
                    while (!importOp.IsCompleted)
                    {
                        // print out the elapsed time and the current import phase
                        Console.WriteLine(importOp.ElapsedTime.ToString() + ":" + importOp.Phase.ToString());
                        // wait for 1 second, and then refresh the status
                        Thread.Sleep(1000);
                        importOp.Refresh();
                    }

                    // if there were any errors when importing, print them
                    if (importOp.HasFailed)
                        Console.WriteLine("Failure Reason:" + importOp.FailureReason.Message);
                    else if (importOp.AllLoggedExceptions != null && importOp.AllLoggedExceptions.Count > 0)
                    {
                        Console.WriteLine("Warning:");
                        foreach (ImportEngineException e in importOp.AllLoggedExceptions)
                            Console.WriteLine(e.Message);
                    }
                    else
                        Console.WriteLine("Success!");

                    // log out of the repository
                    session.LogOut();
                }
                Console.WriteLine("Done!");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }

            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Hit enter to exit.");
            Console.ReadLine();
        }
    }
}
