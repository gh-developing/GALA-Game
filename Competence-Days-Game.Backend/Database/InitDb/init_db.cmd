set "host=localhost"

if [%1]==[] goto skip	
set "host=%1"
:skip

if [%2]==[] goto skipuser
set "PGUSER=%2"
:skipuser

if [%3]==[] goto skippass
set "PGPASSWORD=%3"
:skippass

@echo using %host% with user %username%

pgsql\psql.exe -w -h %host% -v ON_ERROR_STOP=1 --dbname "postgres" -c "CREATE DATABASE \"GALAGame\" WITH OWNER = postgres ENCODING = 'UTF8' CONNECTION LIMIT = -1;"

pgsql\pg_restore.exe -w -v -d "postgres://%PGUSER%:%PGPASSWORD%@%host%:5432/GALAGame" ./GALAGame.backup

pgsql\psql.exe -w -h %host% -v ON_ERROR_STOP=1 --dbname "GALAGame" -c "CALL public.\"sp_InitStaticTypes\"();"

pgsql\psql.exe -w -h %host% -v ON_ERROR_STOP=1 --dbname "GALAGame" -c "CALL public.\"sp_InitStaticTypesUser\"();"