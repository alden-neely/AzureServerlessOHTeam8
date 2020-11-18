using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OpenHackTeam8
{
    public static class CreateRating
    {
        [FunctionName("CreateRating")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "Starfruit",
                collectionName: "Ratings",
                ConnectionStringSetting = "CosmosDBConnectionString")]
            IAsyncCollector<RatingDto> ratingOut,
            ILogger log)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            RatingDto rating = JsonConvert.DeserializeObject<RatingDto>(requestBody);

            // Validate userId, productId, and rating
            var validationErrors = await Validation.GetErrors(rating);
            if (validationErrors.Count > 0)
            {
                return new BadRequestObjectResult(validationErrors);
            }

            // add ID guid
            rating.id = Guid.NewGuid();

            // add timestamp
            rating.Timestamp = DateTime.UtcNow;

            await ratingOut.AddAsync(rating);

            return new OkObjectResult(rating);
        }
    }
}
