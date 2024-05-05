using SourceCodeCheckApp.Config;
using SourceCodeCheckApp.Output;

namespace SourceCodeCheckApp.Analyzers
{
    internal static class AnalyzersFactory
    {
        public static IList<IFileAnalyzer> Create(IOutput output, AnalyzerEntry[] config)
        {
            IDictionary<String, AnalyzerState> analyzersMap = config.ToDictionary(entry => entry.Name!, entry => entry.State);
            AnalyzerState GetAnalyzerState(String name) => analyzersMap.TryGetValue(name, out var state) ? state : AnalyzerState.Off;
            return new IFileAnalyzer[]
            {
                new AutoImplPropertiesAnalyzer(output, GetAnalyzerState(AutoImplPropertiesAnalyzer.Name)),
                new BadFilenameCaseAnalyzer(output, GetAnalyzerState(BadFilenameCaseAnalyzer.Name)),
                new CastToSameTypeAnalyzer(output, GetAnalyzerState(CastToSameTypeAnalyzer.Name)),
                new ChainedAssignmentAnalyzer(output, GetAnalyzerState(ChainedAssignmentAnalyzer.Name)),
                new DefaultLiteralAnalyzer(output, GetAnalyzerState(DefaultLiteralAnalyzer.Name)),
                new ExplicitInterfaceMethodDuplicationAnalyzer(output, GetAnalyzerState(ExplicitInterfaceMethodDuplicationAnalyzer.Name)),
                new ExprBodiedMemberAnalyzer(output, GetAnalyzerState(ExprBodiedMemberAnalyzer.Name)),
                new NameOfExprAnalyzer(output, GetAnalyzerState(NameOfExprAnalyzer.Name)),
                new NonAsciiIdentifiersAnalyzer(output, GetAnalyzerState(NonAsciiIdentifiersAnalyzer.Name)),
                new NullCoalescingOperatorAnalyzer(output, GetAnalyzerState(NullCoalescingOperatorAnalyzer.Name)),
                new NullConditionalOperatorAnalyzer(output, GetAnalyzerState(NullConditionalOperatorAnalyzer.Name)),
                new ObjectInitializerExprAnalyzer(output, GetAnalyzerState(ObjectInitializerExprAnalyzer.Name)),
                new OutInlineVariableAnalyzer(output, GetAnalyzerState(OutInlineVariableAnalyzer.Name)),
                new SuccessorGenericMethodCallAnalyzer(output, GetAnalyzerState(SuccessorGenericMethodCallAnalyzer.Name)),
                new StringInterpolationExprAnalyzer(output, GetAnalyzerState(StringInterpolationExprAnalyzer.Name))
            };
        }
    }
}