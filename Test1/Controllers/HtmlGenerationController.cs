using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using Test1.Data;
using Test1.Models;

[Route("api/[controller]")]
[ApiController]
public class HtmlGenerationController : ControllerBase
{

	private readonly AppDbContext _context;

	private Dictionary<string, string> htmlContentDictionary = new Dictionary<string, string>();

	public HtmlGenerationController(AppDbContext context)
	{
		_context = context;
	}

	[HttpGet("html/{name}")]
	public IActionResult GetHtml(string name)
	{
		// Check if the provided uniqueId (name) exists in the User table
		var user = _context.User.FirstOrDefault(u => u.Name == name);

		if (user == null)
		{
			// User not found in the database, return an error HTML page
			string errorHtml = $@"
<!DOCTYPE html>
<html lang=""en-us"" prefix=""content: http://purl.org/rss/1.0/modules/content/ dc: http://purl.org/dc/terms/ foaf: http://xmlns.com/foaf/0.1/ og: http://ogp.me/ns# rdfs: http://www.w3.org/2000/01/rdf-schema# sioc: http://rdfs.org/sioc/ns# sioct: http://rdfs.org/sioc/types# skos: http://www.w3.org/2004/02/skos/core# xsd: http://www.w3.org/2001/XMLSchema#"">

<head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
    <style type=""text/css"">
        @charset ""UTF-8"";
        [ng\:cloak],
        [ng-cloak],
        [data-ng-cloak],
        [x-ng-cloak],
        .ng-cloak,
        .x-ng-cloak,
        .ng-hide:not(.ng-hide-animate) {{
            display: none !important;
        }}

        ng\:form {{
            display: block;
        }}

        .ng-animate-shim {{
            visibility: hidden;
        }}

        .ng-anchor {{
            position: absolute;
        }}
    </style>
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
    <title>Oops, something lost</title>
    <meta name=""description"" content=""Oops, looks like the page is lost. Start your website on the cheap."">
    <link media=""all"" rel=""stylesheet"" href=""/htdocs_error/style.css"">
    <link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"">
    <link href=""https://fonts.googleapis.com/css?family=Open+Sans:300,300i,400,400i,600,600i,700,700i,800,800i"" rel=""stylesheet"">

    <script>
        (function(i,s,o,g,r,a,m){{i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){{
                (i[r].q=i[r].q||[]).push(arguments)}},i[r].l=1*new Date();a=s.createElement(o),
            m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
        }})(window,document,'script','https://www.google-analytics.com/analytics.js','ga');

        ga('create', 'UA-26575989-46', 'auto');
        ga('send', 'pageview');

    </script>
</head>

<body>

    <div class=""error"" id=""error"">
            <div class=""container"">
                <div class=""content centered""><img style=""width:500px;"" src=""https://conferencecaptioning.com/htdocs_error/something-lost.png"">
                    <h1>Oops, looks like the page is lost.</h1>
                    <p style=""font-size:22px;"" class=""sub-header text-block-narrow"">This is not a fault, just an accident that was not intentional.</p>
                </div>
        </div>
</body>

</html>
";

			return Content(errorHtml, "text/html");
		}

		// Updating the viewers count by 1
		if (user.ViewersCount == null)
			user.ViewersCount = 1;
		else
			user.ViewersCount = user.ViewersCount + 1;
		
		// saving changes to db
		_context.SaveChanges();

		// User found in the database, fetch the generatedHtml data
		string generatedHtml = user.GeneratedHTML;

