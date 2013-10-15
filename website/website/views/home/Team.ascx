<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div id="team-panel" class="row-fluid"> 
    <div class="span2"> </div>
    <div class="span8">
    <div class="row">
        <h1>Company</h1><br />          
    </div> 

    <div class="row">
        <p>
        <i>Teen Wise LLC</i> is an early-stage startup in the beautiful Seattle area.
        It was started by two parents just like you.
        </p>
        <br />
    </div>

    <div class="row team-member">
	    <div class="span4">
		    <h3>Sheri Gazitt</h3>
		    <br />
		    <div class="team-member-photo">
                <a onclick="return openWindow('http://www.linkedin.com/in/sherigazitt')">
                    <img src="/content/images/sheri.png" alt="Sheri Gazitt" />
                </a>
            </div>
        </div>
        <div class="span8">
		    <p>
            Sheri has a Masters in Psychology and is a certified teen life coach.  She is the founder of <a onclick="window.open('http://www.teenwiseseattle.com')">Teen Wise Seattle</a>, 
            a Teen Life Coaching practice.  Sheri speaks frequently about teen issues to both adults and teens around the greater Seattle area.
		    </p>
	    </div>
	</div>

    <div class="row team-member">
	    <div class="span4">
		    <h3>Omri Gazitt</h3>
		    <br />
		    <div class="team-member-photo">
                <a onclick="return openWindow('http://www.linkedin.com/in/ogazitt')">
                    <img src="/content/images/omri.png" alt="Omri Gazitt" />
                </a>
            </div>
        </div>
        <div class="span8">
		    <p>
            Omri is a software exec and the co-founder of <a onclick="window.open('http://www.builtsteady.com')">BuiltSteady</a>. 
            He's worked on many things over his 20 year career, but considers raising his three girls to be his greatest achievement.
            </p>
	    </div>
    </div>
    </div>
</div>


