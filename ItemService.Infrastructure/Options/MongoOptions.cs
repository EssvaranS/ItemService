namespace ItemService.Infrastructure.Options

{
    public class MongoOptions
    {
        public const string SectionName = "MongoSettings";
        public string ConnectionString { get; set; } = "";
        public string DatabaseName { get; set; } = "";
    }
}
