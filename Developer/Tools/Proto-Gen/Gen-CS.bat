@echo off

set BUILD_PATH="%cd%\Build.py"
python %BUILD_PATH% cs


if not %errorlevel%==0 ( goto fail ) else ( goto success )

:success

echo build ok
goto end

:fail

echo build fail
goto end

:end

pause