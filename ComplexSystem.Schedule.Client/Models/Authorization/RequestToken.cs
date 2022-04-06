using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplexSystem.Schedule.Client.Models.Authorization
{
    class RequestToken
    {
        public string ClientId { get; set; }

        public string Secret { get; set; }

        public string Resource { get; set; }
    }
}
