using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Compentence_Days_Game.Backend.Database.Entities
{
    public class User
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string KindOfPerson { get; set; }
        public string Class { get; set; }
        public string InterestedInJobAs { get; set; }
        [Required]
        public int PrizeId { get; set; }
        public Prize Prize { get; set; }
        
        public DateTime? Timestamp { get; set; }
    }
}
