using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace slotmachine_api.Models.Settings
{
    public class JwtSettings : IJwtSettings
    {
        public string JwtKey { get; set; }

        public string JwtIssuer { get; set; }

        public int JwtExpireDays { get; set; }
    }

    public interface IJwtSettings
    {
        public string JwtKey { get; set; }

        public string JwtIssuer { get; set; }

        public int JwtExpireDays { get; set; }
    }
}
