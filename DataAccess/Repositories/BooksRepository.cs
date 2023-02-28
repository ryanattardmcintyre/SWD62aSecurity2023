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

        
    }
}
