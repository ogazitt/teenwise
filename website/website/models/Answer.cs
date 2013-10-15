namespace TeenWise.WebSite.Models
{
    public class Answer
    {
        string answer;
        string other;

        public Answer()
        {
            this.answer = string.Empty;
            this.other = string.Empty;
        }

        public long ID { get; set; }
        public long QuestionID { get; set; }

        public string AnswerText {
            get { return this.answer; }
            set { this.answer = value ?? string.Empty; }
        }

        public string OtherText
        {
            get { return this.other; }
            set { this.other = value ?? string.Empty; }
        }
    }

}
