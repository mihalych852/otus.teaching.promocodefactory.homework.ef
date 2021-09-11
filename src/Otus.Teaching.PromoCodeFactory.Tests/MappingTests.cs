using AutoMapper;
using Otus.Teaching.PromoCodeFactory.WebHost.Infrastructure.Profiles;
using Xunit;

namespace Otus.Teaching.PromoCodeFactory.Tests
{
    public class MappingTests
    {
        private readonly IMapper _sut;

        public MappingTests() =>
            _sut = new MapperConfiguration(cfg => { cfg.AddProfile<AutoMapping>(); }).CreateMapper();


        [Fact]
        public void All_mappings_should_be_setup_correctly()
        {
            _sut.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
