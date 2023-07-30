using NBomber.Contracts.Stats;
using NBomber.CSharp;

using var httpClient = new HttpClient
{
    BaseAddress = new Uri("http://localhost:5009"),
};

var scenario = Scenario.Create("fetch_weatherforecast_scenario", async context =>
{
    try
    {
        var response = await httpClient.GetAsync("weatherforecast");
        return response.IsSuccessStatusCode
            ? Response.Ok()
            : Response.Fail();
    }
    catch
    {
        return Response.Fail();
    }
})
    .WithWarmUpDuration(TimeSpan.FromSeconds(60))
    .WithLoadSimulations(
        Simulation.Inject(rate: 1, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromMinutes(10)));

NBomberRunner
  .RegisterScenarios(scenario)
  .WithReportFileName("fetch_weatherforecast_report")
  .WithReportFolder("weatherforecast_reports")
  .WithReportFormats(ReportFormat.Html, ReportFormat.Md)
  .Run();