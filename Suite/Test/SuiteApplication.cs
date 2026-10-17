using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Testing.Platform.Services;
using Model;
using Model.NewModel;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Test;

internal static class SuiteApplication
{
    private static readonly ActivitySource activitySource = new(typeof(SuiteApplication).FullName!);
    private static IHost? host;
    private static Activity? activity;

    [AssemblyInitialize]
    [SuppressMessage("Usage", "MSTEST0012:AssemblyInitialize methods should have valid layout", Justification = "Analyzer doesn't know AssemblyFixtureProvider")]
    public static async Task Initialize(TestContext context)
    {
        var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
        {
            ContentRootPath = AppContext.BaseDirectory,
        });

        builder.Logging.ClearProviders().AddTestContext();
        builder.Services.AddSuite(builder.Configuration);
        builder.Services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddSource(activitySource.Name));

        host = builder.Build();
        await host.StartAsync(context.CancellationToken);

        activity = activitySource.StartActivity(Resources.Graph.Manifest.Name)?.SetTag("test.suite.name", Resources.Graph.Manifest.Name);
    }

    [AssemblyCleanup]
    [SuppressMessage("Usage", "MSTEST0013:AssemblyCleanup methods should have valid layout", Justification = "Analyzer doesn't know AssemblyFixtureProvider")]
    public static async Task Cleanup(TestContext context)
    {
        activity?.Dispose();

        if (host is null)
        {
            return;
        }

        await host.StopAsync(context.CancellationToken);
        host.Dispose();
    }

    internal static async Task<Result> Test(string name)
    {
        var services = host?.Services ?? throw new InvalidOperationException("Host not initialised");

        var executor = services.GetRequiredService<Executor>();

        return await executor.Execute(name, activity?.Context ?? default);
    }
}
