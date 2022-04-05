using Compentence_Days_Game.Backend.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compentence_Days_Game.Backend.Interfaces
{
    public interface IPriceGenerator
    {

        public Task<Prize> GetNextPrice(bool student);

    }
}
