using AutoMapper;
using Compentence_Days_Game.Backend.Database;
using Compentence_Days_Game.Backend.Database.Entities;
using Compentence_Days_Game.Backend.Interfaces;
using Compentence_Days_Game.Backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Compentence_Days_Game.Backend.Consts;

namespace Compentence_Days_Game.Backend.Services
{
    public class TimesForPrizesDTO
    {
        public int PrizeId { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class PriceGenerator : IPriceGenerator
    {
        private ICDGameService _gameService;
        private IMapper _mapper;

        private Random _random;
        private List<TimesForPrizesDTO> _timesForPrizes;
        private PrizesConst _prizesConst;

        public PriceGenerator(ICDGameService gameService, IMapper mapper)
        {
            _gameService = gameService;
            _mapper = mapper;
            _random = new Random();
            _timesForPrizes = new List<TimesForPrizesDTO>();
            _prizesConst = new PrizesConst();
        }

        private List<Prize> getBystronicPrizes()
        {
            return _gameService.GetCompanyPrizes("Bystronic");
        }

        private List<Prize> getAlthausPrizes()
        {
            return _gameService.GetCompanyPrizes("Althaus");
        }

        private Prize GetLastPrize()
        {
            int lastUserPrizeId = (_gameService.GetLastUser() == null) ? _prizesConst.KinoticketsId : _gameService.GetLastUser().PrizeId;
            Prize prize = _gameService.GetPrice(lastUserPrizeId).Result;
            return prize;
        }

        public async Task<Prize> GetNextPrice(bool student)
        {
            Prize lastPrize = GetLastPrize();
            List<Prize> prices = new List<Prize>();

            if (lastPrize.Id == _prizesConst.KugelschreiberId) { prices = getBystronicPrizes(); }
            else if (lastPrize.Id == _prizesConst.FlaschenhalterId) { prices = getBystronicPrizes(); }
            else if (lastPrize.Id == _prizesConst.PostItBüchleinId) { prices = getAlthausPrizes(); }
            else if (lastPrize.Id == _prizesConst.MinttäfeliId)
            {
                prices = getAlthausPrizes();
            }
            else
            {
                prices = await _gameService.GetAllPrices();
            }

            if (student)
            {
                prices = prices.Where(p => p.Id != 5).ToList();
            }

            var result = GenerateRandom(prices).Source;

            if (result.Count <= 0) result = GetSubstitudePrice(prices, result);
            if (result == null) return null;

            result.Count--;
            await _gameService.UpdatePrice(result);

            return result;
        }

        /// <summary>
        /// Return a substitude if a price is already out.
        /// The price can only be supstituded by another with an equal or higher weight.
        /// The substitude also needs to have a count higher than 0.
        /// </summary>
        /// <param name="prices"></param>
        /// <param name="wonPrice"></param>
        /// <returns></returns>
        private Prize GetSubstitudePrice(List<Prize> prices, Prize wonPrice)
        {
            return prices
                .OrderBy(x => x.Probability)
                .Where(x => x.Probability >= wonPrice.Probability)
                .FirstOrDefault(x => x.Count > 0);
        }

        /// <summary>
        /// Generate a random Price depending on weight.
        /// </summary>
        /// <param name="prices"></param>
        /// <returns></returns>
        private WeightConfiguration GenerateRandom(List<Prize> prices)
        {
            var weightConf = _mapper.Map<IList<WeightConfiguration>>(prices);
            var orderedConf = weightConf.OrderByDescending(x => x.RealWeight).ToList();

            var currI = 0;
            orderedConf.ForEach(weightConf =>
            {
                weightConf.WeightStart = currI + 1;
                currI += weightConf.RealWeight;
                weightConf.WeightEnd = currI;
            });

            var rndVal = _random.Next(1, currI + 1);

            return orderedConf.First(x => rndVal <= x.WeightEnd && rndVal >= x.WeightStart);
        }

    }
}