		// Return the fetched HTML content in the API response
		return Content(generatedHtml, "text/html");
	}


	[HttpPost]
	public IActionResult GenerateHtml([FromBody] HtmlGenerationRequest request)
	{
		
		// Generate a unique identifier for the HTML page (e.g., a random string or a unique ID)
		//string uniqueId = Guid.NewGuid().ToString();

		// Check if the provided uniqueId exists in the database
		var link = _context.User.FirstOrDefault(l => l.Name == request.Name);

		if (link != null)
		{
			// If the uniqueId already exists, return an error indicating it's a duplicate
			string errorHtml = $@"
<html>
<body>
<h1>Error: Duplicate Link</h1>
<p>The provided uniqueId already exists in the database.</p>
</body>
</html>
        ";

			return Content(errorHtml, "text/html");
		}



		// Generate HTML content dynamically based on the input data
		string htmlContent = $@"<!DOCTYPE html>
<html lang=""en"" >
<head>
  <meta charset=""utf-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
  <title>{request.Name}- Event Live Captioning, powered by BeAware</title>
  <meta name=""description"" content=""WeTech Event Live Captioning, powered by BeAware"">    
  <meta charset=""utf-8"">
  <meta name=""author"" content=""Saamer Mansoor"">
  <!--[if IE]><meta http-equiv='X-UA-Compatible' content='IE=edge,chrome=1'><![endif]-->
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />

  <meta name=""keywords"" content=""Deaf, HoH, HardofHearing, Hard, Hearing, Assistant, Android, iPhone, iOS, App, Website"" />
  <link rel=""canonical"" href=""https://www.conferencecaptioning.com/wetech"">  
  <meta property=""og:site_name"" content=""WeTech Event Live-Captioning by BeAware"">
  <!-- Twitter Meta Tags -->
  <meta name=""twitter:card"" content=""summary_large_image"">
  <meta name=""twitter:title"" content=""WeTech Event Live-Captioning By BeAware"">
  <meta name=""twitter:description"" content=""Native Conference Live-Captioning for serving Deaf & Hard of Hearing attendees"">
  <meta name=""twitter:image"" content=""https://conferencecaptioning.com/images/author.png"">
  <!-- Google / Search Engine Tags -->
  <meta itemprop=""name"" content=""WeTech Event Live-Captioning By BeAware"">
  <meta itemprop=""description"" content=""WeTech Event Live Captioning for serving Deaf & Hard of Hearing attendees"">
  <meta itemprop=""image"" content=""https://conferencecaptioning.com/images/author.png"">
  <!-- Facebook Open Graph -->
  <meta property=""og:url"" content=""https://conferencecaptioning.com"">
  <meta property=""og:type"" content=""website"">
  <meta property=""og:title"" content=""WeTech Event Live-Captioning by BeAware"">
  <meta property=""og:description"" content=""WeTech Event Live-Captioning for serving Deaf & Hard of Hearing attendees"">
  <meta property=""og:image"" content=""https://conferencecaptioning.com/images/author.png"">
  <meta name=""robots"" content=""noodp,noydir"">
    <!-- Facebook Open Graph end -->

  <link rel=""apple-touch-icon"" sizes=""57x57"" href=""https://conferencecaptioning.com/images/apple-touch-icon.png"">
  <link rel=""apple-touch-icon"" sizes=""72x72"" href=""https://conferencecaptioning.com/images/apple-touch-icon-72x72.png"">
  <link rel=""apple-touch-icon"" sizes=""114x114"" href=""https://conferencecaptioning.com/images/apple-touch-icon-114x114.png"">
  <!-- <link rel=""apple-touch-icon"" sizes=""180x180"" href=""images/apple-touch-icon.png""> Need 180x180 icon medium-->
  <link rel=""icon"" type=""image/png"" sizes=""32x32"" href=""https://conferencecaptioning.com/images/favicon.png"">
  <!-- <link rel=""icon"" type=""image/png"" sizes=""16x16"" href=""favicon/favicon-16x16.png""> Need this low-->
  <link rel=""manifest"" href=""../favicon/site.webmanifest"">

  <!-- Favicons -->
  <link rel=""shortcut icon"" href=""https://conferencecaptioning.com/images/favicon.png"">
  



  <link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css'>
  <!-- <link rel=""stylesheet"" href=""./style.css""> -->
  <style>
	.holder {{
  font-family: sans-serif;
  font-size: 1.5em;
  line-height: 1.75;
  margin: auto;
  padding: 20px 20px;
}}

img {{
  width: 100%;
}}

h1,h2,h3,h4,h5,h6 {{
  margin: 0em 0 1em;
}}

p,ul,ol {{
  color: #1d1d1d;
  margin-bottom: 2em;
}} 


input {{
  padding: 7px;
  border-radius: 6px;
  font-size: 20px;
  background: #fbfbfb;
  border: 2px solid transparent;
  height: 36px;
  box-shadow: 0 0 0 1px #dddddd, 0 2px 4px 0 rgb(0 0 0 / 7%),
    0 1px 1.5px 0 rgb(0 0 0 / 5%);
}}
input:focus {{
  border: 2px solid #000;
  border-radius: 4px;
}}

.result{{
  padding: 40px;
  box-shadow: 0 14px 28px rgba(0,0,0,0.25), 0 10px 10px rgba(0,0,0,0.22);
}}

.center {{
  margin: auto;
  text-align: center;
}}

.ios-button{{
  padding: 13px 27px;
  align-items: center;
  gap: 10px;
  border-radius: 24px;
  background: #1d3b78;
}}
.ios-button-text{{
  color: var(--neutrals-white, #FFF);
  text-align: center;
  font-family: Roboto;
  font-size: 18px;
  font-style: normal;
  font-weight: 600;
  line-height: normal;
}}
#live-caption{{
    max-height: 370px;
    overflow-y: scroll;
    font-size: large;
    padding: 10px;
}}
#footer {{
    bottom: 0;
    width: 100%;
    position: absolute;
    height: $height-footer;
    background-color: #f5f5f5;
    margin: 0px 0;
  }}

