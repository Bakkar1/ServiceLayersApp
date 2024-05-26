using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccessLayer.Model;

namespace BusinessLogicLayer.Services.Login
{
    public interface ILoginRepository
    {
        #region Password
        Task<DotNetUser> GetCurrentUser();
        Task<IdentityResult> ChangePassword(DotNetUser user, string currentPassword, string newPassword);
        Task RefreshSignIn(DotNetUser user);
        Task<string> GeneratePasswordResetToken(DotNetUser user);
        Task<IdentityResult> ResetPasswordAsync(DotNetUser user, string token, string newPassword);
        #endregion

        #region Account
        IEnumerable<DotNetUser> GetUsers();
        Task<DotNetUser> GetUserAsync(string email);
        Task<SignInResult> SignInWithPassword(string userName, string password, bool remeberMe = false);
        Task SignIn(DotNetUser user, bool IsPersistent);
        Task<IdentityResult> CreateUser(DotNetUser user, string password);
        Task Logout();
        DotNetUser GetUserByUserName(string userName);
        Task<bool> IsEmailConfirmed(DotNetUser user);
        Task<IdentityResult> ConfirmEmail(DotNetUser user, string token);
        Task<DotNetUser> GetUserById(string userId);
        Task<bool> IsAdmin(DotNetUser user);
        Task<bool> IsOwner(DotNetUser user);
        Task AddToUserRole(DotNetUser identityUser);
        #endregion

        #region External Login
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
        Task<DotNetUser> GetUserByEmail(string email);
        string GetExternalName(ExternalLoginInfo externalLoginInfo);
        string GetExternalNameByName(ExternalLoginInfo externalLoginInfo);
        string GetExternalEmail(ExternalLoginInfo externalLoginInfo);
        string GetExternalEmailByEmail(ExternalLoginInfo externalLoginInfo);
        Task<Microsoft.AspNetCore.Identity.SignInResult> ExternalLoginSignInAsync(ExternalLoginInfo externalLoginInfo);
        Task<IdentityResult> AddLogin(DotNetUser user, ExternalLoginInfo externalLoginInfo);
        AuthenticationProperties GetEXtAuthProperties(string provider, string redirectUrl);
        Task<IdentityResult> AddUser(DotNetUser user);
        Task<string> GenerateEmailConfirmationToken(DotNetUser user);
        #endregion

        #region EditUser
        string GetCurrentUserId();
        Task<bool> IsAlredyEmailExist(string email, string userId);
        Task<DotNetUser> UpdateUser(DotNetUser user);
        #endregion

        #region Role
        public SelectList GetRoles();
        string GetRoleName(string UserId);
        Task AddToRole(DotNetUser identityUser, string role);
        void DeleteOldRole(string UserId);
        Task UpdateRole(DotNetUser identityUser, string roleId);
        string GetCurrentUserRole();
        #endregion

        Task<bool> CheckPasswordAsync(DotNetUser user, string password);
    }
}
