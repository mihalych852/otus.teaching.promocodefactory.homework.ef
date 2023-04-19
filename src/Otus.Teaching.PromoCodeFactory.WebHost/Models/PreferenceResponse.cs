using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Models
{
    public class PreferenceResponse
    {
        public string Name { get; set; }

        public Guid PromoCodesId { get; set; }

    }
}
