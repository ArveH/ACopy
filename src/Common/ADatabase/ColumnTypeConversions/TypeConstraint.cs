using System;

namespace ADatabase
{
    public class TypeConstraint : ITypeConstraint
    {
        public TypeConstraint(string constraintType, ITypeOperator op)
        {
            ConstraintType = Convert(constraintType);
            Operator = op;
        }

        public ConstraintTypeName ConstraintType { get; }
        public ITypeOperator Operator { get;  }

        private ConstraintTypeName Convert(string txt)
        {
            switch (txt)
            {
                case "Precision": return ConstraintTypeName.Prec;
                case "Scale": return ConstraintTypeName.Scale;
                case "Length": return ConstraintTypeName.Length;
            }

            throw new ArgumentException($"Can't convert '{txt}' to ConstraintTypeName. Legal values are 'Precision', 'Scale', 'Length'");
        }
    }
}