using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class NumberGenerator
    {
        public static string RandomNumber()
        {
            var generator = new Random();
            var result = generator.Next(0, 100000).ToString("D5");
            return result;
        }
    }
}
