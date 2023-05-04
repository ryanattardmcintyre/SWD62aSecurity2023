using DataAccess.Context;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UsersRepository
    {
        LibraryContext _context;
        public UsersRepository(LibraryContext context)
        {
            _context = context;

        }

        public IQueryable<IdentityUser> GetUsers()
        {
            return _context.Users;
        }

        public string GetUserId(string email)
        {
            return _context.Users.SingleOrDefault(x => x.Email == email).Id;
        }
    }
}
