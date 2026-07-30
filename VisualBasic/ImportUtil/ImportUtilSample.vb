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

Imports System.Threading
Imports Laserfiche.RepositoryAccess
Imports Laserfiche.DocumentServices

Namespace Laserfiche.Samples
    Class ImportUtilSample
        Shared Sub Main(ByVal args As String())
            ' repository login information
            Dim serverName As String = "MyLaserficheServer", repoName As String = "MyRepository"
            Dim username As String = "MyUserName", password As String = "MyPassword"

            ' You can find seven examples of XML list files to process at:
            ' "..\\List File Examples\\Briefcase Example.xml";
            ' "..\\List File Examples\\Electronic Document Example.xml";
            ' "..\\List File Examples\\Folders Example.xml";
            ' "..\\List File Examples\\Image and Text Example.xml";
            ' "..\\List File Examples\\Image Example.xml";
            ' "..\\List File Examples\\Multiple Electronic Document Example.xml";
            ' "..\\List File Examples\\Template Population Example.xml"; 

            Dim listFilePath As String = "My Import List File"

            Try
                ' log into the repository
                Dim repository As New RepositoryRegistration(serverName, repoName)
                Using session As New Session()
                    session.LogIn(username, password, repository)

                    ' initialize an instance of ImportEngine
                    Dim importEngine As New ImportEngine(session)
                    ' configure import engine, the folder and volume to import
                    importEngine.RootPath = "\"
                    importEngine.VolumeName = "DEFAULT"
                    importEngine.IgnoreErrorAndContinue = True

                    ' begin importing
                    Dim importOp As ImportOperation = importEngine.BeginProcess(listFilePath)
                    ' wait util the operation is complete, whether or not it is successful
                    While Not importOp.IsCompleted
                        ' print out the elapsed time and the current import phase
                        Console.WriteLine(importOp.ElapsedTime.ToString() & ":" & importOp.Phase.ToString())
                        ' wait for 1 second, and then refresh the status
                        Thread.Sleep(1000)
                        importOp.Refresh()
                    End While

                    ' if there were any errors when importing, print them
                    If importOp.HasFailed Then
                        Console.WriteLine("Failure Reason:" + importOp.FailureReason.Message)
                    ElseIf importOp.AllLoggedExceptions.Count > 0 Then
                        For Each e As ImportEngineException In importOp.AllLoggedExceptions
                            Console.WriteLine(e.Message)
                        Next
                    Else
                        Console.WriteLine("Success!")
                    End If

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
