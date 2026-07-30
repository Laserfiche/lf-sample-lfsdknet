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
using System.Threading;
using System.Windows.Forms;
using Laserfiche.RepositoryAccess;

namespace Laserfiche.Samples
{
    class SearchResultRow
    {
        public string Name { get; set; }
        public string HitCount { get; set; }
        public string CreationDate { get; set; }
        public string LastModified { get; set; }
        public string Type { get; set; }
        public string PageCount { get; set; }
    }

    class SearchClientApp
    {
        static void Main(string[] args)
        {
            // repository login information
            string serverName = "MyLaserficheServer", repoName = "MyRepository";
            string username = "MyUserName", password = "MyPassword";
            try
            {
                // initialize an instance of List<SearchResultRow> to store the search results
                List<SearchResultRow> contents = new List<SearchResultRow>();

                // log into the repository
                RepositoryRegistration repository = new RepositoryRegistration(serverName, repoName);
                using (Session session = new Session())
                {
                    session.LogIn(username, password, repository);

                    // initialize an instance of the Search class
                    using (Search search = new Search(session))
                    {
                        // specifiy the search query
                        search.Command = "(\"MyTerm\" | {[]:[]=\"*MyTerm*\"} | {[]:[]=\"MyTerm\"})";
                        // wait until the search completes on the server
                        LongOperation longOp = search.BeginRun(false);
                        while (!longOp.IsCompleted)
                        {
                            Thread.Sleep(1000);
                            search.UpdateStatus();
                        }

                        // specify the settings to use when retrieving the search results,
                        // such as the entry type filter and columns.
                        SearchListingSettings searchSetting = new SearchListingSettings();
                        searchSetting.EntryFilter = EntryTypeFilter.AllTypes;
                        searchSetting.AddColumn(SystemColumn.Id);
                        searchSetting.AddColumn(SystemColumn.Name);
                        searchSetting.AddColumn(SystemColumn.HitCount);
                        searchSetting.AddColumn(SystemColumn.CreationDate);
                        searchSetting.AddColumn(SystemColumn.LastModified);
                        searchSetting.AddColumn(SystemColumn.EntryType);
                        searchSetting.AddColumn(SystemColumn.PageCount);

                        // get the search result listing and iterate through the rows
                        using (SearchResultListing searchResultListing = search.GetResultListing(searchSetting))
                        {
                            int num = searchResultListing.RowsCount;
                            // the listing is 1-based; we get the first 10 results and store them in a buffer.
                            for (int i = 1; i <= num; ++i)
                            {
                                // get the values formatted as strings; store the row
                                SearchResultRow newRow = new SearchResultRow();
                                newRow.Name = searchResultListing.GetDatumAsString(i, SystemColumn.Name);
                                newRow.HitCount = searchResultListing.GetDatumAsString(i, SystemColumn.HitCount);
                                newRow.CreationDate = searchResultListing.GetDatumAsString(i, SystemColumn.CreationDate);
                                newRow.LastModified = searchResultListing.GetDatumAsString(i, SystemColumn.LastModified);
                                newRow.Type = searchResultListing.GetDatumAsString(i, SystemColumn.EntryType);
                                newRow.PageCount = searchResultListing.GetDatumAsString(i, SystemColumn.PageCount);

                                contents.Add(newRow);
                            }
                        }
                    }

                    // log out of the repository
                    session.LogOut();
                }

                // show the search results
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ListContentViewer searchResultViewer = new ListContentViewer();
                searchResultViewer.Content = contents;
                searchResultViewer.Text = "Search Result Viewer";
                Application.Run(searchResultViewer);                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error occured!",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            }
        }
    }
}
