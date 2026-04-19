@echo off
REM Wrapper around `dotnet build` that enables MSBuild's static project-graph
REM algorithm. Without --graph, MSBuild can schedule the same project on two
REM parallel build nodes and they race on GenerateDepsFile (Wizards of SimPe
REM has historically tripped this). --graph dedupes those evaluations.
REM
REM Usage:
REM   build              -> Debug build
REM   build Release      -> Release build
REM   build Release -m:1 -> Release build, single-threaded (fallback)
REM
REM All extra args are forwarded to `dotnet build`.

setlocal
set CONFIG=Debug
if not "%~1"=="" (
  set CONFIG=%~1
  shift
)

set EXTRA=
:loop
if "%~1"=="" goto done
set EXTRA=%EXTRA% %1
shift
goto loop
:done

dotnet build "%~dp0SimPE-Fixed.sln" -c %CONFIG% --graph %EXTRA%
exit /b %ERRORLEVEL%
