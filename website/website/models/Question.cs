namespace TeenWise.WebSite.Models
{
    public enum QuestionType
    {
        SingleText = 0,
        MultiText = 1,
        SingleChoice = 2,
        MultiChoice = 3,
        ScaleChoice = 4,
        ListChoice = 5,
        MultiPart = 6,
    }

    public class Question
    {
        public Question()
        { }

        public long ID { get; set; }
        public int SectionID { get; set; }
        public string QuestionText { get; set; }
        public QuestionType Type { get; set; }
        public string[] Options { get; set; }
        public string OtherText { get; set; }

        public string DisplayText()
        {   // strip commentary from multipart questions
            return QuestionText.Split('|')[0];
        }

        public string OptionsAsString()
        {
            if (Options != null && Options.Length > 0)
            {
                string s = string.Empty;
                foreach (string o in Options)
                {
                    s += o + ",";
                }
                return s.TrimEnd(',');
            }
            return null;
        }

        public static string[] ConvertToArray(string options)
        {
            return (options != null) ?  options.Split(',') : null;
        }
    }
}
