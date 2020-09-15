using Blog.Data;
using Blog.Data.FileManager;
using Blog.Data.Repository;
using Blog.Models;
using Blog.Models.Comments;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Home
{
    public class HomeController : Controller
    {
        private IRepository _repo;
        private IFileManager _fileManager;

        public HomeController(IRepository repo, IFileManager fileManager)
        {
            _repo = repo;
            _fileManager = fileManager;
            //var comment = new MainComment();

        }

        public IActionResult Index(int pageNumber, string category, string search)
        {
            if (pageNumber < 1)
            {
                return RedirectToAction("Index", new { pageNumber = 1, category });
            }
            var vm = _repo.GetAllPosts(pageNumber, category, search);
            return View(vm);
        }

        public IActionResult Post(int id) =>
          View(_repo.GetPost(id));


        [HttpGet("/Image/{image}")] //HttpGet path 
        [ResponseCache(CacheProfileName = "Monthly")]
        public IActionResult Image(string image) =>
           new FileStreamResult(_fileManager.ImageStream(image), $"image/{image.Substring(image.LastIndexOf(".") + 1)}");  //image/{mime} it is content type

        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel vm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Post", new { id = vm.PostId });

            var post = _repo.GetPost(vm.PostId);

            if (vm.MainCommentId == 0)
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();
                post.MainComments.Add(new MainComment
                {
                    Message = vm.Message,
                    Created = DateTime.Now
                });
                _repo.UpdatePost(post);
            }
            else
            {
                var comment = new SubComment
                {
                    MainCommentId = vm.MainCommentId,
                    Message = vm.Message,
                    Created = DateTime.Now
                };

                _repo.AddSubComment(comment);
            }

            await _repo.SaveChangesAsync();

            return RedirectToAction("Post", new { id = vm.PostId });
        }


        //public IActionResult Index(string category)

        //    return View(posts);
        //}


        //public IActionResult Post(int id)
        //{
        //    var post = _repo.GetPost(id);

        //    return View(post);
        //}


        //[controller]/Image/{image} i can use like this
        //[HttpGet("/Image/{image}")] //HttpGet path 
        //public IActionResult Image(string image)
        //{
        //    var mime = image.Substring(image.LastIndexOf(".") + 1); // you dont need "."
        //    return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mime}");//image/{mime} it is content type
        //}

    }
}
