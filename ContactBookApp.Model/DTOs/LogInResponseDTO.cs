using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApp.Model.DTOs
{
    public class LogInResponseDTO
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string ErrorMessage { get; set; }
    }
}
