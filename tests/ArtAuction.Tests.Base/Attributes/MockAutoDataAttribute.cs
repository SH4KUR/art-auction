using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace ArtAuction.Tests.Base.Attributes
{
    public class MockAutoDataAttribute : AutoDataAttribute
    {
        public MockAutoDataAttribute() 
            : base(() => new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }   
}