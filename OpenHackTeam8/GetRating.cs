using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace OpenHackTeam8
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "GetRating/{ratingId}")] HttpRequest req,
            [CosmosDB(
                databaseName: "Starfruit",
                collectionName: "Ratings",
                ConnectionStringSetting = "CosmosDBConnectionString",
                SqlQuery = "SELECT * FROM r WHERE r.id={ratingId}"
            )]
                IEnumerable<RatingDto> ratings,
            ILogger log)
        {
            var output = ratings.FirstOrDefault();

            return new OkObjectResult(output);
        }
    }
}
