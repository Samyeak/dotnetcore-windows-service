using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerServicePoC
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration Configuration;

        private string ConnectionString;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                ConnectionString = Configuration["Client:Connection"];
            }
            catch (Exception ex)
            {

                throw;
            }
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            // DO YOUR STUFF HERE
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                CreateFile();
                await Task.Delay(TimeSpan.FromSeconds(25), stoppingToken);
            }
        }

        private void CreateFile()
        {
            string path = @"C:\Test";

            string randomFileName = System.IO.Path.GetRandomFileName();
            string filePath = System.IO.Path.Combine(path, randomFileName);
            System.IO.File.Create(filePath);
        }

        private async Task ExecuteTransactionQueue()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    cmd.CommandText = "USP_TRIGGER_TRANSACTION_SCHEDULE_PROCESS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 150;
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
