using System;

namespace Trippin_Website.Models
{
    public class Grupuri
    {
        public Guid? Id { get; set; }
        public string GroupAdminId { get; set; }
        public string GroupAdminSecondId { get; set; }
        public string Name { get; set; }
        public DateTime? Created { get; set; }
        public int? TargetPiesePeLuna { get; set; }

    }
}