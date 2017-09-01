using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Models
{
    public class ChangelogsListPartialModel
    {
        public List<Changelog> Changelogs { get; set; }
        public int assetid { get; set; }
    }
}