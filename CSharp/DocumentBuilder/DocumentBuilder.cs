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
using System.IO;
using Laserfiche.RepositoryAccess;

namespace Laserfiche.Samples
{
    class DocumentBuilder
    {
        static void Main(string[] args)
        {
            // repository login information
            string serverName = "MyLaserficheServer", repoName = "MyRepository";
            string username = "MyUserName", password = "MyPassword";
            try
            {
                // log into the repository
                RepositoryRegistration repository = new RepositoryRegistration(serverName, repoName);
                using (Session session = new Session())
                {
                    session.LogIn(username, password, repository);
                    // Create a document under the root folder
                    DocumentInfo docInfo = new DocumentInfo(session);
                    FolderInfo rootFolder = Folder.GetRootFolder(session);
                    docInfo.Create(rootFolder, "Created By Document Builder", EntryNameOption.AutoRename);
                    // add a page to the new document
                    docInfo.AppendPage();
                    // import an image into the created page
                    PageInfo page = docInfo.GetPageInfo(1);

                    using (FileStream file = File.OpenRead("..\\..\\..\\..\\..\\Resources\\SAMPLE1.TIF"))
                    {
                        using (Stream writer = page.WritePagePart(PagePart.Image, (int)file.Length))
                        {
                            byte[] buffer = new byte[0x8000];
                            int count = 0;
                            while ((count = file.Read(buffer, 0, buffer.Length)) > 0)
                                writer.Write(buffer, 0, count);
                        }
                    }

                    // dispose the document
                    docInfo.Dispose();

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
