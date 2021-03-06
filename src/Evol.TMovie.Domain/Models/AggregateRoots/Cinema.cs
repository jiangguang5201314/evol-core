﻿using Evol.Common;
using Evol.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evol.TMovie.Domain.Models.AggregateRoots
{
    public class Cinema : AggregateRoot
    {
        public string Name { get; set; }

        public string Address { get; set; }
    }
}
