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
Imports Laserfiche.RepositoryAccess.Activity

Namespace Laserfiche.Samples
    Class NotificationApp
        Shared Sub Main(ByVal args As String())
            ' repository login information
            Dim serverName As String = "MyLaserficheServer", repoName As String = "MyRepository"
            Dim username As String = "MyUserName", password As String = "MyPassword"
            Try
                ' log into the repository
                Dim repository As New RepositoryRegistration(serverName, repoName)
                Using session As New Session()
                    session.LogIn(username, password, repository)

                    ' initialize an instance of NotificationManager
                    Using notiManager As New NotificationManager(session)
                        ' establish a connection between the server and RA's NotificationManager
                        notiManager.Connect()

                        ' subscribe to events on entries that occur in other sessions
                        notiManager.Subscribe(NotificationActivities.AllEntry, NotificationSubscriptionOptions.OtherSessionsOnly)

                        ' wait up to a minute to receive a notification
                        Dim notification As Notification = notiManager.WaitForNotification(60 * 1000)

                        ' if there is a notification, print it out
                        If notification IsNot Nothing Then
                            Console.Write("ActivityType:")
                            Console.WriteLine(notification.ActivityType.ToString())
                            Console.Write("Activity Number:")
                            Console.WriteLine(notification.SequenceNumber)
                        End If
                    End Using

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
