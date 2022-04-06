using System.Collections.Concurrent;
using System.Threading.Tasks;
using ComplexSystem.Common.CustomAuthorization;
using ComplexSystem.Common.Resources;
using ComplexSystem.Resource.Api.Policies;
using Microsoft.AspNetCore.Mvc;

namespace ComplexSystem.Resource.Api.Controllers
{
    [Route("api/gallery")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        public class Gallery
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string UserId { get; set; }
        }

        public static readonly ConcurrentDictionary<string, Gallery> GalleryStore = new();

        [HttpGet("{id}")]
        // Normal people use [Authorize] instead of inventing a bicycle =)))
        [CustomAuthCheck(PolicyName.GalleryReadOnly)]
        public async Task<IActionResult> Get(string id)
        {
            if (!GalleryStore.ContainsKey(id))
            {
                return BadRequest($"Gallery with id '{id}' was not found.");
            }

            GalleryStore.TryGetValue(id, out var gallery);

            return Ok(gallery);
        }

        [HttpPost("")]
        [CustomAuthCheck(PolicyName.GalleryWriteOnly)]
        public async Task<IActionResult> Create([FromBody] Gallery gallery)
        {
            UserController.UserStore.TryGetValue(gallery.UserId, out var user);

            user.Galleries.Add(gallery);
            GalleryStore.TryAdd(gallery.Id, gallery);

            return Ok(gallery);
        }
    }
}
