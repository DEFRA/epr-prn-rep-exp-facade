namespace Epr.Reprocessor.Exporter.Facade.App.Services.Registration;

public interface IRegistrationMaterialService
{
    Task CreateExemptionReferences(CreateExemptionReferencesDto dto);

    Task<CreateRegistrationMaterialResponseDto> CreateRegistrationMaterial(CreateRegistrationMaterialRequestDto requestDto);
}
