﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Comment : BaseEntity
    {
        public string FullName { get; set; }
        public string Content { get; set; }
        public bool Show { get; set; } = false;
    }
}
