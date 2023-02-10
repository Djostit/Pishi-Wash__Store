using Microsoft.EntityFrameworkCore;
using Pishi_Wash__Store.Data;
using Pishi_Wash__Store.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pishi_Wash__Store.Services
{
    public class UserService
    {
        private readonly DataContext _context;
        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task Auth(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            if(user == null) { Debug.WriteLine("null"); }
            if (user.Password.Equals(password)) { Debug.WriteLine("done"); }
            else { Debug.WriteLine("password uncorrect"); }
        }
    }
}
