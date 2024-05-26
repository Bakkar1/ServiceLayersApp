namespace DataAccessLayer.ViewModel.TwoFactorAuth
{
    public class AuthenticatorResult
    {
        public bool IsSucceded { get; set; } = false;
        public List<string>? RecoveryCodes { get; set; }
        public string? StatusMessage { get; set; }

        public string? Error { get; set; }
    }
}
