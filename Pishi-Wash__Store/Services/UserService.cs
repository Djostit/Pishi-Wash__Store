namespace Pishi_Wash__Store.Services
{
    public class UserService
    {
        private readonly TradeContext _tradeContext;
        public UserService(TradeContext tradeContext)
        {
            _tradeContext = tradeContext;
        }
        public async Task<bool> AuthorizationAsync(string username, string password)
        {
            var user = await _tradeContext.Users.SingleOrDefaultAsync(u => u.UserLogin == username);
            if (user == null)
                return false;
            if (user.UserPassword.Equals(password))
            {
                await _tradeContext.Roles.ToListAsync();
                UserSetting.Default.UserName = user.UserName;
                UserSetting.Default.UserSurname = user.UserSurname;
                UserSetting.Default.UserPatronymic = user.UserPatronymic;
                UserSetting.Default.UserRole = user.UserRoleNavigation.RoleName;
                return true;
            }
            return false;
        }
        public async Task<List<string>> GetAllUserLogin()
        {
            return await _tradeContext.Users.Select(u => u.UserLogin).AsNoTracking().ToListAsync();
        }
        public async Task AddNewUser(string UserName, string UserSurname, string UserPatronymic, string UserLogin, string UserPassword)
        {
            await _tradeContext.Users.AddAsync(new User 
            {
                UserName = UserName,
                UserSurname = UserSurname,
                UserPatronymic = UserPatronymic,
                UserLogin = UserLogin,
                UserPassword = UserPassword,
                UserRole = 2
            });
            await _tradeContext.SaveChangesAsync();
        }
    }
}
