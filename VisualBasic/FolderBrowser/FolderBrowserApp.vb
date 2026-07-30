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
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports Laserfiche.RepositoryAccess

Namespace Laserfiche.Samples
    Class EntryRow
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal value As String)
                m_Name = value
            End Set
        End Property
        Private m_Name As String
        Public Property Type() As String
            Get
                Return m_Type
            End Get
            Set(ByVal value As String)
                m_Type = value
            End Set
        End Property
        Private m_Type As String
        Public Property CreatedBy() As String
            Get
                Return m_CreatedBy
            End Get
            Set(ByVal value As String)
                m_CreatedBy = value
            End Set
        End Property
        Private m_CreatedBy As String
        Public Property CreationDate() As String
            Get
                Return m_CreationDate
            End Get
            Set(ByVal value As String)
                m_CreationDate = value
            End Set
        End Property
        Private m_CreationDate As String
        Public Property LastModified() As String
            Get
                Return m_LastModified
            End Get
            Set(ByVal value As String)
                m_LastModified = value
            End Set
        End Property
        Private m_LastModified As String
        Public Property Template() As String
            Get
                Return m_Template
            End Get
            Set(ByVal value As String)
                m_Template = value
            End Set
        End Property
        Private m_Template As String
    End Class

    Class FolderBrowserApp
        <STAThread()> _
        Shared Sub Main()
            ' repository login information
            Dim serverName As String = "MyLaserficheServer", repoName As String = "MyRepository"
            Dim username As String = "MyUserName", password As String = "MyPassword"

            Try
                ' initialize an instance of List<EntryRow> to store the contents of the root folder
                Dim contents As New List(Of EntryRow)()

                ' log into the repository
                Dim repository As New RepositoryRegistration(serverName, repoName)
                Using session As New Session()
                    session.LogIn(username, password, repository)

                    ' get the folder to browse (the root folder)
                    Dim myFolder As FolderInfo = Folder.GetFolderInfo("\", session)

                    ' configure which columns to retrieve
                    Dim entrySetting As New EntryListingSettings()
                    entrySetting.EntryFilter = EntryTypeFilter.AllTypes
                    entrySetting.AddColumn(SystemColumn.DisplayName)
                    entrySetting.AddColumn(SystemColumn.Id)
                    entrySetting.AddColumn(SystemColumn.EntryType)
                    entrySetting.AddColumn(SystemColumn.LastModified)
                    entrySetting.AddColumn(SystemColumn.CreationDate)
                    entrySetting.AddColumn(SystemColumn.CreatorName)
                    entrySetting.AddColumn(SystemColumn.TemplateName)

                    ' get the contents of the root folder
                    Using listing As FolderListing = myFolder.OpenFolderListing(entrySetting, 1000)
                        ' the listing is 1-based, 
                        Dim rowCount As Integer = listing.RowsCount
                        For i As Integer = 1 To rowCount
                            ' construct a new row from the data in the folder listing, and place it in the list
                            Dim newRow As New EntryRow()
                            newRow.Name = listing.GetDatumAsString(i, SystemColumn.DisplayName)
                            newRow.Type = listing.GetDatumAsString(i, SystemColumn.EntryType)
                            newRow.LastModified = listing.GetDatumAsString(i, SystemColumn.LastModified)
                            newRow.CreationDate = listing.GetDatumAsString(i, SystemColumn.CreationDate)
                            newRow.CreatedBy = listing.GetDatumAsString(i, SystemColumn.CreatorName)
                            newRow.Template = listing.GetDatumAsString(i, SystemColumn.TemplateName)
                            contents.Add(newRow)
                        Next
                    End Using

                    ' log out of the repository
                    session.LogOut()
                End Using

                ' show the contents of the root folder
                Application.EnableVisualStyles()
                Application.SetCompatibleTextRenderingDefault(False)
                Dim folderViewer As New ListContentViewer()
                folderViewer.Content = contents
                folderViewer.Text = "Folder Content Viewer"
                Application.Run(folderViewer)
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