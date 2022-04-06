using System.Threading.Tasks;
using ComplexSystem.Schedule.Client.Models.ResourceApi.Gallery;
using Refit;

namespace ComplexSystem.Schedule.Client.RefitClients.ResourceApi
{
    interface IGalleryApiClient
    {
        [Post("/gallery")]
        Task<ApiResponse<Gallery>> CreateGallery(Gallery gallery);

        [Get("/gallery/{id}")]
        Task<ApiResponse<Gallery>> GetGallery(string id);
    }
}
