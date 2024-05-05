# SourceCodeAnalysis.v2

## Phase 0

1. implementation of version of SourceCodeAnalysis app on .NET 8.0 **(implemented)**
1. implementation of app configuring via config file in XML format **(implemented)**
1. implementation of manage different analyzers via config file **(implemented)**
1. implementation of retrieving info about all known analyzers for app via special key in CLI

## Phase 1

1. implementation of analyzer for detecting (as error by default) C# 7.1 target-typed default literals **(implemented)**
1. implementation of analyzer for detecting (as error by default) C# 7.0 out inline variables **(implemented)**
1. implementation of analyzer for detecting (as error by default) string interpolation expressions **(implemented)**
1. implementation of analyzer for detecting (as error by default) expression-bodied members **(implemented)**
1. implementation of analyzer for detecting (as error by default) null-conditional operators **(implemented)**
1. implementation of analyzer for detecting (as error by default) auto-implemented properties **(implemented)**
1. implementation of analyzer for detecting (as error by default) object initializer expressions **(implemented)**
1. implementation of analyzer for detecting (as error by default) null-coalescing operators **(implemented)**
1. implementation of analyzer for detecting (as error by default) chained assignments **(implemented)**
1. implementation of analyzer for detecting (as error by default) generic methods that call generic methods of successors **(implemented)**
1. implementation of analyzer for detecting (as error by default) explicit implementation of an interface in the some class with a private method of the same name **(implemented)**
1. implementation of analyzer for detecting nameof expressions **(implemented)**

## Phase 2

1. implementation of  info collector about implementation of classes, methods, properties implementation in our implementation of system library
1. implementation of analyzer for detecting (as warning by default) usage of classes, methods and properties which are absent or not implemented in the our implementation of system library
1. implementation of support for ignored files via config file
1. implementation of multiple sources support via config file


## Usage of app

#### show help:
```
<app> --help
```
#### show version:
```
<app> --version
```
#### run analysis:
```
<app> --config=<path-to_config>
```
Config file has the following form:
```
<Config>
  <BaseConfig>
    <Source>(Path-to-project|Path-to-solution)</Source>
    <OutputLevel>(Info|Warning|Error)</OutputLevel>
  </BaseConfig>
  <Analyzers>
    <Analyzer>
      <Name>SourceCodeCheckApp.Analyzers.BadFilenameCaseAnalyzer</Name>
      <State>(On|ErrorAsWarning|Off)</State>
    </Analyzer>
    <Analyzer>
      <Name>SourceCodeCheckApp.Analyzers.CastToSameTypeAnalyzer</Name>
      <State>(On|ErrorAsWarning|Off)</State>
    </Analyzer>
    <Analyzer>
      <Name>SourceCodeCheckApp.Analyzers.NonAsciiIdentifiersAnalyzer</Name>
      <State>(On|ErrorAsWarning|Off)</State>
    </Analyzer>
    <Analyzer>
      <Name>SourceCodeCheckApp.Analyzers.StringInterpolationExprAnalyzer</Name>
      <State>(On|ErrorAsWarning|Off)</State>
    </Analyzer>
    <Analyzer>
      <Name>SourceCodeCheckApp.Analyzers.DefaultLiteralAnalyzer</Name>
      <State>(On|ErrorAsWarning|Off)</State>
    </Analyzer>
    <Analyzer>
      <Name>SourceCodeCheckApp.Analyzers.ObjectInitializerExprAnalyzer</Name>
      <State>(On|ErrorAsWarning|Off)</State>
    </Analyzer>
    <Analyzer>
      <Name>SourceCodeCheckApp.Analyzers.AutoImplPropertiesAnalyzer</Name>
      <State>(On|ErrorAsWarning|Off)</State>
    </Analyzer>
    <Analyzer>
      <Name>SourceCodeCheckApp.Analyzers.ExprBodiedMemberAnalyzer</Name>
      <State>(On|ErrorAsWarning|Off)</State>
    </Analyzer>
    <Analyzer>
      <Name>SourceCodeCheckApp.Analyzers.NullCoalescingOperatorAnalyzer</Name>
      <State>(On|ErrorAsWarning|Off)</State>
    </Analyzer>
    <Analyzer>
      <Name>SourceCodeCheckApp.Analyzers.NullConditionalOperatorAnalyzer</Name>
      <State>(On|ErrorAsWarning|Off)</State>
    </Analyzer>
    <Analyzer>
      <Name>SourceCodeCheckApp.Analyzers.OutInlineVariableAnalyzer</Name>
      <State>(On|ErrorAsWarning|Off)</State>
    </Analyzer>
    <Analyzer>
      <Name>SourceCodeCheckApp.Analyzers.ChainedAssignmentAnalyzer</Name>
      <State>(On|ErrorAsWarning|Off)</State>
    </Analyzer>
    <Analyzer>
      <Name>SourceCodeCheckApp.Analyzers.ExplicitInterfaceMethodDuplicationAnalyzer</Name>
      <State>(On|ErrorAsWarning|Off)</State>
    </Analyzer>
    <Analyzer>
      <Name>SourceCodeCheckApp.Analyzers.SuccessorGenericMethodCallAnalyzer</Name>
      <State>(On|ErrorAsWarning|Off)</State>
    </Analyzer>
    <Analyzer>
      <Name>SourceCodeCheckApp.Analyzers.NameOfExprAnalyzer</Name>
      <State>(On|ErrorAsWarning|Off)</State>
    </Analyzer>
  </Analyzers>
</Config>
```

