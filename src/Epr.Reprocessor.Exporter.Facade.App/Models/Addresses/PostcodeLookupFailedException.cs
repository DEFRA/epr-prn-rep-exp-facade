﻿using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Addresses;


[Serializable]
[ExcludeFromCodeCoverage]
public class PostcodeLookupFailedException : Exception
{
	public PostcodeLookupFailedException() { }
	public PostcodeLookupFailedException(string message) : base(message) { }
	public PostcodeLookupFailedException(string message, Exception inner) : base(message, inner) { }
	protected PostcodeLookupFailedException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}