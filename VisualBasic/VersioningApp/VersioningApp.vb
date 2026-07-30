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

Imports Laserfiche.RepositoryAccess

Namespace Laserfiche.Samples
    Class VersoningApp
        Shared Sub Main(ByVal args As String())
            ' repository login information
            Dim serverName As String = "MyLaserficheServer", repoName As String = "MyRepository"
            Dim username As String = "MyUserName", password As String = "MyPassword"

            Try
                ' log into the repository
                Dim repository As New RepositoryRegistration(serverName, repoName)
                Using session As New Session()
                    session.LogIn(username, password, repository)

                    ' create a document named 'Versioning Demo' in the root folder
                    Dim docInfo As New DocumentInfo(session)
                    Dim parent As FolderInfo = Folder.GetRootFolder(session)
                    docInfo.Create(parent, "Versioning Demo", EntryNameOption.None)
                    docInfo.Unlock()

                    docInfo.Comment = "I am revision 1."
                    docInfo.Save()
                    ' first put the document under version control
                    docInfo.PutUnderVersionControl()

                    ' get the first version and add a label
                    Dim revision1 As New DocumentVersion(docInfo.Id, 1, session)
                    revision1.Refresh()
                    revision1.AddLabel("rev 1", LabelConflictStrategy.[Set])

                    ' check out the document so we can modify it
                    docInfo.CheckOut()

                    ' append one page to the document, and write some text on it
                    Dim page As PageInfo = docInfo.AppendPage()
                    page.WriteTextPagePart("Laserfiche versioning.")

                    ' store a comment
                    docInfo.Comment = "I am revision 2."
                    docInfo.Save()

                    ' check in the document to generate new a version.
                    ' the current version of the document is now 2
                    docInfo.CheckIn()

                    ' get the second version and add a label to it
                    Dim revision2 As New DocumentVersion(docInfo.Id, 2, session)
                    revision2.Refresh()
                    revision2.AddLabel("rev 2", LabelConflictStrategy.[Set])

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
