using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagDesktopUI.Library.Models
{
    public class CreateUserModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        public string? EmailAddress { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }

    }
}
