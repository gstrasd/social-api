﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Domain
{
    public interface IInstagramService
    {
        Task<string> GetPostHtmlAsync(Uri postUrl);
    }
}
