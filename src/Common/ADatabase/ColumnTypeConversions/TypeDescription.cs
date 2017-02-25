using System.Collections.Generic;
using System.Text.RegularExpressions;
using ADatabase.Exceptions;

namespace ADatabase
{
    public class TypeDescription : ITypeDescription
    {
        private static readonly Regex TypeRegex = new Regex(@"(?<type>\w+)(\(@?(?<first>Length|Prec|Scale|-?\d+)(, ??@?(?<second>Length|Prec|Scale|\d+))?\))?");

        private readonly ITypeConstraintFactory _typeConstraintFactory;

        public TypeDescription(ITypeConstraintFactory typeConstraintFactory)
        {
            _typeConstraintFactory = typeConstraintFactory;
        }

        private string _typeName;
        public string TypeName
        {
            get { return _typeName; }
            set { _typeName = GetParameters(TypeNameParameters, value); }
        }

        public Dictionary<string, int>TypeNameParameters { get; } = new Dictionary<string, int>();

        private string _convertTo;
        public string ConvertTo
        {
            get { return _convertTo; }
            set { _convertTo = GetParameters(ConvertToParameters, value); }
        }

        public Dictionary<string, int> ConvertToParameters { get; } = new Dictionary<string, int>();

        public List<ITypeConstraint> Constraints { get; } = new List<ITypeConstraint>();

        public void AddConstraint(string constraintType, string operatorName, int constraintValue)
        {
            AddConstraint(constraintType, operatorName, new []{constraintValue});
        }

        public void AddConstraint(string constraintType, string operatorName, IEnumerable<int> constraintValues)
        {
            var constraint = _typeConstraintFactory.GetTypeConstraint(constraintType, operatorName, constraintValues);
            Constraints.Add(constraint);
        }

        public bool Validate(string sourceType, int length, int prec, int scale)
        {
            var match = TypeRegex.Match(sourceType);
            if (!match.Success) return false;
            var type = match.Groups["type"].Value;
            if (type != _typeName) return false;

            if (Constraints.Count == 0) return true;

            if (match.Groups["first"].Value == "Length")
            {
                return Constraints[0].ConstraintType == "Length" && Constraints[0].IsWithinConstraint(length);
            }
            if (match.Groups["first"].Value == "Prec" && match.Groups["second"].Value == "Scale")
            {
                return Constraints.Count == 2
                       && Constraints[0].ConstraintType == "Prec" && Constraints[1].ConstraintType == "Scale"
                       && Constraints[0].IsWithinConstraint(prec) && Constraints[1].IsWithinConstraint(scale);
            }
            if (match.Groups["first"].Value == "Prec")
            {
                return Constraints.Count == 1
                       && Constraints[0].ConstraintType == "Prec"
                       && Constraints[0].IsWithinConstraint(prec);
            }
            if (match.Groups["first"].Value == "Scale")
            {
                return Constraints.Count == 1
                       && Constraints[0].ConstraintType == "Scale"
                       && Constraints[0].IsWithinConstraint(scale);
            }

            return false;
        }

        public string GetDestinationType(ref int length, ref int prec, ref int scale)
        {
            if (ConvertToParameters.ContainsKey("Length"))
            {
                if (length == 0 && ConvertToParameters["Length"] == -99) throw new AColumnTypeException($"No Length value given for destination type '{ConvertTo}'");
                if (ConvertToParameters["Length"] != -99) length = ConvertToParameters["Length"];
                prec = 0;
                scale = 0;
            }
            else if (ConvertToParameters.ContainsKey("Prec") && ConvertToParameters.ContainsKey("Scale"))
            {
                if (prec == 0 && ConvertToParameters["Prec"] == -99) throw new AColumnTypeException($"No Precision value given for destination type '{ConvertTo}'");
                if (scale == 0 && ConvertToParameters["Scale"] == -99) throw new AColumnTypeException($"No Scale value given for destination type '{ConvertTo}'");
                length = 0;
                if (ConvertToParameters["Prec"] != -99) prec = ConvertToParameters["Prec"];
                if (ConvertToParameters["Scale"] != -99) scale = ConvertToParameters["Scale"];
            }
            else if (ConvertToParameters.ContainsKey("Prec"))
            {
                if (prec == 0 && ConvertToParameters["Prec"] == -99) throw new AColumnTypeException($"No Precision value given for destination type '{ConvertTo}'");
                length = 0;
                scale = 0;
                if (ConvertToParameters["Prec"] != -99) prec = ConvertToParameters["Prec"];
            }
            else if (ConvertToParameters.ContainsKey("Scale"))
            {
                if (scale == 0 && ConvertToParameters["Scale"] == -99) throw new AColumnTypeException($"No Scale value given for destination type '{ConvertTo}'");
                length = 0;
                prec = 0;
                if (ConvertToParameters["Scale"] != -99) scale = ConvertToParameters["Scale"];
            }
            return ConvertTo;
        }

        private static string GetParameters(IDictionary<string, int> parameters, string value)
        {
            var match = TypeRegex.Match(value);
            if (!match.Success) throw new AColumnTypeException($"Illegal type: '{value}'");
            var convertTo = match.Groups["type"].Value;
            if (match.Groups["first"].Success && match.Groups["second"].Success)
            {
                var first = match.Groups["first"].Value;
                var second = match.Groups["second"].Value;
                int tmp;
                if (!int.TryParse(first, out tmp)) tmp = -99;
                parameters.Add("Prec", tmp);
                if (!int.TryParse(second, out tmp)) tmp = -99;
                parameters.Add("Scale", tmp);
            }
            else if (match.Groups["first"].Success)
            {
                int tmp;
                if (!int.TryParse(match.Groups["first"].Value, out tmp)) tmp = -99;
                parameters.Add(match.Groups["first"].Value, tmp);
            }

            return convertTo;
        }
    }
}