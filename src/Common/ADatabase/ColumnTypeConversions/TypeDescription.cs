using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ADatabase.Exceptions;

namespace ADatabase
{
    public class TypeDescription : ITypeDescription
    {
        private static readonly Regex TypeRegex = new Regex(@"(?<type>\w+)(\(@(?<first>Length|Prec|Scale)(,@(?<second>Length|Prec|Scale))?\))?");
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

        public bool Validate(string sourceType, int? length, int? prec, int? scale)
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

            return false;
        }

        public string GetDestinationString(int? length, int? prec, int? scale)
        {
            var result = ConvertTo;
            if (ConvertToParameters.ContainsKey("Length"))
            {
                if (length == null) throw new AColumnTypeException($"No Length value given for destination type '{ConvertTo}'");
                return ConvertTo + $"({length})";
            }
            if (ConvertToParameters.ContainsKey("Prec"))
            {
                if (prec == null) throw new AColumnTypeException($"No Precision value given for destination type '{ConvertTo}'");
                if (!ConvertToParameters.ContainsKey("Scale")) throw new AColumnTypeException($"No Precision value given for destination type '{ConvertTo}'");
                return ConvertTo + $"({length})";
            }
            return ConvertTo;
        }

        private static string GetParameters(IDictionary<string, int> parameters, string value)
        {
            var match = TypeRegex.Match(value);
            if (!match.Success) throw new AColumnTypeException($"Illegal type: '{value}'");
            var convertTo = match.Groups["type"].Value;
            if (match.Groups["first"].Success) parameters.Add(match.Groups["first"].Value, -99);
            if (match.Groups["second"].Success) parameters.Add(match.Groups["second"].Value, -99);

            if (parameters.Count == 0) return convertTo;
            if (parameters.ContainsKey("Length") && parameters.Count == 1) return convertTo;
            if (parameters.ContainsKey("Prec") && parameters.ContainsKey("Scale") && parameters.Count == 2) return convertTo;

            throw new AColumnTypeException($"Illegal parameter or parameter combination for type: '{value}', parameters: '{string.Join(",", parameters.Keys)}'");
        }
    }
}