<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if (Request.IsAuthenticated) { %>
<ul class="nav nav-pills pull-right">
    <li class="divider-vertical"></li>    
    <li class="dropdown active">
        <a class="dropdown-toggle" data-toggle="dropdown">
            <i class="icon-user icon-white"></i> <strong><%: Page.User.Identity.Name %></strong> <b class="caret"></b>
        </a>
        <ul class="dropdown-menu">
            <li><a href="<%: Url.Content("~/account/logoff") %>"><i class="icon-off"></i> Sign-out</a></li>
        </ul>
    </li>
</ul>
<% } else { %> 
    <ul class="nav pull-right">
        <li class="divider-vertical"></li>    
        <li><a href="<%: Url.Content("~/account/logon") %>">Register or Sign-in</a></li>
    </ul>
<% } %>

