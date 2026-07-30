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
    class VolumeMakerSample
    {
        static void Main(string[] args)
        {
            try
            {
                VolumeMaker volMaker = new VolumeMaker(@"C:\VolumeMakerSample", "MyVolume");

                // prepare to write entry data to the volume
                volMaker.StartEntries();

                // create a subfolder under the root folder
                CommonEntryProperties subFolderProps = new CommonEntryProperties()
                {
                    Color = null,
                    Comment = "Folder created by VolumeMaker",
                    CreationTimeUtc = DateTime.Now,
                    Creator = null,
                    LastModifiedTimeUtc = DateTime.Now,
                    Name = "MyFolder",
                };
                volMaker.StartFolder(subFolderProps);

                // create a document
                CommonEntryProperties lfDocProps = new CommonEntryProperties()
                {
                    Color = null,
                    Comment = "Document created by VolumeMaker",
                    CreationTimeUtc = DateTime.Now,
                    Creator = null,
                    LastModifiedTimeUtc = DateTime.Now,
                    Name = "Laserfiche Document Sample"
                };
                volMaker.StartDocument(lfDocProps);

                // Set up page attributes
                // these are from sample images. It is the same for all three pages.
                PageAttributes pageAttr = new PageAttributes()
                {
                    AnsiEncoding = false,
                    HasImage = true,
                    HasText = false,
                    HasThumbnail = false,
                    HasWordLocations = false,
                    ImageDepth = 1,
                    ImageHeight = 3300,
                    ImageWidth = 2560,
                    ImageXResolution = 300,
                    ImageYResolution = 300,
                    Rotation = PageRotation.None
                };

                // add the three sample images as pages to the above document
                string[] srcPages =
                {
                "..\\..\\..\\..\\..\\Resources\\SAMPLE1.TIF",
                "..\\..\\..\\..\\..\\Resources\\SAMPLE2.TIF",
                "..\\..\\..\\..\\..\\Resources\\SAMPLE3.TIF"
            };
                foreach (string p in srcPages)
                {
                    volMaker.StartPage(pageAttr);
                    volMaker.AddPagePart(PagePart.Image, p);
                    volMaker.EndPage();
                }
                // we're finished with the document
                volMaker.EndDocument();
                // we're finished with the folder
                volMaker.EndFolder();

                // the last step, finalize writing out the volume
                volMaker.EndEntries();
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
