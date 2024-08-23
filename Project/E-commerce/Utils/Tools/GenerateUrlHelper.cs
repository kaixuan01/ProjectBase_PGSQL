using System;
using System.Collections.Generic;
using System.Web;

namespace Utils.Tools
{
    public static class GenerateUrlHelper
    {
        public static string GenerateUrl(string baseUrl, string urlPath, string? id = null, Dictionary<string, string>? queryParams = null)
        {
            if (!string.IsNullOrEmpty(id))
            {
                // Replace the {id} placeholder in the path with the actual id
                urlPath = urlPath.Replace("{id}", id);
            }

            // Combine the base URL and the path
            var uriBuilder = new UriBuilder(baseUrl)
            {
                Path = urlPath
            };

            // If there are query parameters, add them to the URL
            if (queryParams != null && queryParams.Count > 0)
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);

                foreach (var param in queryParams)
                {
                    query[param.Key] = param.Value;
                }

                uriBuilder.Query = query.ToString();
            }

            return uriBuilder.ToString();
        }
    }

}
