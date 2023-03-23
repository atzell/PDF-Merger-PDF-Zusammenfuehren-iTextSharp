using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Do you want to create a new folder or use an existing one? (n/e)");
        string createNewFolder = Console.ReadLine().ToLower();

        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        if (createNewFolder == "n")
        {
            Console.WriteLine("\nEnter the name of the folder:");
            string folderName = Console.ReadLine();
            Console.WriteLine("\nEnter the path for the new folder:");
            folderPath = Console.ReadLine();
            folderPath = Path.Combine(folderPath, folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            else
            {
                Console.WriteLine("\nA folder with this name already exists.");
                Console.WriteLine("Please enter a different name or use an existing folder.");
                return;
            }
        }
        else if (createNewFolder == "e")
        {
            Console.WriteLine("\nEnter the path of the existing folder:");
            folderPath = Console.ReadLine();
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("\nThe specified folder does not exist.");
                Console.WriteLine("Please enter a valid folder path.");
                return;
            }
        }
        else
        {
            Console.WriteLine("\nInvalid input. The program will exit.");
            return;
        }

        string destFolderPath = folderPath;

        Console.WriteLine("\nEnter the name of the PDF file to be created/merged:");
        string mergedFileName = Console.ReadLine() + ".pdf";

        string destFilePath = Path.Combine(destFolderPath, mergedFileName);

        using (FileStream destStream = new FileStream(destFilePath, FileMode.Create))
        {
            Document document = new Document();
            PdfCopy pdfCopyProvider = new PdfCopy(document, destStream);
            document.Open();

            Console.WriteLine("\nEnter the number of PDF files to be merged:");
            int numFiles;

            while (!int.TryParse(Console.ReadLine(), out numFiles) || numFiles < 2)
            {
                Console.WriteLine("At least two PDF files must be specified for merging.");
                Console.WriteLine("Please enter the number of PDF files again:");
            }

            for (int fileIndex = 1; fileIndex <= numFiles; fileIndex++)
            {
                Console.WriteLine($"\nEnter the path to the {fileIndex}. PDF file:");
                string sourceFilePath = Console.ReadLine();

                while (!File.Exists(sourceFilePath))
                {
                    Console.WriteLine($"The PDF file '{sourceFilePath}' does not exist.");
                    Console.WriteLine($"\nEnter the path to the {fileIndex}. PDF file again:");
                    sourceFilePath = Console.ReadLine();
                }

                PdfReader reader = new PdfReader(sourceFilePath);

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfImportedPage page = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(page);
                }

                reader.Close();
            }

            document.Close();
        }

        Console.WriteLine("The PDF files have been successfully merged.\n");
        Console.WriteLine("\nThe process has finished. Press any key to close the window.");
        Console.ReadKey();
    }
}