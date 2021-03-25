using Application.DTOs.BasicData;
using Application.Enums; 
using Application.Features;
using Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class BasicData : BaseApiController
    {
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(await Mediator.Send(new GetAllRolesQuery()));
        }

        [HttpGet("GetAlHomeWorkSubmitionStatus")]
        public async Task<IActionResult> GetAlHomeWorkSubmitionStatus()
        {
            return Ok(await Mediator.Send(new GetAllHomeworkSubmtionStatusEnumQuery()));
        }

        [HttpGet("GetSingleQuestionTypes")]
        public async Task<IActionResult> GetSingleQuestionTypes()
        {
            return Ok(await Mediator.Send(new GetSingleQuestionTypesQuery()));
        }

        [HttpGet("getDateTime")]
        public IActionResult getDateTime()
        {
            return Ok(DateTime.Now.ToString());
        }


        [HttpGet("GetAllCountries")]
        public IActionResult GetAllCountries()
        {
            List<string> culuterList = new List<string>();
            CultureInfo[] cultureInfos = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            IEnumerable<RegionInfo> AllRegionInfo =
               CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                   .Select(culture => new RegionInfo(culture.Name)) // using Name instead of LCID
                   .ToList();

            foreach (RegionInfo item in AllRegionInfo)
            { 
                if (!(culuterList.Contains(item.EnglishName)))
                {
                    culuterList.Add(item.EnglishName);
                }
            }
            culuterList.Sort();
            return Ok(culuterList);
        }

        [HttpGet("GetAllForumTypes")]
        public async Task<IActionResult> GetForumTypes()
        {
            return Ok(await Mediator.Send(new GetAllForumTypesQuery()));
        }
    }
}
