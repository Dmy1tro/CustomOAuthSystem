using ComplexSystem.Common.Resources;

namespace ComplexSystem.Authorization.Services.Models.ClientStore
{
    public class ClientPermission
    {
        public string Resource { get; set; }

        public string Permission { get; set; }

        public Scope Scope { get; set; }
    }
}
