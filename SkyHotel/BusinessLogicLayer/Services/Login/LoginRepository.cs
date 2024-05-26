using DataAccessLayer.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using DataAccessLayer.Data.Default;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Data;

namespace BusinessLogicLayer.Services.Login
{
    public class LoginRepository : ILoginRepository
    {
        private readonly AppDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<DotNetUser> userManager;
        private readonly SignInManager<DotNetUser> singInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginRepository(AppDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<DotNetUser> userManager,
            SignInManager<DotNetUser> singInManager,
            IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.singInManager = singInManager;
            this._httpContextAccessor = httpContextAccessor;
        }
        public async Task AddToUserRole(DotNetUser identityUser)
        {
            await userManager.AddToRoleAsync(identityUser, Roles.NormalUser);
        }

        #region Login
        public IEnumerable<DotNetUser> GetUsers()
        {
            return (IEnumerable<DotNetUser>)context.Users.ToList();
        }
        public async Task<bool> CheckPasswordAsync(DotNetUser user, string password)
        {
            return await singInManager.UserManager.CheckPasswordAsync(user, password);
        }
        public async Task<DotNetUser> GetUserById(string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }
        public async Task<DotNetUser> GetUserAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }
        public DotNetUser GetUserByUserName(string userName)
        {
            return context.Users.Where(u => u.UserName == userName).FirstOrDefault() as DotNetUser;
        }
        public async Task<SignInResult> SignInWithPassword(string userName, string password, bool remeberMe = false)
        {
            return await singInManager.PasswordSignInAsync(
                         userName, password, remeberMe, lockoutOnFailure: false);
        }
        public async Task SignIn(DotNetUser user, bool IsPersistent)
        {
            await singInManager.SignInAsync(user, IsPersistent);
        }

        public async Task<IdentityResult> CreateUser(DotNetUser user, string password)
        {
            return await userManager.CreateAsync(user, password);
        }

        public async Task Logout()
        {
            await singInManager.SignOutAsync();
        }

        public async Task<bool> IsEmailConfirmed(DotNetUser user)
        {
            return await userManager.IsEmailConfirmedAsync(user);
        }
        public async Task<IdentityResult> ConfirmEmail(DotNetUser user, string token)
        {
            return await userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<bool> IsAdmin(DotNetUser user)
        {
            return await userManager.IsInRoleAsync(user, Roles.Admin);
        }
        public async Task<bool> IsOwner(DotNetUser user)
        {
            return await userManager.IsInRoleAsync(user, Roles.StoreOwner);
        }
        #endregion

        #region External
        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return await singInManager.GetExternalLoginInfoAsync();
        }

        public async Task<DotNetUser> GetUserByEmail(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }
        public async Task<IdentityResult> AddUser(DotNetUser user)
        {
            return await userManager.CreateAsync(user);
        }

        public async Task<IdentityResult> AddLogin(DotNetUser user, ExternalLoginInfo externalLoginInfo)
        {
            return await userManager.AddLoginAsync(user, externalLoginInfo);
        }
        public string GetExternalName(ExternalLoginInfo externalLoginInfo)
        {
            return externalLoginInfo.Principal.FindFirst(ClaimTypes.Name).Value;
        }
        public string GetExternalNameByName(ExternalLoginInfo externalLoginInfo)
        {
            return externalLoginInfo.Principal.FindFirst("name").Value;
        }
        public string GetExternalEmail(ExternalLoginInfo externalLoginInfo)
        {
            return externalLoginInfo.Principal.FindFirst(ClaimTypes.Email).Value;
        }
        public string GetExternalEmailByEmail(ExternalLoginInfo externalLoginInfo)
        {
            return externalLoginInfo.Principal.FindFirst("email").Value;
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> ExternalLoginSignInAsync(ExternalLoginInfo externalLoginInfo)
        {
            return await singInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, false);
        }
        public AuthenticationProperties GetEXtAuthProperties(string provider, string redirectUrl)
        {
            return singInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }

        public async Task<string> GenerateEmailConfirmationToken(DotNetUser user)
        {
            return await userManager.GenerateEmailConfirmationTokenAsync(user);
        }
        #endregion

        #region Password
        public async Task<IdentityResult> ChangePassword(DotNetUser user, string currentPassword, string newPassword)
        {
            return await userManager.ChangePasswordAsync(await GetCurrentUser(),
                    currentPassword, newPassword);
        }
        public async Task<DotNetUser> GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext.User;
            return await userManager.GetUserAsync(user);
        }
        public async Task RefreshSignIn(DotNetUser user)
        {
            await singInManager.RefreshSignInAsync(user);
        }

        public async Task<string> GeneratePasswordResetToken(DotNetUser user)
        {
            return await userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(DotNetUser user, string token, string newPassword)
        {
            return await userManager.ResetPasswordAsync(user, token, newPassword);
        }
        #endregion

        #region GetUser
        public string GetCurrentUserId()
        {
            if(_httpContextAccessor != null)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            return "";
            
        }

        public async Task<bool> IsAlredyEmailExist(string email, string userId)
        {
            return await context.Users.AnyAsync(u => u.Email == email && u.Id != userId);
        }
        public async Task<DotNetUser> UpdateUser(DotNetUser user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return user;
        }

        #endregion

        #region Roles
        public SelectList GetRoles()
        {
            return new SelectList(roleManager.Roles, "Id", "Name");
        }
        public string GetRoleName(string UserId)
        {
            var identityUserRole = context.UserRoles.Where(us => us.UserId == UserId).FirstOrDefault();

            return identityUserRole == null ? "" : identityUserRole.RoleId;
        }
        public async Task AddToRole(DotNetUser identityUser, string role)
        {
            await userManager.AddToRoleAsync(identityUser, role);
        }
        public void DeleteOldRole(string UserId)
        {
            var userRole = context.UserRoles.Where(us => us.UserId == UserId).FirstOrDefault();
            if (userRole != null)
            {
                context.UserRoles.Remove(userRole);
                context.SaveChanges();
            }

        }
        public async Task UpdateRole(DotNetUser identityUser, string roleId)
        {
            DeleteOldRole(identityUser.Id);

            var role = await roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                await userManager.AddToRoleAsync(identityUser, role.Name);
            }
        }

        public string GetCurrentUserRole()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }
        #endregion
    }
}
