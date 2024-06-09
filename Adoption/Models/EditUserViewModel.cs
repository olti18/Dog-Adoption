using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace AdoptionContext.Models
{
	public class EditUserViewModel
	{
		public EditUserViewModel() 
		{
			Claims = new List<String>();
			Roles = new List<String>();
		}

		[Required]
		public string Id { get; set; }
		//[Required]
		//[Display(Name = "Name")]
		//public string UserName { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
      
        [Display(Name = "FirstName")]
		public string? FirstName { get; set; }

        
		[Display(Name = "LastName")]
		public string? LastName { get; set; }
		public List<String> Claims { get; set; }
		public IList<String> Roles { get; set;}

	}
}
