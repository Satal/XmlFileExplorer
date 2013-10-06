﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlFileExplorer.Domain.Validation
{
    public class ValidationError
    {
        public string Description { get; set; }
        public ErrorSeverity ErrorSeverity { get; set; }
    }
}
