using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace slotmachine_api.Models.Responses
{
    public class UserDataResponse
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public int Points { get; set; }
    }
}
