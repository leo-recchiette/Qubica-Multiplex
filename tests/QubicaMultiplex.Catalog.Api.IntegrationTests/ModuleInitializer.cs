using System.Runtime.CompilerServices;

namespace QubicaMultiplex.Catalog.Api.IntegrationTests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
        => VerifierSettings.DontScrubDateTimes();
}
