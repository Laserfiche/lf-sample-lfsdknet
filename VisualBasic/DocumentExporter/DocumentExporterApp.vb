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

Imports System
Imports Laserfiche.DocumentServices
Imports Laserfiche.RepositoryAccess

Namespace Laserfiche.Samples
    Class DocumentExporterApp
        Shared Sub Main()
            ' repository login information
            Dim serverName As String = "MyLaserficheServer", repoName As String = "MyRepository"
            Dim username As String = "MyUserName", password As String = "MyPassword"
            ' the document to export
            Dim docPath As String = "\SAMPLE1"

            Try
                ' log into the repository
                Dim repository As New RepositoryRegistration(serverName, repoName)
                Using session As New Session()
                    session.LogIn(username, password, repository)

                    ' retrieve some basic properties of the document
                    Dim docInfo As DocumentInfo = Document.GetDocumentInfo(docPath, session)

                    ' initialize an instance of DocumentExporter
                    Dim exporter As New DocumentExporter()
                    ' configure the exporter to export images as JPEG, include annotations, and burn-in redactions.
                    exporter.IncludeAnnotations = True
                    exporter.BlackoutRedactions = True
                    exporter.PageFormat = DocumentPageFormat.Jpeg
                    exporter.CompressionQuality = 90

                    ' export the first page of the document to C:\ on the local system
                    exporter.ExportPage(docInfo, 1, "C:\DocumentExportSample.jpg")

                    ' export all pages to a PDF file, using the above settings (include annotations, and burn-in redactions)
                    exporter.ExportPdf(docInfo, docInfo.AllPages, PdfExportOptions.IncludeText,
                                    "C:\DocumentExportSample.pdf")

                    ' log out of the repository
                    session.LogOut()
                End Using
                Console.WriteLine("Done!")
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
