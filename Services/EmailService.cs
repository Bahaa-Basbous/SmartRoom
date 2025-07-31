public class EmailService : IEmailService
{
    public async Task SendPasswordResetEmail(string to, string token)
    {
        var resetUrl = $"http://localhost:5173/reset-password?token={token}&email={to}";
        Console.WriteLine($"[Email to {to}] Password reset link: {resetUrl}");
        await Task.CompletedTask;
    }
}
