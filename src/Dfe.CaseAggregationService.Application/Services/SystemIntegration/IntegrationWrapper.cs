using System.Diagnostics;
using Dfe.CaseAggregationService.Application.Common.Models;
using Dfe.CaseAggregationService.Application.Services.Builders;
using Microsoft.Extensions.Logging;

namespace Dfe.CaseAggregationService.Application.Services.SystemIntegration
{
    public class IntegrationWrapper<T>(IGetCaseInfo<T> mapper, ILogger logger)
    {
        private const string SourceTimingEventName = "CaseAggregation.IntegrationSourceTiming";

        protected async Task<IEnumerable<UserCaseInfo>> TrackAndProcess(
            string sourceName,
            Func<Task<IEnumerable<T>>> fetch,
            CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var cases = await fetch();
                var results = cases.Select(mapper.GetCaseInfo).ToList();

                stopwatch.Stop();
                EmitSourceTiming(sourceName, stopwatch.Elapsed.TotalMilliseconds, results.Count, success: true, skipped: false);

                return results;
            }
            catch (OperationCanceledException)
            {
                stopwatch.Stop();
                EmitSourceTiming(sourceName, stopwatch.Elapsed.TotalMilliseconds, recordCount: 0, success: false, skipped: false);
                throw;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                logger.LogError(ex, ex.Message);
                EmitSourceTiming(sourceName, stopwatch.Elapsed.TotalMilliseconds, recordCount: 0, success: false, skipped: false);
                return [];
            }
        }

        protected Task<IEnumerable<UserCaseInfo>> SkippedResult(string sourceName)
        {
            EmitSourceTiming(sourceName, elapsedMs: 0, recordCount: 0, success: true, skipped: true);
            return Task.FromResult<IEnumerable<UserCaseInfo>>([]);
        }

        private void EmitSourceTiming(
            string sourceName,
            double elapsedMs,
            int recordCount,
            bool success,
            bool skipped)
        {
            using (logger.BeginScope(new Dictionary<string, object>
            {
                ["EventName"] = SourceTimingEventName,
                ["Source"] = sourceName,
                ["ElapsedMs"] = elapsedMs,
                ["RecordCount"] = recordCount,
                ["Success"] = success,
                ["Skipped"] = skipped
            }))
            {
                logger.LogInformation(
                    "{EventName}: Source={Source}, ElapsedMs={ElapsedMs}, RecordCount={RecordCount}, Success={Success}, Skipped={Skipped}",
                    SourceTimingEventName,
                    sourceName,
                    elapsedMs,
                    recordCount,
                    success,
                    skipped);
            }
        }
    }
}
