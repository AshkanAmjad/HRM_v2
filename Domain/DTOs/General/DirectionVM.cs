using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.General
{
    public class DirectionVM
    {
        public string? Area { get; set; }
        public string? County { get; set; }
        public string? District { get; set; }
        public string? Name { get; set; }

        public string? _saveDirOrginal;

        public string? _saveDirThumb;
    }
}
