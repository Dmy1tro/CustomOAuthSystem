using ComplexSystem.Authorization.Services.Models.ClientStore;

namespace ComplexSystem.Authorization.Services.Interfaces
{
    public interface IClientStore
    {
        void AddClient(Client client);

        Client? GetClient(string id);
    }
}
