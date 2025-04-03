using Epr.Reproccessor.Exporter.Facade.Api.Models;
using Epr.Reproccessor.Exporter.Facade.Api.Services.Interfaces;
using Epr.Reproccessor.Exporter.Facade.App.Config;
using Epr.Reproccessor.Exporter.Facade.App.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace Epr.Reproccessor.Exporter.Facade.Api.Services
{
    public class SaveAndContinueService (HttpClient httpClient,
    ILogger<SaveAndContinueService> logger,
    IOptions<PrnBackendServiceApiConfig> settings) : ISaveAndContinueService
    {

        public async Task<HttpResponseMessage> AddAsync(SaveAndContinueRequest request)
        {
            var response = await httpClient.PostAsJsonAsync(settings.Value.Endpoints?.SaveAndContinueSaveUri, request);

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

        public async Task<SaveAndContinueResponse?> GetLatestAsync(int registrationId, string area)
        {
            var response = await httpClient.GetAsync($"{settings.Value.Endpoints?.SaveAndContinueGetLatestUri}/{registrationId}/{area}");
            
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

                if (problemDetails != null)
                {
                    throw new ProblemResponseException(problemDetails, response.StatusCode);
                }
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<SaveAndContinueResponse>();
        }
    }
}
