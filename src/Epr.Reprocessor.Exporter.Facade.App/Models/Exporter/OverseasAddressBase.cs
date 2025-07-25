﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Epr.Reprocessor.Exporter.Facade.App.Models.Exporter
{
    [ExcludeFromCodeCoverage]
    public class OverseasAddressBase
    {
        [MaxLength(100)]
        public required string AddressLine1 { get; set; }
        [MaxLength(100)]
        public required string AddressLine2 { get; set; }
        [MaxLength(70)]
        public required string CityorTown { get; set; }
        public required string Country { get; set; }
        public Guid Id { get; set; }
        public required string OrganisationName { get; set; }
        [MaxLength(20)]
        public required string PostCode { get; set; }
        [MaxLength(70)]
        public required string StateProvince { get; set; }
    }
}