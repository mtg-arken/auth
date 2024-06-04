using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAuth.Pages.Account
{
	public class LoginModel : PageModel
	{
		[Parameter]
		public string state { get; set; }

		public void OnGet()
		{
		}

		public IActionResult OnPost()
		{
			var redirectUri = Request.Form["redirect_uri"];
			string state = Request.Form["state"];

			const string code = "BABAABABABA";

			var query = new QueryBuilder();
			query.Add("code", code);
			query.Add("state", state);

			return Redirect($"{redirectUri}{query.ToString()}");

		}

	}
}
