<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TeenWise.WebSite.Models.RegisteredUser>" %>

<asp:Content ContentPlaceHolderID="Head" runat="server">
    <title>TeenWise</title>
    <!--
    <script src="<%: Url.Content("~/content/videos/swfobject.js") %>" type="text/javascript"> </script>
    <script type="text/javascript">
        // document ready handler
        $(function () {
            swfobject.registerObject("csSWF", "9.0.115", "expressInstall.swf");
        });
    </script>
    -->
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main-content container-fluid">
        <!--
        <div id="main-panel" class="row-fluid">     
            <div class="span12">
                <div class="tagline">
                    <img runat="server" src="~/content/images/simplifytagline.png" alt="simplify your life" />
                </div>            
            </div>
        </div>
        -->
        <% Html.RenderPartial("Product"); %>
        <% Html.RenderPartial("About"); %>
        <% Html.RenderPartial("Team"); %>

    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="Footer" runat="server">
    <ul class="nav">
        <%--<li class="active"><a href="#main-panel">Welcome</a></li>--%>
        <li class="active"><a href="#product-panel">WebTimer</a></li>
        <li class=""><a href="#about-panel">About</a></li>
        <li class=""><a href="#team-panel">Company</a></li>
    </ul>
    <div class="pull-left">
        <a onclick="return openWindow('http://www.facebook.com/TeenWiseSeattle')" style="display:inline-block"><img runat="server" src="~/content/images/facebook.png" /></a>
        <a onclick="return openWindow('http://www.twitter.com/TeenWiseSeattle')" style="display:inline-block"><img runat="server" src="~/content/images/twitter.png" /></a>
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptBlock" runat="server">
    <script type="text/javascript">
        // document ready handler
        $(function () {
            $('.tagline').fadeIn(2000);
            $('.navbar-fixed-bottom').find('.nav a').click(function () { return showPanel($(this)); });
            $('#product-panel').show();
        });

        var openWindow = function openWindow$(url) {
            window.open(url);
            return false;
        }

        var showPanel = function showPanel$($btn) {
            $('.main-content').find('.row-fluid').hide();
            $btn.parents('ul').first().find('li').removeClass('active');
            $btn.parent().addClass('active');
            var panel = $btn.attr('href');
            /*
            if (panel == '#product-panel') {
                $('#video').html($('#video').html());
            }
            */
            $(panel).fadeIn();
            return false;
        }

        var clickNavBtn = function clickNavBtn$(panel) {
            var $btn = $('.navbar-fixed-bottom').find('.nav a[href="#' + panel + '"]');
            showPanel($btn);
        }
    </script>
</asp:Content>