.scroller {{
  max-height: 50%;
  overflow: auto;
  display: flex;
  flex-direction: column-reverse;
  margin: auto;
  text-align: center;
}}



#header
{{
	background-color:#1d3b78;
	font-family:Verdana, Arial, Helvetica, sans-serif;
	display:block;
  width: 100%;
}}

#header h1
{{
	font-weight:normal;
	padding-top:20px;
	margin-left:10px;
	font-family:Verdana, Arial, Helvetica, sans-serif;
	margin-top:0;
	margin-bottom:0;
  color: white;
}}


#header h2
{{
	font-weight:normal;
	font-size:14px;
	padding:5px 0 20px 80px;
	font-family:Verdana, Arial, Helvetica, sans-serif;
	margin-top:0;
	margin-bottom:0;
  color: white;
}}

a.disabled {{
  pointer-events: none;
  cursor: default;
  color: grey;
}}

a.active {{
  pointer-events: none;
  cursor: default;
  color: black;
}}

.customPadding{{
  padding:20px;
}}
  </style>
</head>
<body style=""width: 100%; margin: 0px"">
  <!-- partial:index.partial.html -->
  <div id=""header"" class=""customPadding"" style=""justify-content: center;"">
    <!-- <div style=""display:flex""> -->
      <img style=""height:40px; width:150px; justify-content: center;"" src=""https://www.conferencecaptioning.com/images/logo-light.png""> 
      <img style=""height:24px; width:24px; justify-content: center;"" src=""https://www.conferencecaptioning.com/wetech/images/x.png"">
      <img style=""height: 43.3px; width:122px; justify-content: center;"" src=""{request.ImageURL}"">
      <h1 id=""caption-header"" class=""caption-header"" style=""font-size: 24px; justify-content: center;"">Event Live Captioning</h1>
    <!-- </div> -->
  </div>
  <div class=""holder"">
  <div style=""display: flex; justify-content: center; padding: 0px 0px 10px 0px"">
    <button id=""get-live-caption"" class=""center ios-button ios-button-text"">Get Live Captions</button>
  </div>
  <div style=""height: 400px; border: #1d3b78; border-style: solid;"">
    <div id=""live-caption-empty"" class=""scroller scroller-empty"">Transcription will display here</div>
    <div id=""live-caption"" class=""scroller""></div>
    <div style=""display: flex; justify-content: center;"">
      <i id=""#arrow"" class=""fa-solid fa-arrow-down fa-fade center""></i>
    </div>
  </div>
  <div style=""display: flex; justify-content: center; padding:20px"">
    <i class=""fa fa-language"" aria-hidden=""true"" style=""padding: 0px 10px 0px 10px; color:#1d3b78""></i>
    <a href=""javascript:void(0);""><div id=""eng"" style=""font-size: 18px; padding: 0px 10px 0px 10px;"" class=""active"">English</div></a>
    <a href=""javascript:void(0);""><div id=""french"" style=""font-size: 18px; padding: 0px 10px 0px 10px;"" class=""disabled"">Français</div></a>
  </div>
  <div>
  <span id=""hotmail"" style=""justify-content: center; padding:0px; font-size: 18px; color:#000000; text-decoration: none;"">
    PS: I love you. Get your free live-event transcription at
  </span>
  <a href=""https://conferencecaptioning.com/?src=wetech"" style=""justify-content: center; padding:0px; font-size: 18px; text-decoration:underline;color:#0000ff;"">
    ConferenceCaptioning
  </a>
  </div>
  </div>
  <script src='https://cdnjs.cloudflare.com/ajax/libs/jquery/3.7.0/jquery.min.js'></script>
  <script src='https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.0/js/bootstrap.min.js'></script>
  <!-- <script src=""./script.js""></script> -->
  <script src=""../js/analytics.js""></script>
  <script type=""text/javascript"">
  $(document).ready(function() {{

  loadLang(""eng"")
  $(""#french"").click(function () {{
    // console.log(""SaamerD!"");
    translate(""french"");
  }});
  $(""#eng"").click(function () {{
    // console.log(""SaamerD!"");
    translate(""eng"");
  }});

  // Loads quotes as user wishes on clicking the button
  $(""#get-live-caption"").on(""click"", buttonTapped);
  //$(""#arabic"").on(""click"", function() {{ translate(""arabic""); }});
  // Loads the initial quote - without pressing the button
  const unusedVariable = setInterval(recurringFunction, 1000);  
}});


