﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Domain.ValueObjects
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public UserInfo(string name,Guid id,string role)
        {
            Name = name;
            Id = id;
            Role = role;
        }
    }
}
