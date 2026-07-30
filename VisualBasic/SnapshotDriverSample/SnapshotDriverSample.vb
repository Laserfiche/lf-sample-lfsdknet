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
    Class SnapshotDriverSample
        Shared Sub Main(ByVal args As String())
            ' try to load the profiles of current users, if not found, use default profile
            Dim profileName As String = String.Empty
            Try
                Dim myProfiles As SnapshotProfile() = SnapshotDriver.GetCurrentUserProfiles().ToArray()
                If myProfiles.Length = 0 Then
                    profileName = myProfiles(0).Name
                End If

                Using driver As New SnapshotDriver()
                    driver.AttachToPrinter("Laserfiche Snapshot")

                    ' specify the field values
                    Dim fvs As New FieldValueCollection()
                    fvs.Add("Author", "SnapshotDriverSample")
                    fvs.Add("Date", DateTime.Now)

                    ' assign template and fields
                    Dim metadata As New SnapshotMetadata()
                    metadata.TemplateName = "General"
                    metadata.SetFieldValues(fvs)

                    ' specify the settings for the jobs
                    Dim settings As New SnapshotDriverSettings()
                    ' process the job in batch
                    settings.Mode = SnapshotDriverWorkMode.BatchAndRepository
                    settings.DocumentName = "Snapshot Driver from SDK"
                    settings.FolderPath = "\SnapshotDriverSample"
                    settings.ProfileName = profileName
                    settings.ProfileLocation = SnapshotProfileLocation.CurrentUser
                    settings.Metadata = metadata

                    ' prepare the jobs
                    driver.PrepareBatch(settings)

                    ' print the job in the application with Laserfiche snapshot
                    Dim proc As New ProcessStartInfo
                    proc.FileName = "C:\Program Files\Laserfiche\SDK 10.0\SDKLic.rtf"
                    proc.UseShellExecute = True
                    proc.Verb = "print"

                    Process.Start(proc)

                    ' wait until finishing the printing
                    Dim result As SnapshotResult = driver.WaitForJob(New TimeSpan(0, 1, 0))

                    If result.ErrorCode <> 0 Then
                        Console.WriteLine(result.ErrorMessage)
                    Else
                        Console.WriteLine(String.Format("The document is imported into repository, its ID is {0}", result.EntryId))
                    End If

                    ' do more jobs..

                    ' finish the batch
                    driver.CompleteBatch()
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
