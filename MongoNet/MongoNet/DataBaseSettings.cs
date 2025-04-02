namespace MongoNet
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public const string ConfigSection = "DatabaseSettings";

    }
}
