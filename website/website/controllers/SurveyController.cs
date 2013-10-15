namespace TeenWise.WebSite.Controllers
{
    using System;
    using System.Web.Mvc;
    using TeenWise.WebSite.Models;
    using TeenWise.WebSite.Models.Storage;

    public class SurveyController : Controller
    {
        public ActionResult Index()
        {
            return Start();
        }

        public ActionResult Start()
        {
            return View("Start");
        }

        public ActionResult Questions()
        {
            int sectionIndex = 0;
            string section = this.Request.Params["section"];
            if (section != null)
            {
                int.TryParse(section, out sectionIndex);
            }
            else
            {
                AccountController.SendEmailNotification("Zaplify Survey Started!", this.User.Identity.Name);
            }
            Survey survey = new Survey(this.User.Identity, sectionIndex);
            if (this.Request.HttpMethod.ToUpperInvariant().Equals("GET"))
            {   // no responses posted
                return View(survey);
            }

            if (ProcessAnswers(survey))
            {   // move to next section of survey
                survey.CurrentSectionIndex = survey.CurrentSectionIndex + 1;
            }

            string view;
            if (survey.IsView(out view))
            {
                if (view.Equals("Finish", StringComparison.OrdinalIgnoreCase))
                {
                    AccountController.SendEmailNotification("Zaplify Survey Completed!", this.User.Identity.Name);
                }
                return View(view, survey);
            }
            return View("Questions", survey);
        }

        public ActionResult Video()
        {
            return View();
        }

        public ActionResult Zaplify(string command)
        {
            if (this.Request.Params["command"] == "create" &&
                this.User.Identity.Name == "stevemillet@comcast.net")
            {
                Survey.CreateSurvey();
            }
            return View("Start");
        }

        bool ProcessAnswers(Survey survey)
        {
            for (int i=0; i < survey.CurrentQuestions.Count; i++)
            {
                string answerKey = "q_" + i.ToString();
                string answer = this.Request.Form[answerKey];
                if (answer != null)
                {
                    survey.CurrentAnswers[i].AnswerText = answer;
                }

                answer = this.Request.Form[answerKey + "_other"];
                if (answer != null)
                {
                    survey.CurrentAnswers[i].OtherText = answer;
                }
            }
            // store processed answers
            Storage.Survey.SetAnswers(this.User.Identity, survey.CurrentAnswers.ToArray());
            return true;
        }

    }
}
