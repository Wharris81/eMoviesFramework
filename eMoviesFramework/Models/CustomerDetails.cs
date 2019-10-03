using System.ComponentModel.DataAnnotations;

namespace eMoviesFramework.Models
{
    public class CustomerDetails
    {
        [RegularExpression(@"^[a-zA-Z\-]+$", ErrorMessage = "You may only enter letters and hyphens")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public int CustomerID { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Range(0, 9999999999999999, ErrorMessage = "You must enter a 16 digit card number")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "You must enter a 16 digit card number")]
        [Required(ErrorMessage = "Card number is required")]
        public string CardNumber { get; set; }

        public string CardNumberObfuscated { get { return CardNumber.Substring(0, 4) + " **** **** ****"; } }

        public string CardType { get; set; }

        public bool EmailPreference { get; set; }

        public string EmailPreferenceYN { get { return EmailPreference ? "Yes" : "No"; } }
    }
}
