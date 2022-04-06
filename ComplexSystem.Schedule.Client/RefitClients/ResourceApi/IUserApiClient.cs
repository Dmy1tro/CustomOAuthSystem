using System.Threading.Tasks;
using ComplexSystem.Schedule.Client.Models.ResourceApi.User;
using Refit;

namespace ComplexSystem.Schedule.Client.RefitClients.ResourceApi
{
    interface IUserApiClient
    {
        [Get("/users/{id}")]
        public Task<ApiResponse<User>> GetUser(string id);
    }
}