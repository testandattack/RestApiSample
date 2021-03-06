using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ContosoRest.Models.Shared;
using Microsoft.Data.SqlClient;

namespace ContosoRest.Database
{
    public partial class UserContext : BaseContext
    {
        private SqlConnection _sqlConnection;
        private Settings _settings;

        public UserContext(DbContextOptions<UserContext> options, IOptionsSnapshot<Settings> settings, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            _settings = settings.Value;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _sqlConnection = new SqlConnection(_settings.SQL.ConnectionStrings.SqlConn_User);

            optionsBuilder.UseSqlServer(_sqlConnection, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));
            base.OnConfiguring(optionsBuilder);
        }
    }
}
