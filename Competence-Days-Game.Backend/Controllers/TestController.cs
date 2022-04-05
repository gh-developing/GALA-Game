using AutoMapper;
using Bystronic;
using Compentence_Days_Game.Backend.Database;
using Compentence_Days_Game.Backend.Database.Entities;
using Compentence_Days_Game.Backend.Interfaces;
using Compentence_Days_Game.Backend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Compentence_Days_Game.Backend.Controller
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        private IPriceGenerator _priceGenerator;

        public TestController(
            IPriceGenerator priceGenerator)
        {
            _priceGenerator = priceGenerator;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<Dictionary<string, int>>> GeneratePriceFor(int userCount)
        {
            var results = new Dictionary<string, int>();

            Prize price;

            for(var i = 0; i < userCount; i++)
            {
                price = await _priceGenerator.GetNextPrice(false);
                if (!results.ContainsKey(price.Name)) {
                    results.Add(price.Name, 0);
                }

                results[price.Name]++;

            }
            
            return Ok(results);
        }
    }
}
