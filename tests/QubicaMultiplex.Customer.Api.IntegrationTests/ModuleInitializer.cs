using System.Runtime.CompilerServices;

namespace QubicaMultiplex.Customer.Api.IntegrationTests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
        => VerifierSettings.IgnoreMember("traceId");
}
