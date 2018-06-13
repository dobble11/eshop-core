using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Collections.Generic;

namespace EShop.Controllers
{
    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        private readonly IHostingEnvironment _env;

        public UploadController(IHostingEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        public IActionResult Post(IFormFile[] files)
        {
            var id = Guid.NewGuid().ToString();
            var names = new List<string>();
            var path = Path.Combine(_env.WebRootPath, "assets", id);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            for (int i = 0; i < files.Length; i++)
            {
                var ext = Path.GetExtension(files[i].FileName);

                using (FileStream fs = System.IO.File.Create($"{path}/{i}{ext}"))
                {
                    files[i].CopyTo(fs);
                    fs.Flush();
                }
                names.Add($"{i}{ext}");
            }

            return Ok(new { id, names });
        }
    }
}
