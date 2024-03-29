﻿using System;
using System.Collections.Generic;

namespace LogicLibrary.Models
{
    public interface IResult
    {
        string Id { get; set; }
        string Name { get; set; }
        public Location Location { get; set; }
        List<string> AutoSuggestions { get; }
    }
}
