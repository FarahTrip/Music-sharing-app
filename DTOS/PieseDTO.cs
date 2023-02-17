using System;

namespace Trippin_Website.DTOS
{
    public class PieseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Key { get; set; }

        public int Bpm { get; set; }


        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public bool IsBanger { get; set; }

    }
}