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
Imports System.Threading
Imports System.Windows.Forms
Imports Laserfiche.RepositoryAccess

Namespace Laserfiche.Samples
    Class SearchResultRow
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal value As String)
                m_Name = value
            End Set
        End Property
        Private m_Name As String
        Public Property HitCount() As String
            Get
                Return m_HitCount
            End Get
            Set(ByVal value As String)
                m_HitCount = value
            End Set
        End Property
        Private m_HitCount As String
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
        Public Property Type() As String
            Get
                Return m_Type
            End Get
            Set(ByVal value As String)
                m_Type = value
            End Set
        End Property
        Private m_Type As String
        Public Property PageCount() As String
            Get
                Return m_PageCount
            End Get
            Set(ByVal value As String)
                m_PageCount = value
            End Set
        End Property
        Private m_PageCount As String
    End Class

    Class SearchClientApp
        Shared Sub Main(ByVal args As String())
            ' repository login information
            Dim serverName As String = "MyLaserficheServer", repoName As String = "MyRepository"
            Dim username As String = "MyUserName", password As String = "MyPassword"
            Try
                ' initialize an instance of List<SearchResultRow> to store the search results
                Dim contents As New List(Of SearchResultRow)()

                ' log into the repository
                Dim repository As New RepositoryRegistration(serverName, repoName)
                Using session As New Session()
                    session.LogIn(username, password, repository)

                    ' initialize an instance of the Search class
                    Using search As New Search(session)
                        ' specifiy the search query
                        search.Command = "(""MyTerm"" | {[]:[]=""*MyTerm*""} | {[]:[]=""MyTerm""})"
                        ' wait until the search completes on the server
                        Dim longOp As LongOperation = search.BeginRun(False)
                        While Not longOp.IsCompleted
                            Thread.Sleep(1000)
                            search.UpdateStatus()
                        End While

                        ' specify the settings to use when retrieving the search results,
                        ' such as the entry type filter and columns.
                        Dim searchSetting As New SearchListingSettings()
                        searchSetting.EntryFilter = EntryTypeFilter.AllTypes
                        searchSetting.AddColumn(SystemColumn.Id)
                        searchSetting.AddColumn(SystemColumn.Name)
                        searchSetting.AddColumn(SystemColumn.HitCount)
                        searchSetting.AddColumn(SystemColumn.CreationDate)
                        searchSetting.AddColumn(SystemColumn.LastModified)
                        searchSetting.AddColumn(SystemColumn.EntryType)
                        searchSetting.AddColumn(SystemColumn.PageCount)

                        ' get the search result listing and iterate through the rows
                        Using searchResultListing As SearchResultListing = search.GetResultListing(searchSetting)
                            Dim num As Integer = searchResultListing.RowsCount
                            ' the listing is 1-based; we get the first 10 results and store them in a buffer.
                            For i As Integer = 1 To num
                                ' get the values formatted as strings; store the row
                                Dim newRow As New SearchResultRow()
                                newRow.Name = searchResultListing.GetDatumAsString(i, SystemColumn.Name)
                                newRow.HitCount = searchResultListing.GetDatumAsString(i, SystemColumn.HitCount)
                                newRow.CreationDate = searchResultListing.GetDatumAsString(i, SystemColumn.CreationDate)
                                newRow.LastModified = searchResultListing.GetDatumAsString(i, SystemColumn.LastModified)
                                newRow.Type = searchResultListing.GetDatumAsString(i, SystemColumn.EntryType)
                                newRow.PageCount = searchResultListing.GetDatumAsString(i, SystemColumn.PageCount)

                                contents.Add(newRow)
                            Next
                        End Using
                    End Using

                    ' log out of the repository
                    session.LogOut()
                End Using

                ' show the search results
                Application.EnableVisualStyles()
                Application.SetCompatibleTextRenderingDefault(False)
                Dim searchResultViewer As New ListContentViewer()
                searchResultViewer.Content = contents
                searchResultViewer.Text = "Search Result Viewer"
                Application.Run(searchResultViewer)
            Catch Ex As Exception
                MessageBox.Show(Ex.Message, "error occured!", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk)
            End Try
        End Sub
    End Class
End Namespace
