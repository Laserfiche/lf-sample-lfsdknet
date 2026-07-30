' Copyright 2016 Compulink Management Center, Inc.
'
' Licensed under the Apache License, Version 2.0 (the "License");
' you may not use this file except in compliance with the License.
' You may obtain a copy of the License at
'
'    http://www.apache.org/licenses/LICENSE-2.0
'
' Unless required by applicable law or agreed to in writing, software
' distributed under the License is distributed on an "AS IS" BASIS,
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' See the License for the specific language governing permissions and
' limitations under the License.

Imports Laserfiche.DocumentServices
Imports Laserfiche.RepositoryAccess

Namespace Laserfiche.Samples
    Class VolumeMakerSample
        Shared Sub Main(ByVal args As String())
            Try
                Dim volMaker As New VolumeMaker("C:\VolumeMakerSample", "MyVolume")

                ' prepare to write entry data to the volume
                volMaker.StartEntries()

                ' create a subfolder under the root folder
                Dim subFolderProps As New CommonEntryProperties()
                subFolderProps.Color = Nothing
                subFolderProps.Comment = "Folder created by VolumeMaker"
                subFolderProps.CreationTimeUtc = DateTime.Now
                subFolderProps.Creator = Nothing
                subFolderProps.LastModifiedTimeUtc = DateTime.Now
                subFolderProps.Name = "MyFolder"
                volMaker.StartFolder(subFolderProps)

                ' create a document
                Dim lfDocProps As New CommonEntryProperties()
                lfDocProps.Color = Nothing
                lfDocProps.Comment = "Document created by VolumeMaker"
                lfDocProps.CreationTimeUtc = DateTime.Now
                lfDocProps.Creator = Nothing
                lfDocProps.LastModifiedTimeUtc = DateTime.Now
                lfDocProps.Name = "Laserfiche Document Sample"
                volMaker.StartDocument(lfDocProps)

                ' Set up page attributes
                ' these are from sample images. It is the same for all three pages.
                Dim pageAttr As New PageAttributes()
                pageAttr.AnsiEncoding = False
                pageAttr.HasImage = True
                pageAttr.HasText = False
                pageAttr.HasThumbnail = False
                pageAttr.HasWordLocations = False
                pageAttr.ImageDepth = 1
                pageAttr.ImageHeight = 3300
                pageAttr.ImageWidth = 2560
                pageAttr.ImageXResolution = 300
                pageAttr.ImageYResolution = 300
                pageAttr.Rotation = PageRotation.None

                ' add the three sample images as pages to the above document
                Dim srcPages As String() = {
                    "..\..\..\..\..\Resources\SAMPLE1.TIF",
                    "..\..\..\..\..\Resources\SAMPLE2.TIF",
                    "..\..\..\..\..\Resources\SAMPLE3.TIF"}
                For Each p As String In srcPages
                    volMaker.StartPage(pageAttr)
                    volMaker.AddPagePart(PagePart.Image, p)
                    volMaker.EndPage()
                Next
                ' we're finished with the document
                volMaker.EndDocument()
                ' we're finished with the folder
                volMaker.EndFolder()

                ' the last step, finalize writing out the volume
                volMaker.EndEntries()
                Console.WriteLine("Done")
            Catch Ex As Exception
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine(Ex.Message)
            End Try

            Console.ResetColor()
            Console.WriteLine()
            Console.WriteLine("Hit enter to exit.")
            Console.ReadLine()
        End Sub
    End Class
End Namespace
