namespace TeenWise.WebSite.Models
{
    using System;
    using System.Web.Script.Serialization;

    public static class JsonSerializer
    {
        static JavaScriptSerializer serializer;

        public static JavaScriptSerializer Instance
        {
            get
            {
                if (serializer == null)
                {
                    serializer = new JavaScriptSerializer();
                }
                return serializer;
            }
        }

        public static T Deserialize<T>(string json)
        {
            T value = Instance.Deserialize<T>(json);
            return value;
        }

        public static string Serialize(object value)
        {
            return Instance.Serialize(value);
        }
    }
}
