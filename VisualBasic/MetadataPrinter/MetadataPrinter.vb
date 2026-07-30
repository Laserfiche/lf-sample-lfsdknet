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

Imports System.Text
Imports Laserfiche.RepositoryAccess

Namespace Laserfiche.Samples
    Class MetadataPrinter
        Shared Sub Main(ByVal args As String())
            ' repository login information
            Dim serverName As String = "MyLaserficheServer", repoName As String = "MyRepository"
            Dim username As String = "MyUserName", password As String = "MyPassword"
            ' the document to print metadata from
            Dim docPath As String = "\SAMPLE1"

            Try
                ' log into the repository
                Dim repository As New RepositoryRegistration(serverName, repoName)
                Using session As New Session()
                    session.LogIn(username, password, repository)

                    ' retrieve some basic properties of the document
                    Dim docInfo As DocumentInfo = Document.GetDocumentInfo(docPath, session)
                    ' get the field values of the document
                    Dim fvs As FieldValueCollection = docInfo.GetFieldValues()

                    ' print all the field names and their values
                    For i As Integer = 0 To fvs.Count - 1
                        Dim fieldName As String = fvs.PositionToName(i)
                        Dim fieldVal As Object = fvs(i)
                        Dim f As FieldInfo = Field.GetInfo(fieldName, session)
                        Dim formattedVal = f.ValueToString(fieldVal)
                        Console.WriteLine(fieldName + ":")
                        Console.WriteLine(formattedVal)
                        Console.WriteLine(Environment.NewLine)
                    Next

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
