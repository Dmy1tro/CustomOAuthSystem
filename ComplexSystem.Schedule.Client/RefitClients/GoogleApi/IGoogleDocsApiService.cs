using System.Threading.Tasks;
using ComplexSystem.Schedule.Client.Models.GoogleApi.Docs;
using Refit;

namespace ComplexSystem.Schedule.Client.RefitClients.GoogleApi
{
    interface IGoogleDocsApiService
    {
        [Get("/documents/{id}")]
        Task<ApiResponse<Document>> GetDocument(string id);

        [Post("/documents")]
        Task<ApiResponse<object>> CreateDocument(Document document);
    }
}
