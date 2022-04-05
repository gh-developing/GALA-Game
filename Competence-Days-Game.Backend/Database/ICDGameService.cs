using Compentence_Days_Game.Backend.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Compentence_Days_Game.Backend.Consts;

namespace Compentence_Days_Game.Backend.Database
{
    public interface ICDGameService
    {

        public Task<User> AddNewUser(User user);
        public Task<User> GetUser(string phoneNr);
        public Task<IReadOnlyList<User>> GetAllUsers();
        public User GetLastUser();

        public Task<Prize> UpdatePrice(Prize price);
        public Task<Prize> GetPrice(int id);
        public Task<List<Prize>> GetAllPrices();
        public Task<string> GetUsersAsCsv();
        public List<Prize> GetCompanyPrizes(string company);

        public Task InitPrices();

    }
}
