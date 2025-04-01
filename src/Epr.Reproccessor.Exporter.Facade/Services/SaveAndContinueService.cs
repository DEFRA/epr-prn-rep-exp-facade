using Epr.Reproccessor.Exporter.Facade.Api.Models;
using Epr.Reproccessor.Exporter.Facade.Api.Services.Interfaces;
using Epr.Reproccessor.Exporter.Facade.App.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Epr.Reproccessor.Exporter.Facade.Api.Services
{
    public class SaveAndContinueService (HttpClient httpClient,
    ILogger<SaveAndContinueService> logger,
    IConfiguration config) : ISaveAndContinueService
    {
        private const string SaveAndContinueUri = "api/v1.0/saveandcontinue/save";

        public async Task<HttpResponseMessage> SaveAsync(SaveAndContinueModel model)
        {
            var response = await httpClient.PostAsJsonAsync(SaveAndContinueUri, model);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

                if (problemDetails != null)
                {
                    throw new ProblemResponseException(problemDetails, response.StatusCode);
                }
            }

            response.EnsureSuccessStatusCode();

            return response;
        }
    }
}
