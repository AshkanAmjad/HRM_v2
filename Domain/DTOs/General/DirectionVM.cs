using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.General
{
    public class DirectionVM
    {
        public int? Area { get; set; }
        public int? County { get; set; }
        public int? District { get; set; }
        public string? Name { get; set; }

        public string? _saveDirOrginal;

        public string? _saveDirThumb;
    }
}
