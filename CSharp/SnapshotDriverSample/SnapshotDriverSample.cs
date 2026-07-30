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
using System.Diagnostics;
using System.Linq;
using Laserfiche.DocumentServices;
using Laserfiche.RepositoryAccess;

namespace Laserfiche.Samples
{
    class SnapshotDriverSample
    {
        static void Main(string[] args)
        {
            // try to load the profiles of current users, if not found, use default profile
            string profileName = string.Empty;
            try
            {
                SnapshotProfile[] myProfiles = SnapshotDriver.GetCurrentUserProfiles().ToArray();
                if (myProfiles.Length == 0)
                    profileName = "Default profile";

                using (SnapshotDriver driver = new SnapshotDriver())
                {
                    driver.AttachToPrinter("Laserfiche Snapshot");

                    // specify the field values
                    FieldValueCollection fvs = new FieldValueCollection();
                    fvs.Add("Author", "SnapshotDriverSample");
                    fvs.Add("Date", DateTime.Now);

                    // assign template and fields
                    SnapshotMetadata metadata = new SnapshotMetadata();
                    metadata.TemplateName = "General";
                    metadata.SetFieldValues(fvs);

                    // specify the settings for the jobs
                    SnapshotDriverSettings settings = new SnapshotDriverSettings();
                    // process the job in batch
                    settings.Mode = SnapshotDriverWorkMode.BatchAndRepository;
                    settings.DocumentName = "Snapshot Driver from SDK";
                    settings.FolderPath = "\\SnapshotDriverSample";
                    settings.ProfileName = profileName;
                    settings.ProfileLocation = SnapshotProfileLocation.CurrentUser;
                    settings.Metadata = metadata;

                    // prepare the jobs
                    driver.PrepareBatch(settings);

                    // print the job in the application with Laserfiche snapshot
                    ProcessStartInfo proc = new ProcessStartInfo();
                    proc.FileName = @"C:\Program Files\Laserfiche\SDK\SDKLic.rtf";
                    proc.UseShellExecute = true;
                    proc.Verb = "print";
                    Process.Start(proc);

                    // wait until finishing the printing
                    SnapshotResult result = driver.WaitForJob(new TimeSpan(0, 1, 0));

                    if (result.ErrorCode != 0)
                        Console.WriteLine(result.ErrorMessage);
                    else
                        Console.WriteLine(string.Format("The document is imported into repository, its ID is {0}.", result.EntryId));

                    // do more jobs..

                    // finish the batch
                    driver.CompleteBatch();
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
