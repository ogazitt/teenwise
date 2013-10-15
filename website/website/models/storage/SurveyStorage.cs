namespace TeenWise.WebSite.Models.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlTypes;
    using System.Security.Principal;
    
    public abstract class SurveyStorage
    {
        public abstract Section[] GetSurvey(string surveyName);
        public abstract void SetSurvey(string surveyName, Section[] sections);
        public abstract Question[] GetQuestions(int sectionID);
        public abstract void SetQuestions(Question[] questions);
        public abstract Answer[] GetAnswers(IIdentity identity, int sectionID);
        public abstract void SetAnswers(IIdentity identity, Answer[] answers);
    }

    public class SqlSurveyStorage : SurveyStorage
    {
        const string SPROC_GETSURVEY = "GetSurvey";
        const string SPROC_SETSECTION = "SetSection";
        const string SPROC_GETQUESTIONS = "GetQuestions";
        const string SPROC_SETQUESTION = "SetQuestion";
        const string SPROC_GETANSWERS = "GetAnswers";
        const string SPROC_SETANSWER = "SetAnswer";

        const string COLUMN_SECTIONID = "SectionID";
        const string COLUMN_SECTIONNAME = "SectionName";
        const string COLUMN_SECTIONINDEX = "SectionIndex";
        const string COLUMN_ID = "ID";
        const string COLUMN_INDEX = "Index";
        const string COLUMN_QUESTIONID = "QuestionID";
        const string COLUMN_TYPE = "Type";
        const string COLUMN_QUESTIONTEXT = "QuestionText";
        const string COLUMN_ANSWERTEXT = "AnswerText";
        const string COLUMN_OTHERTEXT = "OtherText";
        const string COLUMN_OPTIONS = "Options";

        const string PARAM_SURVEYNAME = "SurveyName";
        const string PARAM_SECTIONID = "SectionID";
        const string PARAM_EMAILADDRESS = "EmailAddress";

        string connection;
        const int retryCount = 3;

        public SqlSurveyStorage(string connection)
        {
            this.connection = connection;
        }

        public override Section[] GetSurvey(string surveyName)
        {
            return SqlStorageCommand.Execute<Section[]>(this.connection, SPROC_GETSURVEY,
                CommandType.StoredProcedure, (SqlStorageCommand command) =>
                {
                    List<Section> sections = new List<Section>();
                    command.AddParameter(PARAM_SURVEYNAME, surveyName);
                    command.ExecuteReader();
                    while (command.TryNextRow())
                    {
                        Section section = new Section()
                        {
                            ID = command.GetColumnAsInt(COLUMN_SECTIONID),
                            Name = command.GetColumn(COLUMN_SECTIONNAME)
                        };
                        sections.Add(section);
                    }
                    return sections.ToArray();
                });                     
        }

        public override void SetSurvey(string surveyName, Section[] sections)
        {
            int i = 0;
            foreach (Section s in sections)
            {
                SqlStorageCommand.Execute(this.connection, SPROC_SETSECTION,
                    CommandType.StoredProcedure, (SqlStorageCommand command) =>
                    {
                        command.AddParameter(COLUMN_SECTIONID, s.ID);
                        command.AddParameter(PARAM_SURVEYNAME, surveyName);
                        command.AddParameter(COLUMN_SECTIONNAME, s.Name);
                        command.AddParameter(COLUMN_SECTIONINDEX, i++);
                        // TODO: add return code for errors
                        //command.AddOutputParameter(PARAM_ERRCODE, SqlDbType.Int);
                        int result = command.ExecuteNonQuery();

                        if (result < 0)
                        {
                            //int errorCode = command.GetIntParameter(PARAM_ERRORCODE);
                            throw new Exception("Error trying to store survey sections.");
                        }
                    });
            }
        }

        public override Question[] GetQuestions(int sectionID)
        {
            return SqlStorageCommand.Execute<Question[]>(this.connection, SPROC_GETQUESTIONS,
                CommandType.StoredProcedure, (SqlStorageCommand command) =>
                {
                    List<Question> questions = new List<Question>();
                    command.AddParameter(PARAM_SECTIONID, sectionID);
                    command.ExecuteReader();
                    while (command.TryNextRow())
                    {
                        Question question = new Question()
                        {
                            ID = command.GetColumnAsLong(COLUMN_ID),
                            SectionID = sectionID,
                            QuestionText = command.GetColumn(COLUMN_QUESTIONTEXT),
                            OtherText = command.GetColumn(COLUMN_OTHERTEXT),
                            Options = Question.ConvertToArray(command.GetColumn(COLUMN_OPTIONS)),
                            Type = (QuestionType)command.GetColumnAsInt(COLUMN_TYPE)
                        };
                        questions.Add(question);
                    }
                    return questions.ToArray();
                });
        }

        public override void SetQuestions(Question[] questions)
        {
            int i = 0;
            foreach (Question q in questions)
            {
                SqlStorageCommand.Execute(this.connection, SPROC_SETQUESTION,
                    CommandType.StoredProcedure, (SqlStorageCommand command) =>
                    {
                        command.AddParameter(COLUMN_ID, q.ID);
                        command.AddParameter(COLUMN_SECTIONID, q.SectionID);
                        command.AddParameter(COLUMN_INDEX, i++);
                        command.AddParameter(COLUMN_TYPE, (int)q.Type);
                        command.AddParameter(COLUMN_QUESTIONTEXT, q.QuestionText);
                        command.AddParameter(COLUMN_OTHERTEXT, q.OtherText ?? SqlString.Null);
                        command.AddParameter(COLUMN_OPTIONS, q.OptionsAsString() ?? SqlString.Null);
                        // TODO: add return code for errors
                        //command.AddOutputParameter(PARAM_ERRCODE, SqlDbType.Int);
                        int result = command.ExecuteNonQuery();

                        if (result < 0)
                        {
                            //int errorCode = command.GetIntParameter(PARAM_ERRORCODE);
                            throw new Exception("Error trying to store questions.");
                        }
                    });
            }
        }

        public override Answer[] GetAnswers(IIdentity identity, int sectionID)
        {
            return SqlStorageCommand.Execute<Answer[]>(this.connection, SPROC_GETANSWERS,
                CommandType.StoredProcedure, (SqlStorageCommand command) =>
                {
                    List<Answer> answers = new List<Answer>();
                    command.AddParameter(PARAM_EMAILADDRESS, identity.Name);
                    command.AddParameter(PARAM_SECTIONID, sectionID);
                    command.ExecuteReader();
                    while (command.TryNextRow())
                    {
                        Answer answer = new Answer()
                        {
                            ID = command.GetColumnAsLong(COLUMN_ID),
                            QuestionID = command.GetColumnAsLong(COLUMN_QUESTIONID),
                            AnswerText = command.GetColumn(COLUMN_ANSWERTEXT),
                            OtherText = command.GetColumn(COLUMN_OTHERTEXT),
                        };
                        answers.Add(answer);
                    }
                    return answers.ToArray();
                });
        }

        public override void SetAnswers(IIdentity identity, Answer[] answers)
        {
            foreach (Answer a in answers)
            {
                if (!string.IsNullOrEmpty(a.AnswerText))
                {   // only store if answer text is defined
                    SqlStorageCommand.Execute(this.connection, SPROC_SETANSWER,
                        CommandType.StoredProcedure, (SqlStorageCommand command) =>
                        {
                            command.AddParameter(COLUMN_ID, a.ID);
                            command.AddParameter(PARAM_EMAILADDRESS, identity.Name);
                            command.AddParameter(COLUMN_QUESTIONID, a.QuestionID);
                            command.AddParameter(COLUMN_ANSWERTEXT, a.AnswerText ?? SqlString.Null);
                            command.AddParameter(COLUMN_OTHERTEXT, a.OtherText ?? SqlString.Null);
                            // TODO: add return code for errors
                            //command.AddOutputParameter(PARAM_ERRCODE, SqlDbType.Int);
                            int result = command.ExecuteNonQuery();

                            if (result < 0)
                            {
                                //int errorCode = command.GetIntParameter(PARAM_ERRORCODE);
                                throw new Exception("Error trying to store answers.");
                            }
                        });
                }
            }

        }


    }

}