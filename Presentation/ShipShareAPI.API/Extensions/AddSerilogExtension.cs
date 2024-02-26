using Microsoft.AspNetCore.Cors.Infrastructure;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.MSSqlServer.Sinks.MSSqlServer.Options;
using System.Collections.ObjectModel;

namespace ShipShareAPI.API.Extensions
{
    public static class AddSerilogExtension
    {

        public static void AddSerilog(this WebApplicationBuilder builder)
        {
            var sinkOptions = new MSSqlServerSinkOptions()
            {
                TableName = "logs",
                AutoCreateSqlTable = true,
            };

            //var colOptions = new ColumnOptions()
            //{
            //    AdditionalColumns = new Collection<SqlColumn>()
            //            {
            //                new SqlColumn()
            //                {
            //                    ColumnName = "user_name",
            //                    DataType = System.Data.SqlDbType.NVarChar,
            //                },
            //            }
            //};

            Logger log = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt")
                .WriteTo.MSSqlServer(
                builder.Configuration.GetConnectionString("ShipShareConStr"),
                sinkOptions: sinkOptions
                //columnOptions: colOptions
                )
                .Enrich.FromLogContext()
                .CreateLogger();
            builder.Host.UseSerilog(log);
        }
    }
}
