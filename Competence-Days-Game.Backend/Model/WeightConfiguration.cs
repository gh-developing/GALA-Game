using Compentence_Days_Game.Backend.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compentence_Days_Game.Backend.Model
{
    public class WeightConfiguration
    {

        public Prize Source { get; set; }

        internal int RealWeight { get; set; }
        internal int WeightStart { get; set; }
        internal int WeightEnd { get; set; }

    }
}
