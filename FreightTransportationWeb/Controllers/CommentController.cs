using FreightTransportationWeb.Data;
using FreightTransportationWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace FreightTransportationWeb.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string id)
        {
			return View(_context.Comments.Where(a => a.AppUserCommentId == id).ToList());
        }
        // GET: ArticlesComments/Create
        public IActionResult Create(string id)
        {
            ViewBag.ContractorId = id;
            return View(_context.Comments.Where(a => a.AppUserCommentId == id).ToList());
        }

        [HttpPost]
        public IActionResult AddComment(string contractorId, int rating, string comment)
        {
            Comment newComment = new Comment()
            {
                AppUserCommentId = contractorId,
                PublishedDate = DateTime.Now,
                Comments = comment,
                Rating = rating
            };
            _context.Add(newComment);
            _context.SaveChanges();
			return RedirectToAction("Index","User");
        }
    }
}
