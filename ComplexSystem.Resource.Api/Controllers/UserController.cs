using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using ComplexSystem.Common.CustomAuthorization;
using ComplexSystem.Resource.Api.Policies;
using Microsoft.AspNetCore.Mvc;
using static ComplexSystem.Resource.Api.Controllers.GalleryController;

namespace ComplexSystem.Resource.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public class UserModel
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public int Age { get; set; }

            public ICollection<Gallery> Galleries { get; set; }
        }

        public static readonly ConcurrentDictionary<string, UserModel> UserStore = new()
        {
            ["198fc671-d84d-42e4-88d3-913af4290b11"] =
            new UserModel
            {
                Id = "198fc671-d84d-42e4-88d3-913af4290b11",
                Name = "Billy",
                Age = 14 + 1,
                Galleries = new List<Gallery>()
            }
        };

        [HttpGet("{id}")]
        [CustomAuthCheck(PolicyName.UserReadOnly)]
        public async Task<IActionResult> Get(string id)
        {
            if (!UserStore.ContainsKey(id))
            {
                return BadRequest($"User with id '{id}' was not found.");
            }

            UserStore.TryGetValue(id, out var user);

            return Ok(user);
        }
    }
}
