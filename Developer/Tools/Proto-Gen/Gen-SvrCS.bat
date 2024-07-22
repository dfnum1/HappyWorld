@echo off
set BUILD_DIR="%cd%"
if NOT "%1" == "" set BUILD_DIR=%1

set BUILD_PATH="%BUILD_DIR%\Build.py"
python %BUILD_PATH% svrcs


if not %errorlevel%==0 ( goto fail ) else ( goto success )

:success

echo build ok
goto end

:fail

echo build fail
goto end

:end

pause