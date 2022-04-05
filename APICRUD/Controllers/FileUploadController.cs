using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

namespace APICRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public FileUploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }


        // POST: api/FileUpload
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<IFormFile> files)
        {
            var uploadFolder = Path.Combine(_environment.WebRootPath, "files");

            foreach (var file in files)
                if (file.Length > 0)
                {
                    var fileName = Path.GetFileName(ContentDispositionHeaderValue.Parse(file.ContentDisposition)
                        .FileName.Trim('"'));

                    using (var fileStream = new FileStream(Path.Combine(uploadFolder, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

            return Ok( new { message = "OK"});
        }
    } 
}