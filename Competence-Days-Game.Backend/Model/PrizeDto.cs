using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compentence_Days_Game.Backend.Model
{
    public class PrizeDto : BaseEntityDto
    {

        public string Name { get; set; }
        public float Probability { get; set; }
        public int Count { get; set; }

    }
}
