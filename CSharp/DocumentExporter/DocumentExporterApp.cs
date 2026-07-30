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
using Laserfiche.DocumentServices;
using Laserfiche.RepositoryAccess;

namespace Laserfiche.Samples
{
    static class DocumentExporterApp
    {
        static void Main()
        {
            // repository login information
            string serverName = "MyLaserficheServer", repoName = "MyRepository";
            string username = "MyUserName", password = "MyPassword";
            // the document to export
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

                    // initialize an instance of DocumentExporter
                    DocumentExporter exporter = new DocumentExporter();
                    // configure the exporter to export images as JPEG, include annotations, and burn-in redactions.
                    exporter.IncludeAnnotations = true;
                    exporter.BlackoutRedactions = true;
                    exporter.PageFormat = DocumentPageFormat.Jpeg;
                    exporter.CompressionQuality = 90;

                    // export the first page of the document to C:\ on the local system
                    exporter.ExportPage(docInfo, 1, "C:\\DocumentExportSample.jpg");

                    // export all pages to a PDF file, using the above settings (include annotations, and burn-in redactions)
                    exporter.ExportPdf(docInfo, docInfo.AllPages, PdfExportOptions.IncludeText, "C:\\DocumentExportSample.pdf");

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
