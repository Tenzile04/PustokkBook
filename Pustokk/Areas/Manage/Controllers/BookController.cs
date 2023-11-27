using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustokk.DAL;
using Pustokk.Models;

namespace Pustokk.Areas.Manage.Controllers
{
    [Area("manage")]
    public class BookController : Controller
    {
        private readonly AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Book> books = _context.Books.ToList();
          
            return View(books);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            return View();
        }
        public IActionResult Create(Book book)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            if (!ModelState.IsValid) return View(book);

            if (!_context.Genres.Any(x => x.Id == book.GenreId))
            {
                ModelState.AddModelError("GenreId","Genre Not found");
                return View();
            }
            if (!_context.Authors.Any(x => x.Id == book.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Author Not found");
                return View();
            }
            
            bool check = false;
            if (book.TagIds != null)
            {
                foreach (var tagId in book.TagIds)
                {
                    if (!_context.Tags.Any(x => x.Id == tagId))
                    {
                        check = true;
                        break;
                    }
                }
            }
            if (check = true)
            {
                ModelState.AddModelError("TagId", "Tag not found");
                return View();
            }
            else
            {
                if (book.TagIds != null)
                {
                    foreach (var tagId in book.TagIds)
                    {
                        BookTag booktag = new BookTag
                        {
                            Book=book,
                            TagId = tagId,
                            
                        };
                        _context.BookTags.Add(booktag);
                    }
                }
            }

            _context.Books.Add(book);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        [HttpGet]
        public IActionResult Update(int id)
        {

            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            Book wantedbook=_context.Books.FirstOrDefault(b=>b.Id==id);
            if (wantedbook == null)
            {
                return NotFound();
            };
            return View(wantedbook);
        }
        [HttpPost]
        public IActionResult Update(Book book)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            if (!ModelState.IsValid) return View();

            Book existbook= _context.Books.FirstOrDefault(b => b.Id == book.Id);
            if (existbook == null) return NotFound();
            if (!_context.Genres.Any(g => g.Id == book.GenreId))
            {
                ModelState.AddModelError("GenreId", "Genre not found!");
                return View();
            }
            if (!_context.Authors.Any(x => x.Id == book.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Author not found!");
                return View();
            }

            //if (!_context.Tags.Any(t => t.Id == book.TagIds))
            //{
            //    ModelState.AddModelError("TagIds", "Tag not found!");
            //    return View();
            //}

            //bool check = false;
            //if (book.TagIds != null)
            //{
            //    foreach (var tagId in book.TagIds)
            //    {
            //        if (!_context.Tags.Any(x => x.Id == tagId))
            //        {
            //            check = true;
            //            break;
            //        }
            //    }
            //}
            //if (check = true)
            //{
            //    ModelState.AddModelError("TagId", "Tag not found");
            //    return View();
            //}
            //else
            //{
            //    if (book.TagIds != null)
            //    {
            //        foreach (var tagId in book.TagIds)
            //        {
            //            BookTag booktag = new BookTag
            //            {
            //                Book = book,
            //                TagId = tagId,

            //            };
            //            _context.BookTags.Add(booktag);
            //        }
            //    }
            //}



            existbook.Name = book.Name;
            existbook.Description = book.Description;
            existbook.SalePrice = book.SalePrice;
            existbook.CostPrice = book.CostPrice;
            existbook.DiscountPercent = book.DiscountPercent;
            existbook.IsAvailable = book.IsAvailable;
            existbook.Tax = book.Tax;
            existbook.Code = book.Code;
            existbook.AuthorId = book.AuthorId;
            existbook.GenreId = book.GenreId;
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {

            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            if (id == null) return NotFound();

            Book book = _context.Books.FirstOrDefault(b => b.Id == id);

            if (book == null) return NotFound();


            return View(book);
        }

        [HttpPost]
        public IActionResult Delete(Book book)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            Book wantedBook = _context.Books.FirstOrDefault(b => b.Id == book.Id);

            if (wantedBook == null)
            {
                return NotFound();
            }

            _context.Books.Remove(wantedBook);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
