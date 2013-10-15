<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Survey.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <div class="survey-content row-fluid">
        <p>That concludes the Zaplify Product survey.</p>
        <br />
        <p style="max-width:500px">
            Thank you again for taking the time to participate, it is greatly appreciated.<br />
            Your feedback is being used to shape the product and will help determine the
            features available in an early-adopter release planned in the coming months. 
        </p>
        <button class="btn btn-primary" onclick="window.location = '<%= Url.Content("~/home/index")%>'">Done</button>
    </div>
</asp:Content>


