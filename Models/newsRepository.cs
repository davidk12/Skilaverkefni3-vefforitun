using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Data.SqlClient;

namespace project_3.Models
{
    public class newsRepository
    {
        private List<Blog> blogs = new List<Blog>();            //The list which holds the 10 latest blogs.

        private blogDBDataContext db = new blogDBDataContext(); //db will give a way to access the database through LINQ.

        public IEnumerable<Blog> getLatestBlogs()               //Query the database for the 10 latest entries,
        {                                                       //and order the result by their date.
            var result = (from b in db.Blogs
                          orderby b.blogDate descending
                          select new { b.blogID, b.title, b.content, b.blogDate, b.category }).Take(10);

            foreach (var r in result)
            {
                Blog blog = new Blog()
                {
                    blogID = r.blogID,
                    title = r.title,
                    content = r.content,
                    blogDate = r.blogDate,
                    category = r.category
                };
                blogs.Add(blog);
            }
            return blogs;
        }

        public Blog get_blog_by_id(int? id)
        {
            var result = db.Blogs.FirstOrDefault(r => r.blogID == id);                  //Query the database for an entry which matches
            return result;                                                               //the id parameter.
        }

        public void add_blog(Blog b)
        {
            b.blogDate = DateTime.Now;                                                   
            db.Blogs.InsertOnSubmit(b);

            db.SubmitChanges();
        }

        public void edit_blog(Blog b)
        {
            var result = get_blog_by_id(b.blogID);              
            result.title = b.title;
            result.content = b.content;
            result.category = b.category;

            db.SubmitChanges();
        }
    }
}