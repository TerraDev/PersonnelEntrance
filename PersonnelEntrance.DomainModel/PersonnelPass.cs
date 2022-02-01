using System;
using System.ComponentModel.DataAnnotations;

namespace PersonnelEntrance.DomainModel
{
    public class PersonnelPass
    {
        [Key]
        public Guid passId;

        //unique-composite
        [Required]
        public string employeeName;

        //unique-composite
        [Required]
        public DateTime passTime;

        [Required]
        public PassTypeEnum passType;
    }
}
