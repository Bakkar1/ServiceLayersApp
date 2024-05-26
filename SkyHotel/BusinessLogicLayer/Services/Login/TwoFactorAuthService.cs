using Microsoft.AspNetCore.Identity;
using System.Text.Encodings.Web;
using System.Text;
using DataAccessLayer.Data;
using DataAccessLayer.Model;
using DataAccessLayer.ViewModel.TwoFactorAuth;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services.Login
{
    public interface ITwoFactorAuthRepository
    {
        Task<AuthenticatorVM?> GetAuthenticatorVM(string userName);
        Task<AuthenticatorResult> EnabeTwoFactorAuth(DotNetUser currentUser, string code);
        Task<SignInResult> VerifyTwoFactorToken(TwoFactorVM model);

        Task<DotNetUser> GetTwoFactorAuthenticationUser();

        Task<bool> Disable2faTwoFactorAuth(string userName);
        Task ResetAuthenticator(string userName);

        Task<SignInResult> TwoFactorRecoveryCodeSignIn(string recoveryCode);

    }
    public class TwoFactorAuthRepository : ITwoFactorAuthRepository
    {
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        private readonly UserManager<DotNetUser> _userManager;
        private readonly SignInManager<DotNetUser> _signInManager;
        private readonly UrlEncoder _urlEncoder;
        private readonly AppDbContext _context;

        public TwoFactorAuthRepository(
            UserManager<DotNetUser> userManager,
            UrlEncoder urlEncoder,
            AppDbContext context,
            SignInManager<DotNetUser> signInManager)
        {
            _userManager = userManager;
            _urlEncoder = urlEncoder;
            _context = context;
            _signInManager = signInManager;
        }

        public async Task<AuthenticatorResult> EnabeTwoFactorAuth(DotNetUser currentUser, string code)
        {
            AuthenticatorResult authenticatorResult = new AuthenticatorResult();

            // Strip spaces and hypens
            var verificationCode = code.Replace(" ", string.Empty).Replace("-", string.Empty);

            bool is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                currentUser, TokenOptions.DefaultAuthenticatorProvider, verificationCode);

            if (!is2faTokenValid)
            {
                authenticatorResult.Error = "Verification code is invalid.";
                return authenticatorResult;
            }
            await _userManager.SetTwoFactorEnabledAsync(currentUser, true);

            //var userId = await _userManager.GetUserIdAsync(currentUser);
            //_logger.LogInformation("User with ID '{UserId}' has enabled 2FA with an authenticator app.", userId);


            authenticatorResult.StatusMessage = "Your authenticator app has been verified.";

            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(currentUser, 10);
            authenticatorResult.RecoveryCodes = recoveryCodes.ToList();

            authenticatorResult.IsSucceded = true;

            return authenticatorResult;
        }

        public async Task<SignInResult> VerifyTwoFactorToken(TwoFactorVM model)
        {
            var user = await GetTwoFactorAuthenticationUser();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            string? authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);
            //if(string.IsNullOrWhiteSpace(authenticatorCode))
            //{
            //    return false;
            //}

            return await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode,
                model.RememberMe, model.RememberMachine);
        }

        public async Task<DotNetUser> GetTwoFactorAuthenticationUser()
        {
            return await _signInManager.GetTwoFactorAuthenticationUserAsync();
        }
        public async Task<AuthenticatorVM?> GetAuthenticatorVM(string userName)
        {
            DotNetUser? user = _context.Users.Where(u => u.UserName == userName).FirstOrDefault();

            if (user == null) { return null; }

            // Load the authenticator key & QR code URI to display on the form
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            var SharedKey = FormatKey(unformattedKey);

            var AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);

            AuthenticatorVM authenticator = new AuthenticatorVM()
            {
                SharedKey = SharedKey,
                AuthenticatorUri = AuthenticatorUri,
            };

            return authenticator;
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
            AuthenticatorUriFormat,
                _urlEncoder.Encode("Yasser App"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }

        public async Task<bool> Disable2faTwoFactorAuth(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                return false;
            }

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);

            return disable2faResult.Succeeded;
        }

        public async Task ResetAuthenticator(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                return;
            }
            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            await _signInManager.RefreshSignInAsync(user);
        }

        public async Task<SignInResult> TwoFactorRecoveryCodeSignIn(string recoveryCode)
        {
            return await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);
        }
    }


}
