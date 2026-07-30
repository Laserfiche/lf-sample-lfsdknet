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
using Laserfiche.RepositoryAccess;

namespace Laserfiche.Samples
{
    class VersoningApp
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

                    // create a document named 'Versioning Demo' in the root folder
                    DocumentInfo docInfo = new DocumentInfo(session);
                    FolderInfo parent = Folder.GetRootFolder(session);
                    docInfo.Create(parent, "Versioning Demo", EntryNameOption.None);
                    docInfo.Unlock();

                    docInfo.Comment = "I am revision 1.";
                    docInfo.Save();
                    // first put the document under version control
                    docInfo.PutUnderVersionControl();

                    // get the first version and add a label
                    DocumentVersion revision1 = new DocumentVersion(docInfo.Id, 1, session);
                    revision1.Refresh();
                    revision1.AddLabel("rev 1", LabelConflictStrategy.Set);

                    // check out the document so we can modify it
                    docInfo.CheckOut();

                    // append one page to the document, and write some text on it
                    PageInfo page = docInfo.AppendPage();
                    page.WriteTextPagePart("Laserfiche versioning.");

                    // store a comment
                    docInfo.Comment = "I am revision 2.";
                    docInfo.Save();

                    // check in the document to generate new a version.
                    // the current version of the document is now 2
                    docInfo.CheckIn();

                    // get the second version and add a label to it
                    DocumentVersion revision2 = new DocumentVersion(docInfo.Id, 2, session);
                    revision2.Refresh();
                    revision2.AddLabel("rev 2", LabelConflictStrategy.Set);

                    // log out of the repository
                    session.LogOut();
                }
                Console.WriteLine("Done");
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
