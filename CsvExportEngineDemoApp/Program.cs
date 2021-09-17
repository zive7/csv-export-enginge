namespace CsvExportEngineDemoApp
{
    using CsvExportEngine.Contracts;
    using CsvExportEngine.Services;
    using CsvExportEngineDemoApp.Maps;
    using CsvExportEngineDemoApp.Storage;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    class Program
    {
        private const string fileName = "test.csv";
        static async Task Main(string[] args)
        {
            PersonRepo personRepo = new PersonRepo();
            ICsvExportService csvExportService = new CsvExportService();

            byte[] content = csvExportService.GenerateCsvFile<PersonDto, PersonMap>(personRepo.GetPersons().ToArray(), x => x);

            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var fullFilePath = $"{path}\\{fileName}";

            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);
            }

            await File.WriteAllBytesAsync(fullFilePath, content);

            Console.ReadKey();
        }
    }
}
