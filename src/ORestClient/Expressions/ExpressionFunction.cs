using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ORestClient.Expressions {
    public class ExpressionFunction {
        public string FunctionName { get; set; }
        public List<ODataExpression> Arguments { get; set; }

        public class FunctionCall {
            public string FunctionName { get; }
            public int ArgumentCount { get; }

            public FunctionCall(string functionName, int argumentCount) {
                FunctionName = functionName;
                ArgumentCount = argumentCount;
            }

            public override bool Equals(object obj) {
                if (obj is FunctionCall call) {
                    return FunctionName == call.FunctionName &&
                           ArgumentCount == call.ArgumentCount;
                }
                return base.Equals(obj);
            }

            public override int GetHashCode() {
                return FunctionName.GetHashCode() ^ ArgumentCount.GetHashCode();
            }
        }

        internal ExpressionFunction() {
        }

        public ExpressionFunction(string functionName, IEnumerable<object> arguments) {
            FunctionName = functionName;
            Arguments = arguments.Select(ODataExpression.FromValue).ToList();
        }

        public ExpressionFunction(string functionName, IEnumerable<Expression> arguments) {
            FunctionName = functionName;
            Arguments = arguments.Select(ODataExpression.FromLinqExpression).ToList();
        }
    }
}