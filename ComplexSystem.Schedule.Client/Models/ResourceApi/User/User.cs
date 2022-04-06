using System.Collections.Generic;

namespace ComplexSystem.Schedule.Client.Models.ResourceApi.User
{
    internal class User
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public ICollection<Gallery.Gallery> Galleries { get; set; }
    }
}