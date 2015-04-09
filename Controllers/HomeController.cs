using project_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace project_3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            newsRepository repository = new newsRepository();                  //Get the 10 latest entries from the database
            var blogEntries = repository.getLatestBlogs();                     //and return the view.
            return View(blogEntries);
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                newsRepository repository = new newsRepository();          
                var model = repository.get_blog_by_id(id);                      //Get the entry by it's ID  
                                                                                //And return the view.
                return View(model);
            }

            return null;
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection formData)               
        {
            newsRepository repository = new newsRepository();               
            Blog b = repository.get_blog_by_id(id);                             //Get the entry by it's ID
            if (b != null)                                                      //Check if it exists, if it does
            {                                                                   //try to edit it, if a form is empty 
                try                                                             //then return a view which includes a error message.
                {
                    UpdateModel(b);
                    repository.edit_blog(b);
                    return RedirectToAction("Index");
                }
                catch (SqlException)
                {
                    return View("EditFail");
                }
            }
            else                                                               //If the entry does not exist, then return a NotFound view
            {                                                                  
                return View("NotFound");
            }
        }

        [HttpGet]
        public ActionResult Create()                                         
        {
            return View(new Blog());
        }

        [HttpPost]
        public ActionResult Create(FormCollection formData)                    //Try to add a entry, if any of it's forms is empty
        {                                                                      //then catch the exception and return a CreateFail view.
            try
            {
                Blog b = new Blog();
                UpdateModel(b);
                newsRepository repository = new newsRepository();
                repository.add_blog(b);
                return RedirectToAction("Index");
            }
            catch (SqlException)
            {
                return View("CreateFail");
            }
        }
    }
}
