namespace ArtAuction.Core.Application.Interfaces
{
    public interface IPasswordService
    {
        public string GetHash(string inputPassword);
        public bool IsPasswordCorrect(string password, string hash);
    }
}