## Implemented analyzers

1. **SourceCodeCheckApp.Analyzers.BadFilenameCaseAnalyzer** analyzer checks if file has the type with name that match the filename without extension. If there is no such type, but there are types with names which mismatch by letter case only, than such situation are considered as error. If file contains type, which matches by name and types which mismatch by letter case only, than such situation are considered as warning. If file contains only types which mismatch by name, than such situation are considered as warning.
1. **SourceCodeCheckApp.Analyzers.CastToSameTypeAnalyzer** analyzer finds cast expression with type `T` to the same type `T`. Some casts are considered as errors (e.g. cast to `string`), other - as warnings.
1. **SourceCodeCheckApp.Analyzers.NonAsciiIdentifiersAnalyzer** analyzer finds all identifiers with non ASCII letters in their names. All such identifiers are considered as errors.
1. **SourceCodeCheckApp.Analyzers.StringInterpolationExprAnalyzer** analyzer finds all string interpolation expressions. All such expressions are considered as errors.
1. **SourceCodeCheckApp.Analyzers.DefaultLiteralAnalyzer** analyzer finds all target-typed default literals. All such literals are considered as errors.
1. **SourceCodeCheckApp.Analyzers.ObjectInitializerExprAnalyzer** analyzer finds all object initializer expressions. All such expressions are considered as errors.
1. **SourceCodeCheckApp.Analyzers.AutoImplPropertiesAnalyzer** analyzer finds all auto-implemented properties. All such properties are considered as errors.
1. **SourceCodeCheckApp.Analyzers.ExprBodiedMemberAnalyzer** analyzer finds all expression-bodied members. All such members are considered as errors.
1. **SourceCodeCheckApp.Analyzers.NullCoalescingOperatorAnalyzer** analyzer finds all null-coalescing operators. All such operators are considered as errors.
1. **SourceCodeCheckApp.Analyzers.NullConditionalOperatorAnalyzer** analyzer finds all null-conditional operators. All such operators are considered as errors.
1. **SourceCodeCheckApp.Analyzers.OutInlineVariableAnalyzer** analyzer finds out inline variables. All such variables are considered as errors.
1. **SourceCodeCheckApp.Analyzers.ChainedAssignmentAnalyzer** analyzer finds chained assignment expressions. All such expressions are considered as errors.
1. **SourceCodeCheckApp.Analyzers.ExplicitInterfaceMethodDuplicationAnalyzer** analyzer finds explicit implementations of an interface in the some class with a private methods of the same name. All such implementations are considered as errors.
1. **SourceCodeCheckApp.Analyzers.SuccessorGenericMethodCallAnalyzer** analyzer finds calls of generic methods of successors from generic methods of ancestors. All such calls are considered as errors.
1. **SourceCodeCheckApp.Analyzers.NameOfExprAnalyzer** analyzer finds nameof expressions. All such expressions are considered as errors.