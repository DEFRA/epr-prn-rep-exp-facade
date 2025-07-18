﻿namespace Epr.Reprocessor.Exporter.Facade.App.Clients;

using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Epr.Reprocessor.Exporter.Facade.App.Constants;

public abstract class BaseHttpClient
{
    protected readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    protected BaseHttpClient(HttpClient httpClient, JsonSerializerOptions jsonOptions = null)
    {
        _httpClient = httpClient;
        _jsonOptions = jsonOptions ?? new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    protected async Task<TResponse> GetAsync<TResponse>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TResponse>(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        });
    }

    protected async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
    {
        var content = CreateJsonContent(data);
        var response = await _httpClient.PostAsync(url, content);

        response.EnsureSuccessStatusCode();

        if (response.StatusCode == HttpStatusCode.NoContent)
            return default;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        return await response.Content.ReadFromJsonAsync<TResponse>(options);
    }

    protected async Task PostAsync<TRequest>(string url, TRequest data)
    {
        var content = CreateJsonContent(data);
        var response = await _httpClient.PostAsync(url, content);

        response.EnsureSuccessStatusCode();
    }

    protected async Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest data)
    {
        var content = CreateJsonContent(data);
		var response = await _httpClient.PutAsync(url, content);
		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync();
		return JsonSerializer.Deserialize<TResponse>(json, _jsonOptions);
	}

    protected async Task<bool> PutAsync<TRequest>(string url, TRequest data)
    {
        var content = CreateJsonContent(data);
        var response = await _httpClient.PutAsync(url, content);
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    protected async Task<bool> PatchAsync<TRequest>(string url, TRequest data)
    {
        var content = CreateJsonContent(data);
        var response = await _httpClient.PatchAsync(url, content);
        response.EnsureSuccessStatusCode();

        return response.IsSuccessStatusCode;
    }

    protected async Task<bool> DeleteAsync(string url)
    {
        var response = await _httpClient.DeleteAsync(url);
        return response.IsSuccessStatusCode;
    }

    private StringContent CreateJsonContent<T>(T data)
    {
        var json = JsonSerializer.Serialize(data, _jsonOptions);
        return new StringContent(json, Encoding.UTF8, ContentType.ApplicationJson);
    }
}
