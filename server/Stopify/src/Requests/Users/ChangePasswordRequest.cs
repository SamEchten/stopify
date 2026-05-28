namespace Stopify.Requests.Users;

public class ChangePasswordRequest(string currentPassword, string newPassword)
{
    public string CurrentPassword { get; } = currentPassword;
    public string NewPassword { get; } = newPassword;
}
