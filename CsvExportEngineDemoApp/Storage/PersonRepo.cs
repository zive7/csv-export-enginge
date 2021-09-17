namespace CsvExportEngineDemoApp.Storage
{
    using CsvExportEngine.Contracts;
    using System.Collections.Generic;

    public class PersonRepo
    {

        public IReadOnlyList<PersonDto> GetPersons()
        {
            return new List<PersonDto>()
            {
                new PersonDto
                {
                    Name = "Tom Tailor",
                    Age = 15
                },
                new PersonDto
                {
                    Name = "Jimmy Bob",
                    Age = 22
                },
                new PersonDto
                {
                    Name = "Foo Test Asp",
                    Age = 12
                }
            };
        }
    }
}
