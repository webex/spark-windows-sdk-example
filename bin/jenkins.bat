
@echo OFF

if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin" set MSBUILDDIR=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin
echo %MSBUILDDIR%

pushd .
bin\nuget.exe restore solutions\KitchenSink\KitchenSink.sln -NonInteractive

"%MSBUILDDIR%\msbuild.exe" solutions\KitchenSink\KitchenSink.sln /t:Rebuild /p:Configuration="Debug" /p:Platform="x86"
if not %errorlevel% == 0 ( 
	echo build debug version failed!
	goto EXIT 
)

"%MSBUILDDIR%\msbuild.exe" solutions\KitchenSink\KitchenSink.sln /t:Rebuild /p:Configuration="Release" /p:Platform="x86"
if not %errorlevel% == 0 (
	echo build release version failed!
	goto EXIT
)

popd

:EXIT
echo error level: %errorlevel% 
EXIT /B %errorlevel%