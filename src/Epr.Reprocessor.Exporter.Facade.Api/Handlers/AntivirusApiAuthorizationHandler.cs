﻿using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using Azure.Core;
using Azure.Identity;
using Epr.Reprocessor.Exporter.Facade.App.Config;
using Microsoft.Extensions.Options;

namespace Epr.Reprocessor.Exporter.Facade.Api.Handlers;

[ExcludeFromCodeCoverage]
public class AntivirusApiAuthorizationHandler : DelegatingHandler
{
	private readonly TokenRequestContext _tokenRequestContext;
	private readonly ClientSecretCredential _credentials;

	public AntivirusApiAuthorizationHandler(IOptions<AntivirusApiConfig> options)
	{
		var antivirusApiOptions = options.Value;
		_tokenRequestContext = new TokenRequestContext(new[] { antivirusApiOptions.Scope });
		_credentials = new ClientSecretCredential(antivirusApiOptions.TenantId, antivirusApiOptions.ClientId, antivirusApiOptions.ClientSecret);
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var tokenResult = await _credentials.GetTokenAsync(_tokenRequestContext, cancellationToken);
		request.Headers.Authorization = new AuthenticationHeaderValue(Microsoft.Identity.Web.Constants.Bearer, tokenResult.Token);

		return await base.SendAsync(request, cancellationToken);
	}
}