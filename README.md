## AspnetCore microservices:
**Development Environment:**

## Syntax
* docker-compose down => down all service in files
* docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --remove-orphans => update, re-run, ... servies in files

## 9.Serilog
* Serilog.AspNetCore" Version="5.0.0"
* Serilog.Enrichers.Environment" Version="2.2.0"
* Serilog.Formatting.Compact" Version="1.1.0" 
* Serilog.Sinks.Elasticsearch" Version="8.4.1"
* Swashbuckle.AspNetCore

## 2.1-4 ProductApi
* AutoMapper" Version="11.0.1"
* AutoMapper.Extensions.Microsoft.DependencyInjection" Version="11.0.0"
* Microsoft.EntityFrameworkCore" Version="6.0.7"
* Microsoft.EntityFrameworkCore.Design" Version="6.0.7
* MySqlConnector" Version="2.2.7"
* Pomelo.EntityFrameworkCore.MySql" Version="6.0.1"
* Serilog.AspNetCore" Version="5.0.0"
* Swashbuckle.AspNetCore" Version="6.3.1"

## 3.1-7 CustomerApi
* https://www.postgresql.org/ftp/pgadmin/pgadmin4/v7.6/windows/ => download desktop tool postgresql management
* Microsoft.EntityFrameworkCore" Version="6.0.7"
* Microsoft.EntityFrameworkCore.Design" Version="6.0.7"
* Npgsql" Version="6.0.4"
* Npgsql.EntityFrameworkCore.PostgreSQL" Version="6.0.4"
* Swashbuckle.AspNetCore" Version="6.3.1"

## 5.1-8 OrderApi
* Add, remove, update
** dotnet ef migrations add "Init_OrderDb" --project Ordering.Infrastructure --startup-project Ordering.API --output-dir Persistence\Migrations
** dotnet ef migrations remove "Init_OrderDb" --project Ordering.Infrastructure --startup-project Ordering.API
** dotnet ef database update "Init_OrderDb" --project Ordering.Infrastructure --startup-project Ordering.API


