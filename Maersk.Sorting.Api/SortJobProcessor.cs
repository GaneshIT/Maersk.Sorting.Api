using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api
{
    public class SortJobProcessor : ISortJobProcessor
    {
        private readonly ILogger<SortJobProcessor> _logger;
        public static List<SortJob> listOfJobs = new List<SortJob>();

        #region methods

        #region SortJobProcessor
        public SortJobProcessor(ILogger<SortJobProcessor> logger)
        {
            _logger = logger;
        }
        #endregion

        #region GetJob
        public Task<SortJob> GetJob(Guid jobid)
        {
            var data= listOfJobs.Where(j => j.Id == jobid).ToList();
            SortJob sortJob = new SortJob(data[0].Id, data[0].Status, data[0].Duration, data[0].Input, data[0].Output);
            return Task.FromResult(sortJob);
        }
        #endregion

        #region GetJobs
        public Task<List<SortJob>> GetJobs()
        {
            return Task.FromResult(listOfJobs);
        }
        #endregion

        #region Process
        public async Task<SortJob> Process(SortJob job)
        {
            listOfJobs.Add(job);
            _logger.LogInformation("Processing job with ID '{JobId}'.", job.Id);

            var stopwatch = Stopwatch.StartNew();

            var output = job.Input.OrderBy(n => n).ToArray();
            await Task.Delay(5000); // NOTE: This is just to simulate a more expensive operation

            var duration = stopwatch.Elapsed;

            _logger.LogInformation("Completed processing job with ID '{JobId}'. Duration: '{Duration}'.", job.Id, duration);
            var sortjob = new SortJob(
                id: job.Id,
                status: SortJobStatus.Completed,
                duration: duration,
                input: job.Input,
                output: output);
            listOfJobs.RemoveAt(listOfJobs.Count - 1);
            listOfJobs.Add(sortjob);
            return sortjob;
        }
        #endregion

        #endregion

    }
}
