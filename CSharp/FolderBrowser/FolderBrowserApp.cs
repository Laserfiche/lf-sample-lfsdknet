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
using System.Windows.Forms;
using System.Collections.Generic;
using Laserfiche.RepositoryAccess;

namespace Laserfiche.Samples
{
    class EntryRow
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string CreatedBy { get; set; }
        public string CreationDate { get; set; }
        public string LastModified { get; set; }
        public string Template { get; set; }
    }

    static class FolderBrowserApp
    {
        [STAThread]
        static void Main()
        {
            // repository login information
            string serverName = "MyLaserficheServer", repoName = "MyRepository";
            string username = "MyUserName", password = "MyPassword";

            try
            {
                // initialize an instance of List<EntryRow> to store the contents of the root folder
                List<EntryRow> contents = new List<EntryRow>();
            
                // log into the repository
                RepositoryRegistration repository = new RepositoryRegistration(serverName, repoName);
                using (Session session = new Session())
                {
                    session.LogIn(username, password, repository);

                    // get the folder to browse (the root folder)
                    FolderInfo myFolder = Folder.GetFolderInfo("\\", session);

                    // configure which columns to retrieve
                    EntryListingSettings entrySetting = new EntryListingSettings();
                    entrySetting.EntryFilter = EntryTypeFilter.AllTypes;
                    entrySetting.AddColumn(SystemColumn.DisplayName);
                    entrySetting.AddColumn(SystemColumn.Id);
                    entrySetting.AddColumn(SystemColumn.EntryType);
                    entrySetting.AddColumn(SystemColumn.LastModified);
                    entrySetting.AddColumn(SystemColumn.CreationDate);
                    entrySetting.AddColumn(SystemColumn.CreatorName);
                    entrySetting.AddColumn(SystemColumn.TemplateName);

                    // get the contents of the root folder
                    using (FolderListing listing = myFolder.OpenFolderListing(entrySetting, 1000))
                    {
                        // the listing is 1-based, 
                        int rowCount = listing.RowsCount;
                        for (int i = 1; i <= rowCount; ++i)
                        {
                            // construct a new row from the data in the folder listing, and place it in the list
                            EntryRow newRow = new EntryRow();
                            newRow.Name = listing.GetDatumAsString(i, SystemColumn.DisplayName);
                            newRow.Type = listing.GetDatumAsString(i, SystemColumn.EntryType);
                            newRow.LastModified = listing.GetDatumAsString(i, SystemColumn.LastModified);
                            newRow.CreationDate = listing.GetDatumAsString(i, SystemColumn.CreationDate);
                            newRow.CreatedBy = listing.GetDatumAsString(i, SystemColumn.CreatorName);
                            newRow.Template = listing.GetDatumAsString(i, SystemColumn.TemplateName);
                            contents.Add(newRow);
                        }
                    }

                    // log out of the repository
                    session.LogOut();
                }

                // show the contents of the root folder
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ListContentViewer folderViewer = new ListContentViewer();
                folderViewer.Content = contents;
                folderViewer.Text = "Folder Content Viewer";
                Application.Run(folderViewer);
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
