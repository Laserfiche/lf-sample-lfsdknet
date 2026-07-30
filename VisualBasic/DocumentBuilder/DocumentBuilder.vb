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

Imports System.IO

Namespace Laserfiche.Samples
    Class DocumentBuilder
        Shared Sub Main(ByVal args As String())
            ' repository login information
            Dim serverName As String = "MyLaserficheServer", repoName As String = "MyRepository"
            Dim username As String = "MyUserName", password As String = "MyPassword"
            Try
                ' log into the repository
                Dim repository As New RepositoryRegistration(serverName, repoName)
                Using session As New Session()
                    session.LogIn(username, password, repository)

                    ' Create a document under the root folder
                    Dim docInfo As New DocumentInfo(session)
                    Dim rootFolder As FolderInfo = Folder.GetRootFolder(session)
                    docInfo.Create(rootFolder, "Created By Document Builder", EntryNameOption.AutoRename)
                    ' add a page to the new document
                    docInfo.AppendPage()
                    ' import an image into the created page
                    Dim page As PageInfo = docInfo.GetPageInfo(1)
                    Using fs As FileStream = File.OpenRead("..\..\..\..\..\Resources\SAMPLE1.TIF")
                        Using writer As Stream = page.WritePagePart(PagePart.Image, fs.Length)
                            Dim b(32768) As Byte
                            While (True)
                                Dim n As Integer = fs.Read(b, 0, b.Length)
                                If (n = 0) Then
                                    Exit While
                                End If
                                writer.Write(b, 0, n)
                            End While
                        End Using
                    End Using

                    ' dispose the document
                    docInfo.Dispose()

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