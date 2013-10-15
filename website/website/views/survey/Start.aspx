<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Survey.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="TeenWise.WebSite.Models" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<% 
    string questionsUrl = Url.Content("~/survey/questions");
%>
    <div class="survey-content row-fluid">
        <p>Thank you for visiting our site.</p>
        <p>
            TeenWise is developing a consumer product to help parents stay on top of their kids' web usage.
            We have created a survey to get customer feedback. 
            The survey will begin by asking you to answer a few questions about the technology you use.
            
            In addition, we welcome any other input or ideas you might have to make the product relevant 
            and useful to you.
        </p><br />
        <p>Thank you for taking the time to participate, it is greatly appreciated.</p>
        <button class="btn btn-primary" onclick="window.location = '<%= questionsUrl%>'">Start Survey</button>
    </div>
</asp:Content>


