@echo off
set mytime=%time::=%
set mytime=%mytime:,=%
set dtstamp=%date%_%mytime%

cloc-1.60.exe ..\..\src ^
--3 ^
--windows ^
--list-file=include-file.txt ^
--report-file=cloc_output_report_%dtstamp%.txt ^
--counted=cloc_output_report_%dtstamp%_counted.txt ^
--ignored=cloc_output_report_%dtstamp%_ignored.txt ^
--exclude-list-file=exclude_file.txt ^
--exclude-lang="MSBuild scripts,XML,XSD,XSLT" ^
--not-match-f=(?i)[.]designer[.]cs^|[.]config^|Assemblyinfo[.]cs^|jquery-1\.4\.1^|feature[.]cs ^
--not-match-d=(?i)/bin/^|/obj/^|App_LocalResources^|_ReSharper 

PAUSE
