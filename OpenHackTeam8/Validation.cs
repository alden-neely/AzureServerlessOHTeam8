using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace OpenHackTeam8
{
    public static class Validation
    {
        public static string UserIdValidationUrl = "https://serverlessohuser.trafficmanager.net/api/GetUser?userId=";
        public static string ProductIdValidationUrl = "https://serverlessohproduct.trafficmanager.net/api/GetProduct?productId=";

        public static async Task<List<ValidationError>> GetErrors(RatingDto rating)
        {
            var httpClient = new HttpClient();
            
            var validationErrors = new List<ValidationError>();
            
            if (await IsUserIdNotValid(rating.UserId, httpClient)) 
            {
                validationErrors.Add(new ValidationError("Invalid UserId", $"The userId {rating.UserId} is not valid."));
            }

            if (await IsProductIdNotValid(rating.ProductId, httpClient))
            {
                validationErrors.Add(new ValidationError("Invalid ProductId", $"The productId {rating.ProductId} is not valid."));
            }

            if (IsRatingOutOfRange(rating.Rating))
            {
                validationErrors.Add(new ValidationError("Out of Range Rating Score", $"The rating of {rating.Rating} is outside the range of 0 and 5."));
            }

            return validationErrors;
        }

        public static bool IsRatingOutOfRange(int rating) =>
            !(rating >= 0 && rating <= 5);

        public static async Task<bool> IsProductIdNotValid(Guid productId, HttpClient httpClient) =>
            await IsBadRequest(ProductIdValidationUrl + productId, httpClient);
            
        public static async Task<bool> IsUserIdNotValid(Guid userId, HttpClient httpClient) =>
            await IsBadRequest(UserIdValidationUrl + userId, httpClient);
        
        public static async Task<bool> IsBadRequest(string url, HttpClient httpClient)
        {
            var response = await httpClient.GetAsync(url);
            return response.StatusCode == HttpStatusCode.BadRequest;
        }
    }
}
