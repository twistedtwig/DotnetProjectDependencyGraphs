Project Dependency Graph
========================

tool to display project dependencies with a CS Project file or solution.


What it does
------------

The application is designed to show what references a project or solution has, i.e. what are the other projects and certain project or solution file references.  It doesn't include external DLL files, only CS projects.

The application has a number of outputs available.  Currently they all work around YUML, (http://www.yuml.me).


How to use the application
--------------------------

The application can take a number of command line arguments:

- leveltodig
- outputfolder
- rootfile
- outputtype
- loglevel
- logfolder
- logfile
- logtype

A quick example: 

```
ProjectReferences.Console.exe -rootfile "c:\work\myprojectfile.csproj" -outputfolder "C:\temp\projectReferences" -outputtype YumlImage -loglevel High
```

Some properties can be set via the configuration file:

- OutputFolder
- OutputFile
- LoggerType
- LoggerLevel

