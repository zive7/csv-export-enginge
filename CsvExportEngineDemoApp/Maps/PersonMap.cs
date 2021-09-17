namespace CsvExportEngineDemoApp.Maps
{
    using CsvExportEngine.Contracts;
    using CsvExportEngine.Maps;

    public class PersonMap : ClassMap<PersonDto>
    {
        public PersonMap()
        {
            Map(m => m.Name).WithHeaderTranslation("Name").WithIndex(0);
            Map(m => m.Age).WithHeaderTranslation("Age").WithIndex(1);
        }
    }
}