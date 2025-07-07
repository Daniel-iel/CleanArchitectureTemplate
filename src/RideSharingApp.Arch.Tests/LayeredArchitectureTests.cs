using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace RideSharingApp.Arch.Tests
{
    public class LayeredArchitectureTests
    {
        private static readonly Architecture Architecture =
            new ArchLoader().LoadAssemblies(
                typeof(RideSharingApp.Domain.Rides.RideRequest).Assembly,
                typeof(RideSharingApp.Application.DependencyInjection).Assembly,
                typeof(RideSharingApp.Infrastructure.Database.UnitOfWork).Assembly,
                typeof(RideSharingApp.Api.DependencyInjection).Assembly
            ).Build();

        [Fact]
        public void Domain_Should_Not_Depend_On_Application_Infrastructure_Or_Api()
        {
            var domain = Types().That().ResideInNamespace("RideSharingApp.Domain", true);
            var forbidden = Types().That().ResideInNamespace("RideSharingApp.Application", true)
                .Or().ResideInNamespace("RideSharingApp.Infrastructure", true)
                .Or().ResideInNamespace("RideSharingApp.Api", true);
            domain.Should().NotDependOnAny(forbidden).Check(Architecture);
        }

        [Fact]
        public void Application_Should_Not_Depend_On_Infrastructure_Or_Api()
        {
            var application = Types().That().ResideInNamespace("RideSharingApp.Application", true);
            var forbidden = Types().That().ResideInNamespace("RideSharingApp.Infrastructure", true)
                .Or().ResideInNamespace("RideSharingApp.Api", true);
            application.Should().NotDependOnAny(forbidden).Check(Architecture);
        }

        [Fact]
        public void Infrastructure_Should_Not_Depend_On_Api()
        {
            var infrastructure = Types().That().ResideInNamespace("RideSharingApp.Infrastructure", true);
            var forbidden = Types().That().ResideInNamespace("RideSharingApp.Api", true);
            infrastructure.Should().NotDependOnAny(forbidden).Check(Architecture);
        }

        [Fact]
        public void Api_Should_Depend_On_Other_Layers_But_Not_Vice_Versa()
        {
            var api = Types().That().ResideInNamespace("RideSharingApp.Api", true);
            var forbidden = Types().That().ResideInNamespace("RideSharingApp.Domain", true)
                .Or().ResideInNamespace("RideSharingApp.Application", true)
                .Or().ResideInNamespace("RideSharingApp.Infrastructure", true);
            forbidden.Should().NotDependOnAny(api).Check(Architecture);
        }
    }
}
