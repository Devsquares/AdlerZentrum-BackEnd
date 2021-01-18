using Application.Features;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class ListeningAudioFileController : BaseApiController
    {
        private IWebHostEnvironment _hostingEnvironment;
        public ListeningAudioFileController(IWebHostEnvironment environment)
        {
            _hostingEnvironment = environment;
        }
        //// GET: api/<controller>
        //[HttpGet]
        //public async Task<IActionResult> Get([FromQuery] GetAllListeningAudioFilesParameter filter)
        //{

        //    return Ok(await Mediator.Send(new GetAllListeningAudioFilesQuery()
        //    {
        //        PageSize = filter.PageSize,
        //        PageNumber = filter.PageNumber,
        //        FilterArray = filter.FilterArray,
        //        FilterRange = filter.FilterRange,
        //        FilterSearch = filter.FilterSearch,
        //        FilterValue = filter.FilterValue,
        //        SortBy = filter.SortBy,
        //        SortType = filter.SortType,
        //        NoPaging = filter.NoPaging
        //    }));
        //}

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var obj = Mediator.Send(new GetListeningAudioFileByIdQuery { Id = id }).Result;
            var uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "ListeningAudioFiles");
            var filePath = Path.Combine(uploads, obj.data.FileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), obj.data.FileName);
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        [HttpPost]
        //[Authorize(Roles = "SuperAdmin")] 
        public async Task<IActionResult> Post(IFormFile file)
        {
            CreateListeningAudioFileCommand command = new CreateListeningAudioFileCommand();
            command.FileName = file.Name + DateTime.Now;
            var uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "ListeningAudioFiles");
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }
            if (file.Length > 0)
            {
                command.FilePath = Path.Combine(uploads, command.FileName);
                using (var fileStream = new FileStream(command.FilePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return Ok(await Mediator.Send(command));
        }

        //// PUT api/<controller>/5
        //[HttpPut("{id}")]
        ////[Authorize(Roles = "SuperAdmin")]
        ////TODO: enable authorization
        //public async Task<IActionResult> Put(int id, UpdateListeningAudioFileCommand command)
        //{
        //    if (id != command.Id)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok(await Mediator.Send(command));
        //}

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        ////[Authorize(Roles = "SuperAdmin")]
        ////TODO: enable authorization
        //public async Task<IActionResult> Delete(int id)
        //{
        //    return Ok(await Mediator.Send(new DeleteListeningAudioFileByIdCommand { Id = id }));
        //}

    }
}
