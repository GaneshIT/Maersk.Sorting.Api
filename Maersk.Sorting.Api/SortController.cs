using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api.Controllers
{
    [ApiController]
    [Route("sort")]
    public class SortController : Controller
    {
        private readonly ISortJobProcessor _sortJobProcessor;

        #region web methods

        #region SortController
        public SortController(ISortJobProcessor sortJobProcessor)
        {
            _sortJobProcessor = sortJobProcessor;
        }
        #endregion

        #region EnqueueAndRunJob
        [HttpPost("run")]
        [Obsolete("This executes the sort job asynchronously. Use the asynchronous 'EnqueueJob' instead.")]
        public async Task<ActionResult<SortJob>> EnqueueAndRunJob(int[] values)
        {
            var pendingJob = new SortJob(
                id: Guid.NewGuid(),
                status: SortJobStatus.Pending,
                duration: null,
                input: values,
                output: null);

            var completedJob = await _sortJobProcessor.Process(pendingJob);

            return Ok(completedJob);
        }
        #endregion

        #region EnqueueJob
        [HttpPost]
        public async Task<ActionResult<SortJob>> EnqueueJob(int[] values)
        {
            // TODO: Should enqueue a job to be processed in the background.
            var pendingJob = new SortJob(
                id: Guid.NewGuid(),
                status: SortJobStatus.Pending,
                duration: null,
                input: values,
                output: null);

            var completedJob = await _sortJobProcessor.Process(pendingJob);

            return Ok(completedJob);
        }
        #endregion

        #region GetJobs
        [HttpGet]
        public async Task<ActionResult<List<SortJob>>> GetJobs()
        {
            var getjobs = await _sortJobProcessor.GetJobs();
            return Ok(getjobs);
        }
        #endregion

        #region GetJob
        [HttpGet("{jobId}")]
        public async Task<ActionResult<SortJob>> GetJob(Guid jobId)
        {
            var getJob = await _sortJobProcessor.GetJob(jobId);
            return Ok(getJob);
        }
        #endregion

        #endregion
    }
}
