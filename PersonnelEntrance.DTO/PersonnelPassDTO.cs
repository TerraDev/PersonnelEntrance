using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PersonnelEntrance.DTO
{
    public class PersonnelPassDTO
    {
        public Guid passId { get; set; }

        [Required]
        public string personnelName { get; set; }

        [Required]
        public DateTime passTime { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PassEnumDTO passType { get; set; }
    }
}
