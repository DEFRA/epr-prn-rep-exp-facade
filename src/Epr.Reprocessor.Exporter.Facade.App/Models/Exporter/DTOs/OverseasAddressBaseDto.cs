﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Exporter.DTOs
{
    [ExcludeFromCodeCoverage]
    public class OverseasAddressBaseDto
    {
        [MaxLength(100)]
        public required string AddressLine1 { get; set; }
        [MaxLength(100)]
        public required string AddressLine2 { get; set; }
        [MaxLength(70)]
        public required string CityOrTown { get; set; }
        [MaxLength(100)]
        public required string CountryName { get; set; }
        public Guid ExternalId { get; set; }
        [MaxLength(100)]
        public required string OrganisationName { get; set; }
        [MaxLength(20)]
        public required string PostCode { get; set; }
        [MaxLength(70)]
        public required string StateProvince { get; set; }
    }
}