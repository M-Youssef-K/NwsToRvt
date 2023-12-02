using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NwsToRvt.ConvertUtils
{
    public class ConvertToFeetUtils
    {

        public static double ConvertMetersToFeet(double Value)
        {
            return Value * 3.28084;
        }

        public static double ConvertCentimetersToFeet(double Value) 
        {
            return Value * 0.0328084;
        }

        public static double ConvertMillimetersToFeet(double Value)
        {
            return Value * 0.00328084;
        }

    }
}
