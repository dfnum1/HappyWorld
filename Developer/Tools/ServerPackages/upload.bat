@echo off
set curDir=%cd%
set winscp_path="C:\Program Files (x86)\WinSCP\WinSCP.exe"
set username=root
set ip_address=192.144.198.50
set ip_port=57321
set remote_dir=/data/
set local_file=server_bin.zip
set password="ZU1dv2*3cf&1p!H$y7lL"

%winscp_path% /console /command "option batch continue" "option confirm off" "open sftp://%username%:"%password%"@%ip_address%:%ip_port%" "option transfer binary" "put %local_file%  %remote_dir%" "exit" /log=%curDir%/file_to_linux_log.txt