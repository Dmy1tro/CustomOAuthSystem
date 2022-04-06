using System.Collections.Generic;
using System.Linq;

namespace ComplexSystem.Authorization.Services.Models.ClientStore
{
    public class Client
    {
        public string Id { get; set; }

        public string SecretSHA256 { get; set; }

        public ICollection<ClientPermission> Permissions { get; set; } = new List<ClientPermission>();

        public bool HasAnyPermissions(string resource)
        {
            return Permissions.Any(p => p.Permission.Split('.').First() == resource);
        }
    }
}
