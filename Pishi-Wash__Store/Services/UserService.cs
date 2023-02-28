namespace Pishi_Wash__Store.Services
{
    public class UserService
    {
        private readonly DataContext _context;
        public UserService(DataContext context)
        {
            _context = context;
        }
        public async Task<bool> AuthorizationAsync(string username, string password)
        {
            var user = await _context.User.SingleOrDefaultAsync(u => u.UserLogin == username);
            if (user == null)
                return false;
            if (user.UserPassword.Equals(password))
            {
                UserSetting.Default.Id = user.UserID;
                UserSetting.Default.UserName = user.UserName;
                UserSetting.Default.UserSurname = user.UserSurname;
                UserSetting.Default.UserPatronymic = user.UserPatronymic;
                UserSetting.Default.UserRole = user.UserRole;

                return true;
            }
            return false;
        }
    }
}
