public interface IEmailService
{
    Task SendPasswordResetEmail(string to, string token);
}
