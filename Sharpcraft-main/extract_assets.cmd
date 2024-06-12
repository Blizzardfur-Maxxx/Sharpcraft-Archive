@echo off
set workdir=%CD%
set clientpath=%workdir%\Client\
set corepath=%workdir%\Core\
echo put the file name in quotes if the path contains spaces.
set /p jarpath="enter file path to the b1.7.3 jar file or drag and drop: "

%workdir%\tools\AssetDumper.exe %jarpath% %clientpath% %corepath%

if errorlevel 1 (
	goto fail
)

echo Success.
timeout 20
exit

:fail
echo Failed!
pause