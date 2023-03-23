using iTextSharp.text;
using iTextSharp.text.pdf;


class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Möchten Sie einen neuen Ordner erstellen oder einen vorhandenen verwenden? (n/v)");
        string createNewFolder = Console.ReadLine().ToLower();

     
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        if (createNewFolder == "n")
        {
            Console.WriteLine("\nGib den Namen des Ordners ein:");
            string folderName = Console.ReadLine();
            Console.WriteLine("\nGib den Pfad für den neuen Ordner ein:");
            folderPath = Console.ReadLine();
            folderPath = Path.Combine(folderPath, folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            else
            {
                Console.WriteLine("\nEin Ordner mit diesem Namen existiert bereits.");
                Console.WriteLine("Bitte geben Sie einen anderen Namen ein oder verwenden Sie einen vorhandenen Ordner.");
                return;
            }
        }
        else if (createNewFolder == "v")
        {
            Console.WriteLine("\nGeben Sie den Pfad des vorhandenen Ordners ein:");
            folderPath = Console.ReadLine();
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("\nDer angegebene Ordner existiert nicht.");
                Console.WriteLine("Bitte geben Sie einen gültigen Ordnerpfad ein.");
                return;
            }
        }
        else
        {
            Console.WriteLine("\nUngültige Eingabe. Das Programm wird beendet.");
            return;
        }

        string destFolderPath = folderPath;

        Console.WriteLine("\nGib den Namen der zu erstellenden/zusammengefassten PDF-Datei ein:");
        string mergedFileName = Console.ReadLine() + ".pdf";

        string destFilePath = Path.Combine(destFolderPath, mergedFileName);

        using (FileStream destStream = new FileStream(destFilePath, FileMode.Create))
        {
            Document document = new Document();
            PdfCopy pdfCopyProvider = new PdfCopy(document, destStream);
            document.Open();

            Console.WriteLine("\nGib die Anzahl der PDF-Dateien ein, die zusammengeführt werden sollen:");
            int numFiles;

            while (!int.TryParse(Console.ReadLine(), out numFiles) || numFiles < 2)
            {
                Console.WriteLine("Es müssen mindestens zwei PDF-Dateien zum Zusammenführen angegeben werden.");
                Console.WriteLine("Gib die Anzahl der PDF-Dateien erneut ein:");
            }

            for (int fileIndex = 1; fileIndex <= numFiles; fileIndex++)
            {
                Console.WriteLine($"\nGib den Pfad zur {fileIndex}. PDF-Datei ein:");
                string sourceFilePath = Console.ReadLine();

                while (!File.Exists(sourceFilePath))
                {
                    Console.WriteLine($"Die PDF-Datei '{sourceFilePath}' existiert nicht.");
                    Console.WriteLine($"\nGib den Pfad zur {fileIndex}. PDF-Datei erneut ein:");
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

        Console.WriteLine("Die PDF-Dateien wurden erfolgreich zusammengeführt.\n");
        Console.WriteLine("\nDer Vorgang wurde abgeschlossen. Drücken Sie eine beliebige Taste, um das Fenster zu schließen.");
        Console.ReadKey();
    }
}
//© atzell all rights reserved
