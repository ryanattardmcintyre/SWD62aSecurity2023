using DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text.Encodings.Web;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BooksController : Controller
    {

        BooksRepository _booksRepository;
        public BooksController(BooksRepository booksRepository) {
            _booksRepository = booksRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize()]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken()]
        public IActionResult Create(BookViewModel book)
        {
            if (ModelState.IsValid)
            {
                _booksRepository.Add(
                    new Domain.Models.Book()
                    {
                        Isbn = book.Isbn,
                        Name = HtmlEncoder.Default.Encode(book.Name),
                        Path = book.Path
                         ,
                        Year = book.Year
                    }

                    );
                
                return RedirectToAction("Index");
            }
            else
            {
                return View(book);
            }
        
        }
    }
}
