namespace ArtAuction.Core.Base.Interfaces
{
    public interface IPasswordService
    {
        public string GetHash(string inputPassword);
        public bool IsPasswordCorrect(string password, string hash);
    }
}