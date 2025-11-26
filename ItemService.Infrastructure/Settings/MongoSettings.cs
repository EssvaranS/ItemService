namespace ItemService.Infrastructure.Settings
{
    public class MongoSettings
    {
        public const string SectionName = "MongoSettings";
        public string ConnectionString { get; set; } = "";
        public string DatabaseName { get; set; } = "";
    }
}
