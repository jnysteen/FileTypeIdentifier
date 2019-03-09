using System;
using System.IO;
using JNysteen.FileTypeIdentifier.FileSignatures;

namespace JNysteen.FileTypeIdentifier.ConsoleApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /***
             *    This simple console application contains an example of how the file type identifier is used.
             *    A file signature mapping is instantiated and configured to contain the signatures for a PDF file.
             *    A file type identifier is instantiated with this mapping, enabling it to recognize PDF files.
             *
             *    The input loop asks the user for a file path and, using the file type identifier, tells the user
             *    whether the inputted file was a PDF or not. 
             */

            // Instantiate a mapping of the file signatures that should be recognized
            var fileSignatureMapping = new FileSignatureMapping();

            // Add the file signatures for the PDF file type
            fileSignatureMapping.AddFileSignatures(DocumentSignatures.PDFSignatures, DocumentSignatures.PDF);
            // If more types of files should be recognized, they can be added to the file signature mapping here

            // Instantiate a new file type identifier with the configure file signature mapping
            var fileTypeIdentifier = new FileTypeIdentifier(fileSignatureMapping);

            while (true)
            {
                Console.WriteLine("Input the path to a file that should be identified: ");
                var inputFilePath = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(inputFilePath))
                {
                    Console.WriteLine("No input was provided...");
                    continue;
                }

                if (!File.Exists(inputFilePath))
                {
                    Console.WriteLine($"File at path '{inputFilePath}' does not exist!");
                    continue;
                }

                string identifiedFileType = null;

                // Open a read Stream for the provided file
                using (var fileStream = File.OpenRead(inputFilePath))
                {
                    identifiedFileType = fileTypeIdentifier.GetFileType(fileStream);
                }

                // Now, if the provided file is a PDF, it will be recognized by the file type identifier!
                if (identifiedFileType != null)
                    Console.WriteLine($"The type of this file is '{identifiedFileType}'");
                // If the file is not a PDF, the type is unknown to the identifier
                else
                    Console.WriteLine("The file type identifier was not configured to recognize this type of file!");
            }
        }
    }
}