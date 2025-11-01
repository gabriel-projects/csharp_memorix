using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.Memorix.Domain.Models
{
    public class JwtResultModel
    {
        public string AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public double Expire { get; set; }
        public string Type { get; set; }
    }
}
