using Moq;

namespace ArtAuction.Tests.Base.Extensions
{
    public static class MockExtension
    {
        public static Mock<T> AsMock<T>(this T obj) where T : class
        {
            return Mock.Get(obj);
        }
    }
}