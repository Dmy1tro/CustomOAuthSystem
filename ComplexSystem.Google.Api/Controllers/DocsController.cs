using System.Collections.Concurrent;
using System.Threading.Tasks;
using ComplexSystem.Common.CustomAuthorization;
using ComplexSystem.Google.Api.Policies;
using Microsoft.AspNetCore.Mvc;

namespace ComplexSystem.Google.Api.Controllers
{
    [Route("api/documents")]
    [ApiController]
    public class DocsController : ControllerBase
    {
        public class Document
        {
            public string Id { get; set; }

            public string Title { get; set; }
        }

        public static readonly ConcurrentDictionary<string, Document> DocumentStore = new();

        [HttpGet("{id}")]
        [CustomAuthCheck(PolicyName.DocsReadOnly)]
        public async Task<IActionResult> Get(string id)
        {
            if (!DocumentStore.ContainsKey(id))
            {
                return BadRequest($"Document with id '{id}' was not found.");
            }

            DocumentStore.TryGetValue(id, out var document);

            return Ok(document);
        }

        [HttpPost("")]
        [CustomAuthCheck(PolicyName.DocsWriteOnly)]
        public async Task<IActionResult> Create([FromBody] Document document)
        {
            DocumentStore.TryAdd(document.Id, document);

            return Ok(document);
        }
    }
}
