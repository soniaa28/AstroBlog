using Microsoft.AspNetCore.Mvc.Rendering;

namespace AstroBlog.Models.ViewModel
{
	public class UserViewModel
	{
		public Guid Id { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public IEnumerable<SelectListItem>? Tags { get; set; }
		public string[] SelectedTags { get; set; } = Array.Empty<string>();
	}
}
