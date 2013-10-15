<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TeenWise.WebSite.Models.RegisteredUser>" %>

<asp:Content ContentPlaceHolderID="Head" runat="server">
    <title>Register</title>
    <style type="text/css">
      .validation-summary-valid { display: none; }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
    <div class="row-fluid">
    <div class="span1"></div>
    <div class="span10 well">
    <% using (Html.BeginForm("logon", "account", FormMethod.Post, new { @class = "form-horizontal" })) { %>
            <fieldset>
                <legend>Register or Sign-in</legend>
<%  if (Model == null) { %>

                    <div class="control-group">
                        <label class="control-label" for="email">Email address</label>
                        <div class="controls">
                            <input type="text" id="email" name="email" />
                        </div>
                    </div>
                    <div class="help-block">                
                        <p>Enter an email address to register with TeenWise.</p>
                        <p>If you are returning to the site, enter the email address you registered previously.</p>
                    </div>
                    <div class="form-actions">
                        <button class="btn btn-primary" type="submit">Submit</button>
                    </div>
                    <%: Html.ValidationSummary("", new { @class = "alert alert-error" })%> 

<%  } else { %>           
                     
                    <div class="help-block">                
                            <p>Thank you for registering with TeenWise!</p>
                            <p>Check out our product at <a href="http://www.webtimer.co">www.webtimer.co</a>!</p>
                            <!-- <p>To learn more about our product plans...</p> 
                            <a class="btn btn-primary" href="/survey">Take our survey!</a> -->
                    </div>

<% } %>
            </fieldset>
    <% } %>
    </div>
    </div>
    </div>
</asp:Content>
