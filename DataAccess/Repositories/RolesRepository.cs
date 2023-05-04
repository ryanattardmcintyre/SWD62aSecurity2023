using DataAccess.Context;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class RolesRepository
    {
        LibraryContext _context;
        public RolesRepository(LibraryContext context)
        {
            _context = context;
            
        }


        public IQueryable<IdentityRole>   GetRoles()
        {
            return _context.Roles;

        }
        public string GetRoleId(string roleName)
        {
           return _context.Roles.SingleOrDefault(x => x.Name == roleName).Id;
        }

        public void AllocateRole(string email , string roleName)
        {

            string userId = _context.Users.SingleOrDefault(x => x.Email == email).Id;

            string roleId = GetRoleId(roleName);
            _context.UserRoles.Add(new IdentityUserRole<string>() { RoleId = roleId, UserId = userId });
            _context.SaveChanges();
        
        }


    }
}
