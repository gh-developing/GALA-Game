using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Compentence_Days_Game.Backend.Database.Entities
{
    public class Prize : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public float Probability { get; set; }
        [Required]
        public int Count { get; set; }
    }
}
