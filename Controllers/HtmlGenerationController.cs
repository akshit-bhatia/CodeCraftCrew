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
<html>
<body>
<h1>Error: Stream not found</h1>
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
		string htmlContent = $@"
<html>
<body>
<h1>Hello, {request.Name}!</h1>
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