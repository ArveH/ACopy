using System.Collections.Generic;

namespace ADatabase
{
    public class TypeDescription : ITypeDescription
    {
        public string TypeName { get; set; }
        public string TypeTo { get; set; }
        public List<ITypeConstraint> Constraints { get; set; }
    }
}