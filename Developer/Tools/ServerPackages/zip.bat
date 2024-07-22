@echo off
set curDir=%cd%

set dir=%curDir%\server_bin

set zip=C:\Program Files\7-Zip\7z.exe


echo "%zip%"
echo "%dir%"

"%zip%" a -tzip server_bin.zip "%dir%"

@pause