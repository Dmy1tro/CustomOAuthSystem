using System;
using ComplexSystem.Authorization.Services.Interfaces;
using ComplexSystem.Authorization.Services.Models.ClientStore;

namespace ComplexSystem.Authorization.Services.Services
{
    internal class ClientStore : IClientStore
    {
        private readonly IStore _store;

        public ClientStore(IStore store)
        {
            _store = store;
        }

        public void AddClient(Client client)
        {
            _store.SetItem(client.Id, client);
        }

        public Client? GetClient(string id)
        {
            return _store.GetItem<Client>(id);
        }
    }
}
