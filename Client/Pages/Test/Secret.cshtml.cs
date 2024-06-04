using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Test
{
	[Authorize]
	public class SecretModel : PageModel
	{
		public void OnGet()
		{
		}
	}
}
