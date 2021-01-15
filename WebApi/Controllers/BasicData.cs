using Application.DTOs.BasicData;
using Application.Enums;
using Application.Interfaces;
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
        public async Task<IActionResult> getDateTime()
        {
            return Ok(DateTime.Now.ToString());
        }


        [HttpGet("GetAllCountries")]
        public async Task<IActionResult> GetAllCountries()
        {
            List<string> culuterList = new List<string>();
            CultureInfo[] cultureInfos = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo item in cultureInfos)
            {
                RegionInfo regionInfo = new RegionInfo(item.LCID);
                if (!(culuterList.Contains(regionInfo.EnglishName)))
                {
                    culuterList.Add(regionInfo.EnglishName);
                }
            }
            culuterList.Sort();
            return Ok(culuterList);
        }
        
        [Produces("application/json")]
        [Route("api/audio")]
        [HttpPost]
        public async Task<IActionResult> ProcessCommandAsync([FromForm] IFormFile command)
        {
            if (command.ContentType != "audio/wav" && command.ContentType != "audio/wave" || command.Length < 1)
            {
                return BadRequest();
            }
            var text = await CovnvertSpeechToTextApiCall(ConvertToByteArrayContent(command));

            return Ok(FormulateResponse(text));
        }


        private ByteArrayContent ConvertToByteArrayContent(IFormFile audofile)
        {
            byte[] data;

            using (var br = new BinaryReader(audofile.OpenReadStream()))
            {
                data = br.ReadBytes((int)audofile.OpenReadStream().Length);
            }

            return new ByteArrayContent(data);
        }
    }
}
