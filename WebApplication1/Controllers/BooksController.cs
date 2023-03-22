using DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Text.Encodings.Web;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BooksController : Controller
    {

        BooksRepository _booksRepository;
        ILogger<BooksController> _logger;
        public BooksController(BooksRepository booksRepository, ILogger<BooksController> logger) {
            _booksRepository = booksRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            //to do: allow the user to download the file from a private folder
            var list = _booksRepository.GetBooks();

            var output = from book in list
                         select new BookViewModel()
                         {
                             Isbn = book.Isbn,
                             Name = book.Name 
                         };

            return View(output);
        }
        [Authorize()]
        public IActionResult Details(string isbn) //isbn now will be encrypted
        {

            //decrypt the isbn

            var b = _booksRepository.GetBook(isbn);
            BookViewModel model = new BookViewModel()
            {  Isbn= b.Isbn, Name = b.Name , Path=b.Path, Year=b.Year };
            return View(model);
        }

        [Authorize()]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken()]
        public IActionResult Create(BookViewModel book, IFormFile ebook, [FromServices] IWebHostEnvironment host)
        {
            string fileName = "";
            _logger.LogInformation($"A new book with name {book.Name} is about to be created");
            if (ModelState.IsValid)
            {
                _logger.LogInformation($"Book with name {book.Name} had validations ok");

                if (ebook != null)
                {
                    //check for the file type; the file is actually a .pdf!
                    if (System.IO.Path.GetExtension(ebook.FileName) != ".pdf")
                    {
                        _logger.LogWarning($"Book with name {book.Name} is not a pdf, it was a {System.IO.Path.GetExtension(ebook.FileName)}");
                        TempData["error"] = "Only Pdfs are accepted";
                        return View(book);
                    }
                    _logger.LogInformation($"Book with name {book.Name} has a valid extension");
                    //25 50 44 46
                    byte[] pdfHeader = { 37, 80, 68, 70 };
                    using (var myFileStream = ebook.OpenReadStream())
                    {
                        int byteReadFromFile = 0;
                        foreach (byte b in pdfHeader)
                        {
                            byteReadFromFile = myFileStream.ReadByte();
                            if (byteReadFromFile != b)
                            {
                                TempData["error"] = "Only Pdfs are accepted";
                                _logger.LogWarning($"Book with name {book.Name} is not a pdf; it failed file header check");

                                return View(book);
                            }
                        }
                        myFileStream.Position = 0; //resets the position of the index within the file
                    }
                    _logger.LogInformation($"Book with name {book.Name} has a valid file header which confirms its a pdf");
                    _logger.LogCritical($"Book with name {book.Name} of size {ebook.Length} is about to be saved");

                    string absolutePath = host.ContentRootPath + "Files";
                    fileName = Guid.NewGuid() + ".pdf";
                    _logger.LogInformation($"Book with name {book.Name} is about to be saved in {absolutePath + "\\" + fileName}");
                    try
                    {
                        using (var myFileStream = ebook.OpenReadStream())
                        {
                            myFileStream.Position = 0; //resets the position of the index within the file
                            MemoryStream ms = new MemoryStream();
                            myFileStream.CopyTo(ms);
                            ms.Position = 0;

                            System.IO.File.WriteAllBytes(absolutePath + "\\" + fileName, ms.ToArray());
                        }
                    }catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Book with name {book.Name} while saving threw an exception");
                        TempData["error"] = "Book could not be saved. try again later or contact admin on ....";

                    }
                }
                else
                {
                    _logger.LogInformation($"Book with name {book.Name} does not contain any file");

                }

                //if validation is ok, save the file in "Files"
                _logger.LogCritical($"Book with name {book.Name} is about to be saved into db");
                //set the book path to be saved in db
                _booksRepository.Add(
                    new Domain.Models.Book()
                    {
                        Isbn = book.Isbn,
                        Name = HtmlEncoder.Default.Encode(book.Name),
                        Path = "\\Files\\"+fileName,
                        Year = book.Year
                    }

                    );

                _logger.LogInformation($"Book with name {book.Name} was saved into db");


                return RedirectToAction("Index");
            }
            else
            {
                string validationerrors = JsonConvert.SerializeObject( ModelState.Values);

                _logger.LogWarning($"Book with name {book.Name} had validations fail the check \n{validationerrors}");
                return View(book);
            }
        
        }
    }
}
