using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.Extensions
{
    public static class Reflection
    {
        public static void Verify<T>(this ModelBuilder modelBuilder, params Assembly[] assemblies)
        {
            IEnumerable<Type> types = assemblies
                .SelectMany(x => x.GetExportedTypes())
                .Where(
                c => c.IsClass
                &&
                !c.IsAbstract
                &&
                c.IsPublic
                &&
                typeof(T).IsAssignableFrom(c));

            foreach (var type in types)
            {
                modelBuilder.Entity(type);
            }
        }
    }
}
