using Amazon.S3;
using Amazon.S3.Transfer;
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
        public static string bucketName = "adler-audio-files";
        public static string endpoingURL = "https://fra1.digitaloceanspaces.com";
        public static IAmazonS3 s3Client;

        public ListeningAudioFileController(IWebHostEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        public static bool UploadFile(Stream file, string fileName, string folderName)
        {
            var s3ClientConfig = new AmazonS3Config
            {
                ServiceURL = endpoingURL
            };
            s3Client = new AmazonS3Client(s3ClientConfig);
            try
            {
                var fileTransferUtility = new TransferUtility(s3Client);
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName + @"/" + folderName,
                    StorageClass = S3StorageClass.StandardInfrequentAccess,
                    PartSize = 6291456, // 6 MB
                    Key = fileName,
                    
                    CannedACL = S3CannedACL.PublicReadWrite,
                    InputStream = file,
                };
                fileTransferUtility.Upload(fileTransferUtilityRequest);
                return true;
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                if (e.Message.Contains("disposed"))
                    return true;
            }
            return false;
        }

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
            try
            {

                CreateListeningAudioFileCommand command = new CreateListeningAudioFileCommand();
                command.FileName = file.FileName;
                var uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "ListeningAudioFiles");
                // if (!Directory.Exists(uploads))
                // {
                //     Directory.CreateDirectory(uploads);
                // }
                if (file.Length > 0)
                {
                    command.FilePath = Path.Combine(uploads, command.FileName);
                    using (var fileStream = file.OpenReadStream())
                    {
                        // await file.CopyToAsync(fileStream);
                        UploadFile(fileStream, file.FileName, "ListeningAudioFiles");
                    }
                }
                return Ok(await Mediator.Send(command));
            }
            catch (Exception ex)
            {
                return NotFound();
            }

        }

    }
}