var translations =  {{
  eng: """",
  // arabic: """",
  french: """"
}};

eng = document.getElementById(""eng"");
//arabic = document.getElementById(""arabic"");
french = document.getElementById(""french"");

var isStreamingCaptions = false; 
function buttonTapped() {{
  if (isStreamingCaptions){{
    stopTimer() 
  }} else{{ 
    startTimer();
  }}
  isStreamingCaptions = !isStreamingCaptions;
}}

function showRightTranscript(){{
  if (currentLanguage === ""eng""){{
    transcript = translations.eng
  }} else {{
    transcript = translations.french
  }}
  $(""#live-caption"").html(transcript);
}}

var localization = """"
function loadLang(lang){{
  $.getJSON(""https://conferencecaptioning.com/wetech/"" + lang + "".json"", (text) => {{
    localization = text
    document.getElementById(""caption-header"").html(text['caption-header']);
    // if(isStreamingCaptions){{
    //   document.getElementById(""get-live-caption"").html(text['get-live-caption-stop']);
    // }}
    // else{{
    //   document.getElementById(""get-live-caption"").html(text['get-live-caption']);
    // }}
    document.getElementById(""live-caption-empty"").html(text['live-caption-empty']);
    document.getElementById(""hotmail"").html(text['hotmail']);
    document.getElementById(""eng"").html(text['english-language']);
    document.getElementById(""french"").html(text['french-language']);
  }});
}}

var transcript = """";
var isTesting = false; //TODO: Before publishing, Change this to false
var counter = 0; // Only used for debug
function recurringFunction() {{
  if (translations.eng == """"){{ //if transcript is empty, show/hide the placeholder
    $('#live-caption-empty').show;
  }}
  else {{
    $('#live-caption-empty').hide();
    showRightTranscript()
  }}
  if (isStreamingCaptions) {{
    if (isTesting) {{
      transcript = transcript + transcript;
      $(""#live-caption"").html(transcript+counter++);
    }} else {{
      getTranscript();
    }}
  }}
}}

