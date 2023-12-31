﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApp.Model.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }  
        public string Name { get; set; }  
        public string Address { get; set; }  
        public DateTime CreatedAt { get; set; }  
        public string ImageURL { get; set; }
        public string PhoneNumber { get; set; }
    }
}
