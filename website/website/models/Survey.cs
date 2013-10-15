namespace TeenWise.WebSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;

    public class Section
    {
        public Section() {}

        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Survey
    {
        IIdentity identity;
        string surveyName;
        int currentSectionIndex; 
        List<Section> sections;
        List<Question> currentQuestions;
        List<Answer> currentAnswers;

        public Survey(IIdentity identity)
        {
            this.identity = identity;
            this.currentSectionIndex = 0;
        }

        public Survey(IIdentity identity, int sectionIndex)
            : this(identity)
        {
            this.currentSectionIndex = sectionIndex;
        }

        public List<Answer> CurrentAnswers
        {
            get
            {
                if (this.currentAnswers == null)
                {
                    this.currentAnswers = new List<Answer>();
                    Answer[] storedAnswers = Storage.Storage.Survey.GetAnswers(this.identity, CurrentSection.ID);
                    int i = 0;
                    foreach (Question q in CurrentQuestions)
                    {
                        if (i < storedAnswers.Length && q.ID == storedAnswers[i].QuestionID)
                        {
                            this.currentAnswers.Add(storedAnswers[i++]);
                        }
                        else
                        {
                            this.currentAnswers.Add(new Answer() { QuestionID = q.ID });
                        }
                    }
                }
                return this.currentAnswers;
            }
            set
            {
                this.currentAnswers = value;
            }
        }

        public List<Question> CurrentQuestions
        {
            get
            {
                if (this.currentQuestions == null)
                {
                    this.currentQuestions = new List<Question>(Storage.Storage.Survey.GetQuestions(CurrentSection.ID));
                }
                return this.currentQuestions;
            }
        }

        public Section CurrentSection
        {
            get { return Sections[CurrentSectionIndex]; }
        }

        public int CurrentSectionIndex
        {
            get { return this.currentSectionIndex; }
            set
            {
                this.currentSectionIndex = value;
                this.currentQuestions = null;
                this.currentAnswers = null;
            }
        }

        public List<Section> Sections 
        {
            get 
            {
                if (this.sections == null)
                {
                    this.sections = new List<Section>(Storage.Storage.Survey.GetSurvey(SurveyName));
                }
                return this.sections; 
            }
        }

        public string SurveyName
        {
            get { return (this.surveyName ?? "Default"); }
            set { this.surveyName = value; }
        }

        public bool IsView(out string view)
        {
            if (CurrentSection.Name.StartsWith("view_", StringComparison.OrdinalIgnoreCase))
            {
                view = CurrentSection.Name.Split('_')[1];
                return true;
            }
            view = null;
            return false;
        }

        public static void CreateSurvey()
        {
            Section[] sections = new Section[]  { 
                new Section() { Name="User Profile" }, 
                new Section() { Name="Technology" }, 
                new Section() { Name="view_Video" }, 
                new Section() { Name="Product" }, 
                new Section() { Name="Branding & Naming" }, 
                new Section() { Name="Presentation" }, 
                new Section() { Name="Scenarios" }, 
                new Section() { Name="Features" }, 
                new Section() { Name="view_Finish" }, 
            };
            Storage.Storage.Survey.SetSurvey("Default", sections);

            CreateSurveyQuestions(Storage.Storage.Survey.GetSurvey("Default"));
        }

        static void CreateSurveyQuestions(Section[] sections)
        {
            int id = sections[0].ID;
            Question[] questions = new Question[] {
                    new Question() { SectionID=id, QuestionText="Name :", Type=QuestionType.SingleText },
                    new Question() { SectionID=id, QuestionText="Gender :", Type=QuestionType.SingleChoice, Options= new string[] { "Female", "Male" } },
                    new Question() { SectionID=id, QuestionText="Age Group :", Type=QuestionType.SingleChoice, Options= new string[] { "Under 25", "25 to 35", "35 to 45", "Over 45" } },
                    new Question() { SectionID=id, QuestionText="Number of Children :", Type=QuestionType.SingleText, OtherText="Ages :" },
                    new Question() { SectionID=id, QuestionText="What is your occupation? (check each that apply)", Type=QuestionType.MultiChoice, Options= new string[] { "Household CEO", "Professional", "Student", "Retired" } },
                };
            Storage.Storage.Survey.SetQuestions(questions);

            id = sections[1].ID;
            questions = new Question[] {
                    new Question() { SectionID=id, QuestionText="Do you have a Smartphone?", Type=QuestionType.SingleChoice, Options= new string[] { "None", "iPhone", "Android", "Windows Phone 7" }, OtherText="Other " },
                    new Question() { SectionID=id, QuestionText="Do you have a Tablet?", Type=QuestionType.SingleChoice, Options= new string[] { "None", "iPad", "Kindle Fire", "Samsung Galaxy" }, OtherText="Other " },
                    new Question() { SectionID=id, QuestionText="Do you have a Facebook account?", Type=QuestionType.SingleChoice, Options= new string[] { "Yes", "No" } },
                    new Question() { SectionID=id, QuestionText="Do you have a Twitter account?", Type=QuestionType.SingleChoice, Options= new string[] { "Yes", "No" } },
                    new Question() { SectionID=id, QuestionText="How do you access your email? (check each that apply)", Type=QuestionType.MultiChoice, Options= new string[] { "Phone", "Web browser", "Outlook", "Mac Mail" }, OtherText="Other " },
                    new Question() { SectionID=id, QuestionText="What is your primary tool for managing your schedule?", Type=QuestionType.SingleChoice, Options= new string[] { "Phone", "Computer", "Day Planner", "Wall Calendar" }, OtherText="Other " },
                    new Question() { SectionID=id, QuestionText="If you use an electronic calendar, how do you access it?   (check each that apply)", Type=QuestionType.MultiChoice, Options= new string[] { "Phone", "Web browser", "Outlook", "Mac Mail" }, OtherText="Other " },
                    new Question() { SectionID=id, QuestionText="If you use an electronic calendar, which calendar service do you use?", Type=QuestionType.SingleChoice, Options= new string[] { "Gmail", "Windows Live", "Exchange", "Local" }, OtherText="Other " },
                    new Question() { SectionID=id, QuestionText="What is your primary tool for managing lists or tasks?", Type=QuestionType.SingleChoice, Options= new string[] { "Phone", "Computer", "Day Planner", "Post-it or Paper" }, OtherText="Other " },
                    new Question() { SectionID=id, QuestionText="Do you use an application to manage lists or tasks?", Type=QuestionType.SingleChoice, Options= new string[] { "Yes", "No" }, OtherText="If yes, which one? " },
                };
            Storage.Storage.Survey.SetQuestions(questions);

            id = sections[3].ID;
            questions = new Question[] {
                    new Question() { SectionID=id, QuestionText="Would you find this product useful?", Type=QuestionType.ScaleChoice, Options= new string[] { "Low", "1", "2", "3", "4", "5", "High" } },
                    new Question() { SectionID=id, QuestionText="What are the three most exciting things about this product?", Type=QuestionType.MultiPart, Options= new string[] { "3" } },
                    new Question() { SectionID=id, QuestionText="a.|Most exciting things", Type=QuestionType.MultiText, Options= new string[] { "1" } },
                    new Question() { SectionID=id, QuestionText="b.|Most exciting things", Type=QuestionType.MultiText, Options= new string[] { "1" } },
                    new Question() { SectionID=id, QuestionText="c.|Most exciting things", Type=QuestionType.MultiText, Options= new string[] { "1" } },
                    new Question() { SectionID=id, QuestionText="If this product was available today, what would compel you to start using it?", Type=QuestionType.MultiText, Options= new string[] { "3" } },
                    new Question() { SectionID=id, QuestionText="What would make you reluctant to use this product?", Type=QuestionType.MultiText, Options= new string[] { "3" } },
                    new Question() { SectionID=id, QuestionText="Do you have any other feedback or suggestions for the product?", Type=QuestionType.MultiText, Options= new string[] { "3" } },
                };
            Storage.Storage.Survey.SetQuestions(questions);

            id = sections[4].ID;
            questions = new Question[] {
                    new Question() { SectionID=id, QuestionText="What do you think of 'Zaplify' as the product name?", Type=QuestionType.ScaleChoice, Options= new string[] { "Low", "1", "2", "3", "4", "5", "High" } },
                    new Question() { SectionID=id, QuestionText="What image or feeling does 'Zaplify' evoke?", Type=QuestionType.MultiText, Options= new string[] { "3" } },
                    new Question() { SectionID=id, QuestionText="Does the product name fit the product description?", Type=QuestionType.ScaleChoice, Options= new string[] { "Low", "1", "2", "3", "4", "5", "High" } },
                    new Question() { SectionID=id, QuestionText="In priority order, select your three favorites from the following product names: <div class='choices'><span>Zaplify,</span><span>MyValet,</span><span>LifeHub,</span><span>TaskStore,</span><span>ZapList,</span><span>SmartPlannr</span></div>", Type=QuestionType.MultiPart, Options= new string[] { "3" } },
                    new Question() { SectionID=id, QuestionText="a.|Favorite names", Type=QuestionType.ListChoice, Options= new string[] { "Zaplify", "MyValet", "LifeHub", "TaskStore", "ZapList", "SmartPlannr" } },
                    new Question() { SectionID=id, QuestionText="b.|Favorite names", Type=QuestionType.ListChoice, Options= new string[] { "Zaplify", "MyValet", "LifeHub", "TaskStore", "ZapList", "SmartPlannr" } },
                    new Question() { SectionID=id, QuestionText="c.|Favorite names", Type=QuestionType.ListChoice, Options= new string[] { "Zaplify", "MyValet", "LifeHub", "TaskStore", "ZapList", "SmartPlannr" } },
                    new Question() { SectionID=id, QuestionText="What do you think of the tagline 'simplify your life'?", Type=QuestionType.ScaleChoice, Options= new string[] { "Low", "1", "2", "3", "4", "5", "High" } },
                    new Question() { SectionID=id, QuestionText="Does the tagline 'simplify your life' fit the product description?", Type=QuestionType.ScaleChoice, Options= new string[] { "Low", "1", "2", "3", "4", "5", "High" } },
                    new Question() { SectionID=id, QuestionText="Do you have any other suggestions for a product name or tagline?", Type=QuestionType.MultiText, Options= new string[] { "3" } },
                };
            Storage.Storage.Survey.SetQuestions(questions);

            id = sections[5].ID;
            questions = new Question[] {
                    new Question() { SectionID=id, QuestionText="Was the presentation clear and to the point?", Type=QuestionType.ScaleChoice, Options= new string[] { "Low", "1", "2", "3", "4", "5", "High" } },
                    new Question() { SectionID=id, QuestionText="What are three points you remember from the presentation?", Type=QuestionType.MultiPart, Options= new string[] { "3" } },
                    new Question() { SectionID=id, QuestionText="a.|Presentation points", Type=QuestionType.MultiText, Options= new string[] { "1" } },
                    new Question() { SectionID=id, QuestionText="b.|Presentation points", Type=QuestionType.MultiText, Options= new string[] { "1" } },
                    new Question() { SectionID=id, QuestionText="c.|Presentation points", Type=QuestionType.MultiText, Options= new string[] { "1" } },
                    new Question() { SectionID=id, QuestionText="What questions did the presentation leave unanswered?", Type=QuestionType.MultiText, Options= new string[] { "3" } },
                    new Question() { SectionID=id, QuestionText="How can the presentation be improved?", Type=QuestionType.MultiText, Options= new string[] { "3" } },
                    new Question() { SectionID=id, QuestionText="Do you have any other feedback or comments?", Type=QuestionType.MultiText, Options= new string[] { "3" } },
                };
            Storage.Storage.Survey.SetQuestions(questions);

            id = sections[6].ID;
            questions = new Question[] {
                    new Question() { SectionID=id, QuestionText="What are the top tasks you have on your list today?", Type=QuestionType.MultiPart, Options= new string[] { "5" } },
                    new Question() { SectionID=id, QuestionText="a.|Top tasks", Type=QuestionType.MultiText, Options= new string[] { "1" } },
                    new Question() { SectionID=id, QuestionText="b.|Top tasks", Type=QuestionType.MultiText, Options= new string[] { "1" } },
                    new Question() { SectionID=id, QuestionText="c.|Top tasks", Type=QuestionType.MultiText, Options= new string[] { "1" } },
                    new Question() { SectionID=id, QuestionText="d.|Top tasks", Type=QuestionType.MultiText, Options= new string[] { "1" } },
                    new Question() { SectionID=id, QuestionText="e.|Top tasks", Type=QuestionType.MultiText, Options= new string[] { "1" } },
                    new Question() { SectionID=id, QuestionText="In priority order, select the top three scenarios you think the product could help you with.<div class='choices'><span>Soccer Practice,</span><span>Buy Groceries,</span><span>Book Club,</span><span>Mom's Birthday,</span><span>Clean Gutters</span></div>", Type=QuestionType.MultiPart, Options= new string[] { "3" } },
                    new Question() { SectionID=id, QuestionText="a.|Top scenarios", Type=QuestionType.ListChoice, Options= new string[] { "Soccer Practice", "Buy Groceries", "Book Club", "Mom's Birthday", "Clean Gutters" }, OtherText="Why?" },
                    new Question() { SectionID=id, QuestionText="b.|Top scenarios", Type=QuestionType.ListChoice, Options= new string[] { "Soccer Practice", "Buy Groceries", "Book Club", "Mom's Birthday", "Clean Gutters" }, OtherText="Why?" },
                    new Question() { SectionID=id, QuestionText="c.|Top scenarios", Type=QuestionType.ListChoice, Options= new string[] { "Soccer Practice", "Buy Groceries", "Book Club", "Mom's Birthday", "Clean Gutters" }, OtherText="Why?" },
                    new Question() { SectionID=id, QuestionText="Please list other activities you do which are similar to the <b>Soccer Practice</b> scenario.", Type=QuestionType.MultiText, Options= new string[] { "2" } },
                    new Question() { SectionID=id, QuestionText="Please list other activities you do which are similar to the <b>Buy Groceries</b> scenario.", Type=QuestionType.MultiText, Options= new string[] { "2" } },
                    new Question() { SectionID=id, QuestionText="Please list other activities you do which are similar to the <b>Book Club</b> scenario.", Type=QuestionType.MultiText, Options= new string[] { "2" } },
                    new Question() { SectionID=id, QuestionText="Please list other activities you do which are similar to the <b>Mom's Birthday</b> scenario.", Type=QuestionType.MultiText, Options= new string[] { "2" } },
                    new Question() { SectionID=id, QuestionText="Please list other activities you do which are similar to the <b>Clean Gutters</b> scenario.", Type=QuestionType.MultiText, Options= new string[] { "2" } },
                    new Question() { SectionID=id, QuestionText="Are there more appealing scenarios the product could help you with?", Type=QuestionType.MultiText, Options= new string[] { "3" } },
                };
            Storage.Storage.Survey.SetQuestions(questions);

            id = sections[7].ID;
            questions = new Question[] {
                    new Question() { SectionID=id, QuestionText="In priority order, select how you would most like to interact with the product.<div class='choices'><span>Smartphone,</span><span>Tablet,</span><span>Web browser,</span><span>Calendar,</span><span>EMail,</span><span>Facebook</span></div>", Type=QuestionType.MultiPart, Options= new string[] { "3" } },
                    new Question() { SectionID=id, QuestionText="a.|Interaction choice", Type=QuestionType.ListChoice, Options= new string[] { "Smartphone", "Tablet", "Web browser", "Email", "Calendar", "Facebook" } },
                    new Question() { SectionID=id, QuestionText="b.|Interaction choice", Type=QuestionType.ListChoice, Options= new string[] { "Smartphone", "Tablet", "Web browser", "Email", "Calendar", "Facebook" } },
                    new Question() { SectionID=id, QuestionText="c.|Interaction choice", Type=QuestionType.ListChoice, Options= new string[] { "Smartphone", "Tablet", "Web browser", "Email", "Calendar", "Facebook" } },                  
                    new Question() { SectionID=id, QuestionText="In priority order, select how you would most like to input information into the product.<div class='choices'><span>Speak into phone,</span><span>Type into phone,</span><span>Type into computer,</span><span>Import from other apps,</span><span>Send text messages</span></div>", Type=QuestionType.MultiPart, Options= new string[] { "3" } },
                    new Question() { SectionID=id, QuestionText="a.|Input choice", Type=QuestionType.ListChoice, Options= new string[] { "Speak into phone", "Type into phone", "Type into computer", "Import from other apps", "Send text messages" } },
                    new Question() { SectionID=id, QuestionText="b.|Input choice", Type=QuestionType.ListChoice, Options= new string[] { "Speak into phone", "Type into phone", "Type into computer", "Import from other apps", "Send text messages" } },
                    new Question() { SectionID=id, QuestionText="c.|Input choice", Type=QuestionType.ListChoice, Options= new string[] { "Speak into phone", "Type into phone", "Type into computer", "Import from other apps", "Send text messages" } },
                    new Question() { SectionID=id, QuestionText="Would you prefer to choose from a menu of choices OR<br/>&nbsp;&nbsp;&nbsp;&nbsp;use 'free-form' input (text or speech) which potentially asks clarifying questions?", Type=QuestionType.MultiText, Options= new string[] { "3" } },
                    new Question() { SectionID=id, QuestionText="How would you feel about entering information in a standard order? (e.g. what, when, where, ...)", Type=QuestionType.SingleChoice, Options= new string[] { "Reasonable", "Annoying", "Would not use product" } },
                    new Question() { SectionID=id, QuestionText="Is it important for the product to work with your calendar program?", Type=QuestionType.SingleChoice, Options= new string[] { "Must have", "Nice to have", "Don't care" }, OtherText="If so, which calendar?" },
                    new Question() { SectionID=id, QuestionText="Is it important to share information or collaborate with family and friends?", Type=QuestionType.SingleChoice, Options= new string[] { "Must have", "Nice to have", "Don't care" } },
                    new Question() { SectionID=id, QuestionText="Would you want the product to <b>import</b> information from your Facebook? (e.g. contacts)", Type=QuestionType.SingleChoice, Options= new string[] { "Must have", "Nice to have", "Don't care", "Never" } },
                    new Question() { SectionID=id, QuestionText="Would you want the product to <b>post</b> information to your Facebook or Twitter?", Type=QuestionType.SingleChoice, Options= new string[] { "Must have", "Nice to have", "Don't care", "Never" } },
                    new Question() { SectionID=id, QuestionText="If posting information, how would you like this to work?", Type=QuestionType.SingleChoice, Options= new string[] { "Automatically", "Must ask every time", "Never" } },
                };
            Storage.Storage.Survey.SetQuestions(questions);
        }

    }
}
