using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compentence_Days_Game.Backend.Model
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string KindOfPerson { get; set; }
        public string Class { get; set; }
        public string InterestedInJobAs { get; set; }
        public int PrizeId { get; set; }
        public PrizeDto Prize { get; set; }

        public DateTime? Timestamp { get; set; }

    }
}
