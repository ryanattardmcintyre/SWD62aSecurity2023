using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Context;
using Domain.Models;

namespace DataAccess.Repositories
{
    public class BooksRepository
    {
        LibraryContext _context;
        public BooksRepository(LibraryContext context)
        {
            _context = context;
        }


        public void Add(Book b)
        {
            _context.Books.Add(b);
            _context.SaveChanges();
        }

        public void Reserve(Reservation r)
        {
            _context.Reservations.Add(r);
            _context.SaveChanges();

        }
        
        public bool CanUserReserveBook(string isbn, string user_id)
        {
            if (_context.Permissions.Count(x => x.Isbn_Fk == isbn && x.User_Fk == user_id) == 0)
                return false;
            else return true;
        }

    }
}
