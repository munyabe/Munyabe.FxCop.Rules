Munyabe.FxCop.Rules
===================

This is FxCop custom rules, that checks .NET assemblies. FxCop is a static code analysis tool from Microsoft.

[![Build status](https://ci.appveyor.com/api/projects/status/mqqjutacul7y70g2)](https://ci.appveyor.com/project/munyabe/munyabe-fxcop-rules)

## How to build

You need to have following assemblies.
- <b>FxCopSdk.dll</b>
- <b>Microsoft.Cci.dll</b>

The version of committed assemblies is FxCop 10.0. If you want to use the other version, you need to correct the project references.

Note: These assemblies are usually located in following directories on 64bit OS.
- FxCop 10.0
  - C:\Program Files (x86)\Microsoft Fxcop 10.0
- Visual Studio 2013
  - C:\Program Files (x86)\Microsoft Visual Studio 12.0\Team Tools\Static Analysis Tools\FxCop

## How to run

See also [Code Analysis Team Blog](http://blogs.msdn.com/b/codeanalysis/archive/2010/03/26/how-to-write-custom-static-code-analysis-rules-and-integrate-them-into-visual-studio-2010.aspx).
