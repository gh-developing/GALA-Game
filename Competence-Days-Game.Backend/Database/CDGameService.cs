using Bystronic.Database;
using Compentence_Days_Game.Backend.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compentence_Days_Game.Backend.Consts;
using Microsoft.Extensions.Configuration;

namespace Compentence_Days_Game.Backend.Database
{
    public class CDGameService : ICDGameService
    {

        private IDbContext _context;
        private IConfiguration _configuration;
        private PrizesConst _prizesConst;
        public CDGameService(IDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<User> AddNewUser(User user)
        {
            user.Timestamp = DateTime.Now;
            var addedUser = await _context.Set<User>().AddAsync(user);
            await _context.SaveChangesAsync();
            return addedUser.Entity;
        }

        public async Task<Prize> GetPrice(int id)
        {
            return await _context.Set<Prize>()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetUser(string phoneNr)
        {
            return await _context.Set<User>()
                .AsQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == phoneNr);
        }

        public async Task<Prize> UpdatePrice(Prize price)
        {
            var updatedPrice = _context.Set<Prize>().Update(price);
            await _context.SaveChangesAsync();

            updatedPrice.State = EntityState.Detached;

            return updatedPrice.Entity;
        }

        public async Task<IReadOnlyList<User>> GetAllUsers()
        {
            return await _context.Set<User>()
                .AsQueryable()
                .AsNoTracking()
                .ToListAsync();
        }

        public User GetLastUser()
        {
            int userListLength = GetAllUsers().Result.Count;
            User user = GetAllUsers().Result[userListLength -1];

            return user;
        }

        public List<Prize> GetCompanyPrizes(string company)
        {
            List<Prize> companyPrizes = null;
            if (company == "Bystronic")
            {
                companyPrizes = _context.Set<Prize>()
                    .AsQueryable()
                    .AsNoTracking()
                    .Where(p => p.Id != 1)
                    .Where(p => p.Id != 2)
                    .ToList();
            }
            else
            {
                companyPrizes = _context.Set<Prize>()
                    .AsQueryable()
                    .AsNoTracking()
                    .Where(p => p.Id != 3)
                    .Where(p => p.Id != 4)
                    .ToList();
            }

            return companyPrizes;
        }

        public async Task<string> GetUsersAsCsv()
        {
            var users = await _context.Set<User>()
                .AsQueryable()
                .AsNoTracking()
                .Include(user => user.Prize)
                .Where(user => user.Email != "lorem.ipsum@dolor-sit-amet.com")
                .ToListAsync();

            var csv = new StringBuilder();
            csv.AppendLine($"Vorname,Name,Email,Preis,Klasse,Interesse am Beruf,Zeit");

            users.ForEach(user =>
            {
                csv.Append($"{user.FirstName},{user.LastName},{user.Email},{user.Prize.Name},{user.Class},{user.InterestedInJobAs},{user.Timestamp}");
                csv.Append("\n");
            });

            return csv.ToString();
        }

        public async Task<List<Prize>> GetAllPrices()
        {
            return await _context.Set<Prize>()
                .AsQueryable()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task InitPrices()
        {
            await _context.ExecuteRawAsync("CALL public.\"sp_InitStaticTypes\"();");

            var prizes = await _context.Set<Prize>().ToListAsync();
            var configuredPrizes = _configuration.GetSection("Prizes").Get<IEnumerable<Prize>>();

            if (configuredPrizes == null) return;

            foreach (var prize in prizes)
            {
	            var configuredPrize = configuredPrizes.FirstOrDefault(x => x.Id == prize.Id);
	            if (configuredPrize == null) continue;

	            prize.Count = configuredPrize.Count;
	            prize.Probability = configuredPrize.Probability;
            }

            await _context.SaveChangesAsync();

        }

    }
}
