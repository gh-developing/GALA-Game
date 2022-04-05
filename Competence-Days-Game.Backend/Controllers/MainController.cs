using AutoMapper;
using Bystronic;
using Compentence_Days_Game.Backend.Database;
using Compentence_Days_Game.Backend.Database.Entities;
using Compentence_Days_Game.Backend.Interfaces;
using Compentence_Days_Game.Backend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Compentence_Days_Game.Backend.Controller
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {

        private ICDGameService _gameService;
        private IProcessService _processService;
        private IConfiguration _configuration;
        private IPriceGenerator _priceGenerator;
        private IMapper _mapper;

        public MainController(
            ICDGameService gameService,
            IProcessService processService,
            IConfiguration configuration,
            IMapper mapper,
            IPriceGenerator priceGenerator)
        {
            _gameService = gameService;
            _processService = processService;
            _configuration = configuration;
            _priceGenerator = priceGenerator;
            _mapper = mapper;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult> InitDatabase()
        {
            var dbSb = new NpgsqlConnectionStringBuilder(_configuration["SqlServer:ConnectionString"]);

            var localDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var fileName = Path.Combine(localDir, @$"Database\InitDb\init_db.cmd");
            var directory = Path.Combine(localDir, @$"Database\InitDb\");

            Console.WriteLine($"Initialising CDGameDb on {dbSb.Host}");
            Console.WriteLine($"Using [{fileName}] inside [{directory}]");

            var psi = new ProcessStartInfo()
            {
                FileName = fileName,
                Arguments = $"{dbSb.Host} {dbSb.Username} {dbSb.Password}",
                CreateNoWindow = true,
                WorkingDirectory = directory
            };

            await _processService.StartProcessAsync(psi);

            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult> ReInitDataTypes()
        {
            await _gameService.InitPrices();

            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<PrizeDto>> GenerateUserPrize(UserDto userDto)
        {
            var price = (userDto.KindOfPerson == "SchuelerIn")
                ? await _priceGenerator.GetNextPrice(true)
                : await _priceGenerator.GetNextPrice(false);
            userDto.PrizeId = price.Id;
            userDto.Timestamp = DateTime.Now;
            var addedUser = await _gameService.AddNewUser(_mapper.Map<User>(userDto));
            return Ok(_mapper.Map<PrizeDto>(price));
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<PrizeDto>>> GetAllPrices()
        {
            var prices = _mapper.Map<IReadOnlyCollection<PrizeDto>>(await _gameService.GetAllPrices());
            return Ok(prices);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<PrizeDto>> GetPrizeById(int pid) {
            var prices = _mapper.Map<PrizeDto>(await _gameService.GetPrice(pid));
            return Ok(prices);
        }

        [Route("[action]")]
        [HttpPut]
        public async Task<ActionResult<PrizeDto>> ConfigurePrice(PrizeDto prizeDto)
        {
            var price = await _gameService.UpdatePrice(_mapper.Map<Prize>(prizeDto));

            return Ok(_mapper.Map<PrizeDto>(price));
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<FileStreamResult> ExportUserResults()
        {
            var csv = await _gameService.GetUsersAsCsv();

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));
            return new FileStreamResult(stream, new MediaTypeHeaderValue("text/plain"))
            {
                FileDownloadName = "users.csv",
            };
        }


    }
}
