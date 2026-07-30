# Laserfiche SDK 12 Sample Projects

The Laserfiche SDK ships with a set of sample programs written in C#
and VB.NET which demonstrate the use of various facilities provided by
the Laserfiche RepositoryAccess and DocumentServices .NET libraries.
C# programs can be found under the CSSamples solution, and the VB.NET
sample programs can be found under VBSamples.  All of the Visual Studio
solution and project files are targeted at Visual Studio 2022 and .NET
Framework 4.8.

The sample programs are intended to serve as a starting point and
resource for learning how to use these libraries and for developing
your own programs that use these libraries.  The sample programs are
not intended for production use or for incorporation into other
products or programs and they have not received formal testing.
Laserfiche does not offer official technical support for the provided
sample programs, but questions, comments and suggestions concerning
the samples may be posted on Laserfiche Answers.

Not all projects may be available in both C# and VB.NET.  If a project
is available in both languages, the functionality and behavior may
differ slightly between the two implementations, although an effort
has been made to make the applications as similar as possible across
languages.

List of sample programs:

* DocumentBuilder: Creates a document in Laserfiche and imports data
into it using settings specified in an XML file.

* DocumentExporter: A graphical application which uses
DocumentServices.DocumentExporter to export data stored in a
Laserfiche document to files on the client computer.

* FolderBrowser: A graphical folder browsing application which
demonstrates the use of the RepositoryAccess.FolderListing and related
classes.

* ImportUtil: A graphical utility which processes
DocumentServices.ImportEngine XML files.

* MetadataPrinter: A console application which will print metadata
about a Laserfiche entry with a specified path.

* Notification: A graphical application which will connect to a
Laserfiche repository and subscribe to and display notifications.

* OCREngineSample: A small program which shows how to use
DocumentServices to OCR a document.

* SearchClient: A graphical application which allows users to submit
search queries and to display the results.

* SnapshotDriverSample: A program which communicates with Laserfiche
Snapshot to specify the settings that will be used with the next
print job, and then opens a document that the user is expected to print.

* Versioning: A program which illustrates how to create a document in
Laserfiche, put it under version control, and to create multiple versions
of it.

* VolumeMaker: A program which demonstrates the use of
DocumentServices.VolumeMaker to create an attachable Laserfiche volume
by writing the contents of a Laserfiche folder to disk on the client
machine as an attachable Laserfiche volume.
