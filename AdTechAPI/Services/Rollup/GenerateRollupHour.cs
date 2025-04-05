using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AdTechAPI.Rollups
{
    public class GenerateRollupHour
    {
        private readonly AppDbContext _db;
        private readonly ILogger<GenerateRollupHour> _logger;
        public GenerateRollupHour(AppDbContext db, ILogger<GenerateRollupHour> logger)
        {
            _logger = logger;
            _db = db;

        }

        public async Task Run()
        {
            //
            _logger.LogInformation("Executing Hourly Rollup");
            var stopwatch = Stopwatch.StartNew();
            var now = DateTime.UtcNow;

            DateTime rollupStart;
            DateTime rollupEnd;

            if (now.Minute < 3)
            {
                // Roll up the previous hour
                var previousHour = now.AddHours(-1);
                rollupStart = new DateTime(previousHour.Year, previousHour.Month, previousHour.Day, previousHour.Hour, 0, 0);
            }
            else
            {
                // Roll up current hour
                rollupStart = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
            }

            rollupEnd = rollupStart.AddMinutes(59).AddSeconds(59);

            string sql = $@"
                        INSERT INTO ""RollupHour"" (
                            ""StatDate"",
                            ""StatHour"",
                            ""PublisherId"",
                            ""TrafficSourceId"",
                            ""AdvertiserId"",
                            ""CampaignId"",
                            ""LanderId"",
                            ""Clicks"",
                            ""Revenue"",
                            ""CreatedAt"",
                            ""UpdatedAt""
                        )
                        SELECT 
                            CAST(@rollupDate AS date) AS ""StatDate"",
                            @rollupHour AS ""StatHour"",
                            ""PublisherId"",
                            ""TrafficSourceId"",
                            ""AdvertiserId"",
                            ""CampaignId"",
                            ""LanderId"",
                            COUNT(*) AS ""Clicks"",
                            SUM(""Revenue"") AS ""Revenue"",
                            NOW() AT TIME ZONE 'UTC' AS ""CreatedAt"",
                            NOW() AT TIME ZONE 'UTC' AS ""UpdatedAt""
                        FROM ""Clicks""
                        WHERE ""CreatedAt"" >= @startDate AND ""CreatedAt"" <= @endDate
                        GROUP BY 
                            ""PublisherId"",
                            ""TrafficSourceId"",
                            ""AdvertiserId"",
                            ""CampaignId"",
                            ""LanderId""
                        ON CONFLICT (
                            ""StatDate"",
                            ""StatHour"",
                            ""PublisherId"",
                            ""TrafficSourceId"",
                            ""AdvertiserId"",
                            ""CampaignId"",
                            ""LanderId""
                        )
                        DO UPDATE SET
                            ""Clicks"" = EXCLUDED.""Clicks"",
                            ""Revenue"" = EXCLUDED.""Revenue"",
                            ""UpdatedAt"" = NOW() AT TIME ZONE 'UTC'
                            ";

            await _db.Database.ExecuteSqlRawAsync(
                    sql,
                    new NpgsqlParameter("rollupDate", DateOnly.FromDateTime(rollupStart)),
                    new NpgsqlParameter("rollupHour", rollupStart),
                    new NpgsqlParameter("startDate", rollupStart),
                    new NpgsqlParameter("endDate", rollupEnd)
                );
            stopwatch.Stop();
            _logger.LogInformation("Rollup Hour: Finished in {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);
        }
    }
}