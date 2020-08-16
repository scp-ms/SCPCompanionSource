using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSMSL;
using CSMSL.Analysis;
using CSMSL.Chemistry;
using CSMSL.IO;
using CSMSL.Proteomics;
using CSMSL.Spectral;
using CSMSL.Util;

namespace SCPCompanion
{
    public class MultiplexingLine
    {
        public string Name { get; set; }
        private Modification Modification { get; set; }
        public string Type { get; set; }

        public MultiplexingLine(Modification mod, string type = "Unused")
        {
            Name = mod.Name;
            Modification = mod;
            Type = type;
        }
    }
}
