namespace api.Database
{
    public class ApiDataBaseSetttings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CollectionName { get; set; } = null!;
        public string UserTokenCollectionName {get; set;} =null!;

        public string UserMessageCollection {get; set;} = null!;
    }
}