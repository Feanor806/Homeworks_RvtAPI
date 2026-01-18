using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI8_1_WallGeometryStatistics.Models
{
    public class AWall
    {
        public string WallName{ get; set; }
        public string WallType { get; set; }
        public double Length { get; set; }
        public double Height { get; set; }
        public double Thickness { get; set; }
        public double Volume { get; set; }
        public double Area { get; set; }
        public bool Status { get; set; }
    }
}
