
@echo OFF

if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin" set MSBUILDDIR=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin
if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\Common7\IDE" set DEVENVDIR=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\Common7\IDE
echo %MSBUILDDIR%
echo %DEVENVDIR%

pushd .
bin\nuget.exe restore solutions\KitchenSink\KitchenSink.sln -NonInteractive

set SDKNuGetPackage=Cisco.Spark.WindowsSDK.1.4.0-EFT01
echo SDKNuGetPackage is %SDKNuGetPackage%
echo copy scf libraries to solutions\KitchenSink\packages\%SDKNuGetPackage%\
copy /y spark-client-framework\scfLibrary\Release\*.dll solutions\KitchenSink\packages\%SDKNuGetPackage%\
if not %errorlevel% == 0 ( 
	echo update scf libraries failed.
	goto EXIT 
)
echo copy SparkSDK.dll to solutions\KitchenSink\packages\%SDKNuGetPackage%\
copy /y sdk\binary\[build]\bin\x86\Release\SparkSDK.dll solutions\KitchenSink\packages\%SDKNuGetPackage%\
if not %errorlevel% == 0 ( 
	echo update SparkSDK.dll failed.
	goto EXIT 
)

REM "%MSBUILDDIR%\msbuild.exe" solutions\KitchenSink\KitchenSink.sln /t:Rebuild /p:Configuration="Debug" /p:Platform="x86"
"%DEVENVDIR%\devenv" solutions\KitchenSink\KitchenSink.sln /Rebuild "Debug|x86"
if not %errorlevel% == 0 ( 
	echo build debug version failed!
	goto EXIT 
)

REM "%MSBUILDDIR%\msbuild.exe" solutions\KitchenSink\KitchenSink.sln /t:Rebuild /p:Configuration="Release" /p:Platform="x86"
"%DEVENVDIR%\devenv" solutions\KitchenSink\KitchenSink.sln /Rebuild "Release|x86"
if not %errorlevel% == 0 (
	echo build release version failed!
	goto EXIT
)

popd

:EXIT
echo error level: %errorlevel% 
EXIT /B %errorlevel%