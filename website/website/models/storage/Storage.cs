namespace TeenWise.WebSite.Models.Storage
{
    using System.Configuration;
    
    public static class Storage
    {
        public static UserStorage Users = CreateUserStorage();
        public static SurveyStorage Survey = CreateSurveyStorage();

        static UserStorage CreateUserStorage()
        {
            return new SqlUserStorage(ConfiguredConnectionString);
        }

        static SurveyStorage CreateSurveyStorage()
        {
            return new SqlSurveyStorage(ConfiguredConnectionString);
        }

        static string ConfiguredConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["CorpWebDatabase"].ConnectionString;
            }
        }

    }
}