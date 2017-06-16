set SysDate=%date:~0,4%%date:~5,2%%date:~8,2%%time:~0,2%%time:~3,2%%time:~6,2%
sqlcmd -S 127.0.0.1 -U sa -P 53394812 -d Master -Q "EXEC dbo.Usp_RunCollection '' " -O .\LOG\%SysDate%.txt
