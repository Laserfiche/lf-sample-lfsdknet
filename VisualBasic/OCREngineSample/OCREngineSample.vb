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

Imports System.Collections.Generic
Imports System.Text
Imports Laserfiche.RepositoryAccess
Imports Laserfiche.DocumentServices

Namespace Laserfiche.Samples
    Class OCREngineSample
        Shared Sub Main(ByVal args As String())
            ' repository login information
            Dim serverName As String = "MyLaserficheServer", repoName As String = "MyRepository"
            Dim username As String = "MyUserName", password As String = "MyPassword"
            ' the document to OCR
            Dim docPath As String = "\SAMPLE1"

            Try
                ' log into the repository
                Dim repository As New RepositoryRegistration(serverName, repoName)
                Using session As New Session()
                    session.LogIn(username, password, repository)

                    ' retrieve some basic properties of the document
                    Dim docInfo As DocumentInfo = Document.GetDocumentInfo(docPath, session)

                    ' lock the document
                    docInfo.Lock(LockType.Exclusive)

                    ' initialize an instance of OcrEngine
                    Using ocr As OcrEngine = OcrEngine.LoadEngine()
                        ' configure OCR options
                        ocr.AutoOrient = True
                        ocr.Decolumnize = True
                        ocr.OptimizationMode = OcrOptimizationMode.Accuracy

                        ' Generate text for all pages of the given document and import them into the document
                        ocr.Run(docInfo)
                    End Using

                    ' unlock the document
                    docInfo.Unlock()

                    ' log out of the repository
                    session.LogOut()
                End Using
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
