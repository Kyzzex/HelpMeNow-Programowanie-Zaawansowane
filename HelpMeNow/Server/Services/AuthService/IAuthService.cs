namespace HelpMeNow.Server.Services.AuthService
{
    public interface IAuthService
    {
        Task<Response<int>> Register(User user, string password);
        Task<bool> UserExists(string email);

        Task<Response<string>> Login(string email, string password);
    }
}
