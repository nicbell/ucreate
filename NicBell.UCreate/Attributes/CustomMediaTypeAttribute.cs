﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicBell.UCreate.Attributes
{
    public class CustomMediaTypeAttribute : OrderdSyncAttribute
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Alias { get; set; }
        public string Icon { get; set; }
        public bool AllowedAsRoot { get; set; }
        public bool IsContainer { get; set; }
    }
}