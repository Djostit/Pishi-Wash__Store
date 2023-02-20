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
                Global.CurrentUser = new User 
                { 
                    Id = user.UserID, 
                    UserName = user.UserName, 
                    UserSurname = user.UserSurname, 
                    UserPatronymic = user.UserPatronymic, 
                    UserRole = user.UserRole 
                };
                return true;
            }
            return false;
        }
    }
}
