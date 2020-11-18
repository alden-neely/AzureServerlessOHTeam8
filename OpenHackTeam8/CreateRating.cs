using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

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

            // Validate userId and productId

            // validate rating
            if (IsRatingRangeNotValid(rating.Rating))
            {
                return new BadRequestObjectResult("rating must be a number between 0 and 5");
            }

            // add ID guid
            rating.Id = Guid.NewGuid();

            // add timestamp
            rating.Timestamp = DateTime.UtcNow;

            await ratingOut.AddAsync(rating);

            return new OkObjectResult(rating);
        }

        public static bool IsRatingRangeNotValid(int rating) =>
            rating < 0 && rating > 5;
        
    }
}
