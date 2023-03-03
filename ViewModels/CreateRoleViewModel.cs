using System.ComponentModel.DataAnnotations;

namespace Trippin_Website.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}