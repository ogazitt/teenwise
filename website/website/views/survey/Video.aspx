<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Survey.Master" Inherits="System.Web.Mvc.ViewPage<Survey>" %>
<%@ Import Namespace="TeenWise.WebSite.Models" %>

<asp:Content ContentPlaceHolderID="Head" runat="server">
    <script src="<%: Url.Content("~/content/videos/swfobject.js") %>" type="text/javascript"> </script>
    <script type="text/javascript">
        // document ready handler
        $(function () {
            swfobject.registerObject("csSWF", "9.0.115", "expressInstall.swf");
        });
    </script>

    <style type="text/css">
        .main-content
        {
            margin-top: 40px;
        }

        .no-flash
        {
            margin: 0 auto;
            font-family:Arial, Helvetica, sans-serif;
            font-size: x-small;
            color: #cccccc;
            text-align: left;
            width: 210px; 
            height: 200px;	
            padding: 40px;
        }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="row-fluid" style="text-align:center">

    <video width="640" height="418" controls preload>
        <source src="/content/videos/zaplify.mp4" type="video/mp4" />
        <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="640" height="418" id="csSWF">
            <param name="movie" value="/contant/videos/zaplify_controller.swf" />
            <param name="quality" value="best" />
            <param name="bgcolor" value="#1a1a1a" />
            <param name="allowfullscreen" value="true" />
            <param name="scale" value="showall" />
            <param name="allowscriptaccess" value="always" />
            <param name="flashvars" value="autostart=false&thumb=/content/videos/zaplify.png&thumbscale=45&color=0x000000,0x000000" />
            <!--[if !IE]>-->
            <object type="application/x-shockwave-flash" data="/content/videos/zaplify_controller.swf" width="640" height="418">
                <param name="quality" value="best" />
                <param name="bgcolor" value="#1a1a1a" />
                <param name="allowfullscreen" value="true" />
                <param name="scale" value="showall" />
                <param name="allowscriptaccess" value="always" />
                <param name="flashvars" value="autostart=false&thumb=/content/videos/zaplify.png&thumbscale=45&color=0x000000,0x000000" />
            <!--<![endif]-->
                <div id="noUpdate" class="no-flash">
                    <p>
                    The Camtasia Studio video content presented here requires JavaScript 
                    to be enabled and the latest version of the Adobe Flash Player. 
                    If you are using a browser with JavaScript disabled please enable it now. 
                    Otherwise, please update your version of the free Adobe Flash Player by 
                    <a href="http://www.adobe.com/go/getflashplayer">downloading here</a>. 
                    </p>
                </div>
            <!--[if !IE]>-->
            </object>
            <!--<![endif]-->
        </object>
    </video>
<%
    if (Model != null)
    {
        string nextSectionUrl = Url.Content("~/survey/questions?section=" + (Model.CurrentSectionIndex+1).ToString());      
%>
        <div>
            Play the short video above to get a description of the product.<br />
            When the video is over, click the button to continue with the survey.<br />
            <button class="btn btn-primary" onclick="window.location = '<%= nextSectionUrl%>'">Continue...</button>        
        </div>
<%  } %>
</div>
</asp:Content>


