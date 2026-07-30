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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Laserfiche.RepositoryAccess;
using Laserfiche.DocumentServices;

namespace Laserfiche.Samples
{
    class OCREngineSample
    {
        static void Main(string[] args)
        {
            // repository login information
            string serverName = "MyLaserficheServer", repoName = "MyRepository";
            string username = "MyUserName", password = "MyPassword";
            // the document to OCR
            string docPath = "\\SAMPLE1";

            try
            {
                // log into the repository
                RepositoryRegistration repository = new RepositoryRegistration(serverName, repoName);
                using (Session session = new Session())
                {
                    session.LogIn(username, password, repository);

                    // retrieve some basic properties of the document
                    DocumentInfo docInfo = Document.GetDocumentInfo(docPath, session);

                    // lock the document
                    docInfo.Lock(LockType.Exclusive);

                    // initialize an instance of OcrEngine
                    using (OcrEngine ocrEngine = OcrEngine.LoadEngine())
                    {
                        // configure OCR options
                        ocrEngine.AutoOrient = true;
                        ocrEngine.Decolumnize = true;
                        ocrEngine.OptimizationMode = OcrOptimizationMode.Accuracy;

                        // Generate text for all pages of the given document and import them into the document
                        ocrEngine.Run(docInfo);
                    }

                    // unlock the document
                    docInfo.Unlock();

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