function startTimer() {{
  $(""#get-live-caption"").html(""Stop Streaming"");
  $(""#get-live-caption"").html(localization['get-live-caption-stop']);
  // eng.className = ""active"";
  //arabic.className = ""disabled"";
  // french.className = ""disabled"";
}}

function stopTimer() {{
  $(""#get-live-caption"").html(""Get Live Captions"");
  $(""#get-live-caption"").html(localization['get-live-caption']);
  //arabic.className = """";
  // french.className = """";
}}

function getTranscript() {{
  var url=""https://script.google.com/macros/s/AKfycbzqOWlC9bT6TtLp1QJLzAkwDZJKTcCZYnoDhN4JIMXTo5lEvtPruYb-3vrILj__yO_A/exec?streamName=WeTech"";
  // To avoid using JQuery, you can use this https://stackoverflow.com/questions/3229823/how-can-i-pass-request-headers-with-jquerys-getjson-method
  $.getJSON(
    url,
    function (a) {{
      var json = JSON.stringify(a);
      // console.log(json)
      if (a && a.Transcript && a.Transcript != """") {{
        // transcript = a.Transcript;
        translations.eng = a.Transcript; //english
        translations.french = a.Transcript_FR;
        // console.log(translations.eng)
        // console.log(translations.french)

        // translations.arabic = a.Transcript_AR;
        // $(""#live-caption"").html(transcript);
        if (!a.IsActivelyStreaming){{
          buttonTapped(); // Automatically stop streaming if event is not live
        }}

      }}
    }}
  );
}}

var currentLanguage = ""eng"" // ""french"" is the other choice
function translate(language){{
  currentLanguage = language
  loadLang(language)
  // console.log(""SaamerE!"");
  // console.log(""H"" + language);
  eng.className = """";
  //arabic.className = """";
  french.className = """";
  document.getElementById(language).className = ""active"";
  // $(""#live-caption"").html(translations[language]);
}}

/* ---------------------------------------------
 Custom Analytics
 --------------------------------------------- */
 (function($){{
    ""use strict""; // Start of use strict    
    $(document).ready(function(){{
        pageViewed();
        requestReferrerAndLocation();  
    }});
}})(jQuery); // End of use strict


/* ---------------------------------------------
 Custom GDPR compliant analytics
 --------------------------------------------- */
 
  function pageViewed() {{
    var url = ""https://script.google.com/macros/s/AKfycbxzEJVBRmE-z7ZY4C6FRzxPn28TKW6mozP73FfPuVgXYgauG3_MnhroSoe5wVyE8eUkMg/exec"";
    var myJSObject='{{""Event"": ""' + ""PageView: BeAware"" + '""}}';    
    postCall(url, myJSObject);
  }}

 function submitMessage()
 {{
   var Name = document.getElementById('contact_name').value;
   var Email = document.getElementById('contact_email').value;      
   var Message = document.getElementById('contact_message').value;      
   postFeedbackAPI(Name, Email, Message)
 }}
 
 function requestReferrerAndLocation()
 {{
   $.getJSON(""https://ipinfo.io/json"", function (data) {{
     console.log(""data: "" + data);
     var str = data.city + "", "" + data.region + "", "" + data.country;
     console.log(""IP: "" + str);
     sendLocationRequest(str);
   }});
 }}  
 
 function sendLocationRequest(str)
 {{
   var Name = str;
   var Email = document.URL;      
   var Message = document.referrer; 
   postFeedbackAPI(Name, Email, Message)
 }}
  
 function postFeedbackAPI(Name, Email, Message)
 {{
   var url = ""https://script.google.com/macros/s/AKfycbz42xFl_59V36k5VJgldCLFRBv9Gw1n2Z6XapMt1V9d_G-deUaoaOYbkqHddM3HnzA/exec"";
   var myJSObject='{{""Name"": ""' + Name + '"", ""Email"" : ""' + Email + '"", ""Message"" : ""' + Message + '""}}';    
   postCall(url, myJSObject);
 }}
 
 function postCall(url, myJSObject) {{
     $.ajax({{
     type: ""POST"",
     url: url,
     data: myJSObject,
     success: function (response) {{
       console.log(response);
     }},
     error: function (error) {{
       console.log(error.responseText);
     }},
   }});
 }}
  </script>
  
  
  
  </body>  
</html>
";

		// Construct a link to access the HTML page
		string linkToGeneratedPage = $"/api/HtmlGeneration/html/{request.Name}";

		// Store the generated link in the database
		var newLink = new User
		{
			Name = request.Name,
			GeneratedHTML = htmlContent,
			Email = request.Email,
			ImageURL = request.ImageURL,
			ViewersCount = 0
		};

		_context.User.Add(newLink);
		_context.SaveChanges();

		// Return the link in the API response
		return Ok(new { link = linkToGeneratedPage });
	}
}