﻿using ShipShareAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipShareAPI.Domain.Entities
{
    public class SenderPost : Post
    {
        public List<string>? ItemPhotos { get; set; }
        public string ItemType { get; set; } = null!;
        public float ItemWeight { get; set; }

    }
}
