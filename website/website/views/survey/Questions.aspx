<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Survey.Master" Inherits="System.Web.Mvc.ViewPage<Survey>" %>
<%@ Import Namespace="TeenWise.WebSite.Models" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="survey-content row-fluid">
    <form id="form" action="<%: Url.Content("~/survey/questions") %>" method="post" >

        <h2><%: Model.CurrentSection.Name %></h2>

<%  // question loop
    int i = 0;      // question counter
    int m = 0;      // multipart counter
    int index = 0;  // question display count
    foreach (Question q in Model.CurrentQuestions)
    {
        Answer a = Model.CurrentAnswers[i];        
        string id = "q_" + (i++).ToString();
        string displayIndex = string.Empty;
        string displayText = q.DisplayText();

        string qClass = "question";
        string pClass = "prompt-break";
        if ((q.Type == QuestionType.ListChoice) ||
            (displayText.Length < 50 && q.Type == QuestionType.SingleText) ||
            (displayText.Length < 10 && (q.Type == QuestionType.MultiText && int.Parse(q.Options[0]) == 1)))
        {
            pClass = "prompt-nobreak";
        }
                       
        if (m > 0) 
        { 
            m--;
            qClass = "question-part";
        }
        else 
        {
            if (q.Type == QuestionType.MultiPart) 
            { 
                int.TryParse(q.Options[0], out m);
            }
            index++;
            displayIndex = index.ToString() + ". ";
        }
%>
    <div class="<%: qClass %>">
        <div class="<%: pClass %>"><%: displayIndex %><%= displayText %></div>
        <div class="answer">

<%      // single text type
        if (q.Type == QuestionType.SingleText) 
        { 
%>
            <input type="text" name="<%: id %>" value="<%: a.AnswerText %>" />
<%      } %>

<%      // multi text type
        if (q.Type == QuestionType.MultiText) 
        {
            string tClass = (int.Parse(q.Options[0]) == 1) ? "single-row" : "";
%>
            <textarea name="<%: id %>" rows="<%= q.Options[0] %>" class="<%: tClass %>" ><%= a.AnswerText %></textarea>
<%      } %>

<%      // single choice type
        if (q.Type == QuestionType.SingleChoice) 
        {
            foreach (string option in q.Options) 
            {
                string selected = a.AnswerText.Contains(option) ? "checked" : "";
%>            
                <div class="single-choice">
                    <input type="radio" name="<%: id %>" value="<%: option %>" <%= selected %> /><span><%: option %></span>
                </div>
<%          } %>
<%      } %>

<%      // scale choice type
        if (q.Type == QuestionType.ScaleChoice) 
        {
            int j = 0; 
            foreach (string option in q.Options) 
            {
                string selected = a.AnswerText.Contains(option) ? "checked" : "";
                string hidden = (j == 0 || j == (q.Options.Length - 1)) ? "none" : "inline";
                j++;
%>            
                <div class="scale-choice">
                    <input type="radio" name="<%: id %>" value="<%: option %>" <%= selected %> style="display:<%= hidden %>;" /><span><%: option %></span>
                </div>
<%          } %>
<%      } %>

<%      // multi choice type
        if (q.Type == QuestionType.MultiChoice) 
        { 
            foreach (string option in q.Options) 
            {
                string selected = a.AnswerText.Contains(option) ? "checked" : "";
%>            
                <div class="single-choice">
                    <input type="checkbox" name="<%: id %>" value="<%: option %>" <%: selected %> /><span><%: option %></span>
                </div>
<%          } %>
<%      } %>

<%      // list choice type
        if (q.Type == QuestionType.ListChoice) 
        {
%>
            <select name="<%: id %>" >
                <option value="">-- select --</option>
<%          foreach (string option in q.Options) 
            {
                string selected = a.AnswerText.Contains(option) ? "selected" : "";
%>            
                <option value="<%: option %>" <%= selected %> ><%: option %></option>
<%          } %>
            </select>
<%      } %>

<%      // other text
        if (q.OtherText != null) 
        {
            string other = a.OtherText;
            string oClass = (q.Type == QuestionType.ListChoice) ? "single-row medium" : "single-row small";
%>
            <div class="label-other"><%: q.OtherText %></div>
            <textarea name='<%: id + "_other" %>' rows="1" class="<%: oClass %>"><%: other %></textarea>
<%      } %>

        </div>
    </div>

<%  } %>
        
        <br />
        <button class="btn btn-primary" onclick="form.submit()">Continue...</button>    
        <input type="hidden" name="section" value="<%: Model.CurrentSectionIndex.ToString() %>" />
    </form>
    </div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ScriptBlock" runat="server">
    <script type="text/javascript">
        // document ready handler
        $(function () {
            //var jsonAnswers = '<%= JsonSerializer.Serialize(Model.CurrentAnswers) %>';
            //var answers = JSON.parse(jsonAnswers);
        });
    </script>
</asp:Content>

