using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public class Geom
    {
        public static double precision = 1;

        public static double GetPrecision()
        {
            return precision;
        }

        public static void SetPrecision(double precisionToSet)
        {
            precision = precisionToSet;
        }
    }

}
