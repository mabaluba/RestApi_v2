using NUnit.Framework;
using AutoMapper;

namespace M10_RestApi.Tests;

[TestFixture]
public class AutomapperTest
{
    [Test]
    public void ShouldProperlyMapStartupProfiles()
    {
        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(Startup)));
        config.AssertConfigurationIsValid();
    }
}