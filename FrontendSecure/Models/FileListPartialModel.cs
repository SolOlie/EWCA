using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Entities;

namespace FrontendSecure.Models
{
    public class FileListPartialModel
    {
        public List<File> Files { get; set; }
        public int assetid { get; set; }
    }
}