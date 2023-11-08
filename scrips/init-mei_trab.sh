sleep 30s
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P Ubi2023! -d master -i /tmp/CreateTables.sql
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P Ubi2023! -d master -i /tmp/Triggers.sql
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P Ubi2023! -d master -i /tmp/Procs.sql