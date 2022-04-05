using AutoMapper;
using Compentence_Days_Game.Backend.Database.Entities;
using Compentence_Days_Game.Backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compentence_Days_Game.Backend.Config
{
    public class AutoMapper : Profile
    {

        public AutoMapper()
        {

            var priceToDto = CreateMap<Prize, PrizeDto>();
            var dtoToPrice = CreateMap<PrizeDto, Prize>();

            var userToDto = CreateMap<User, UserDto>();
            userToDto.ForMember(
                x => x.Timestamp,
                map => map.MapFrom(src => new DateTimeOffset(src.Timestamp.Value)));
            var dtoToUser = CreateMap<UserDto, User>();
            dtoToUser.ForMember(
                x => x.Timestamp,
                map => map.NullSubstitute(DateTime.Now));


            var priceWeight = CreateMap<Prize, WeightConfiguration>();
            priceWeight.ForMember(
                x => x.Source,
                map => map.MapFrom(src => src));
            priceWeight.ForMember(
                x => x.RealWeight,
                map => map.MapFrom(src => Convert.ToInt32(src.Probability * 100000)));

        }

    }
}
