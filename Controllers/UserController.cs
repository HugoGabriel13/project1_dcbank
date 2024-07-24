using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Data;
using TestApi.Models;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { user.Login }, user);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty");
            }

            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                var csvContent = await stream.ReadToEndAsync();
                var lines = csvContent.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
                var halfIndex = lines.Count / 2;

                var firstHalf = string.Join("\n", lines.Take(halfIndex));
                var secondHalf = string.Join("\n", lines.Skip(halfIndex));

                var result = new Dictionary<string, string>
                {
                    { "FirstHalf", firstHalf },
                    { "SecondHalf", secondHalf }
                };

                return Ok(result);
            }
        }
    }
}
