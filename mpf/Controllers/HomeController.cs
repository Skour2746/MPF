using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mpf.Models;

namespace mpf.Controllers
{
    public class HomeController : Controller
    {
        mpfdbEntities1 dbcontext = new mpfdbEntities1();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.userlist = (from m in dbcontext.UserRegisters
                                join p in dbcontext.Categories on m.Profession equals p.id

                                select new jointest
                                {
                                    username = m.username,
                                    categoryname = p.CategoryName,
                                    Stateid = dbcontext.Loc_State.Where(state => state.State_Code == m.State).Select(state => state.State_Name).FirstOrDefault(),
                                    Districtid = dbcontext.Loc_Districtn.Where(district => district.District_Code == m.District).Select(district => district.District_Name).FirstOrDefault(),
                                    PhoneNumber = m.PhoneNumber,
                                    PhoneNumber2 = m.PhoneNumber2,
                                    Address = m.Address,
                                    image = m.image,
                                    Village = m.Village
                                }).ToList();
            ViewBag.Categoryitems = dbcontext.Categories.Take(3);


            ViewBag.BlogDetail = (from m in dbcontext.Blogs
                                  join p in dbcontext.UserRegisters on m.UserId equals p.id
                                  select new BlogDetail
                                  {
                                      image1 = p.image,
                                      Heading = m.Heading,
                                      Description = m.Description,
                                      image = m.image,
                                      username = p.username
                                      // Make sure to include other properties you need
                                  }).ToList();
            ViewBag.Sliderlist = dbcontext.Sliders.ToList();


            return View();
        }

        public ActionResult Category(int? page)
        {
            int pageSize = 10; // Number of items per page
            int pageNumber = (page ?? 1); // If no page is specified, default to page 1

            // Retrieve data from your data source
            var data = dbcontext.Categories.ToList();

            // Apply pagination
            ViewBag.Categoryitem = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Pass paged data and pagination information to the view
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)data.Count / pageSize);
            return View();
        }
        public ActionResult About()
        {
            ViewBag.TeamShow = dbcontext.UserRegisters.Take(3);
            return View();
        }
        public ActionResult Events()
        {
            ViewBag.EventShow = dbcontext.EventMgmts.ToList();
            return View();
        }

        public ActionResult BlogPage(int? page)
        {
            int pageSize = 9; // Number of items per page
            int pageNumber = (page ?? 1); // If no page is specified, default to page 1

            // Retrieve data from your data source
            var data = (from m in dbcontext.Blogs
                        join p in dbcontext.UserRegisters on m.UserId equals p.id
                        select new BlogDetail
                        {
                            image1 = p.image,
                            Heading = m.Heading,
                            Description = m.Description,
                            image = m.image,
                            username = p.username,
                            rts = m.rts
                            // Make sure to include other properties you need
                        }).ToList();

            // Apply pagination
            ViewBag.BlogDetail = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)data.Count / pageSize);
            return View();
        }
        public ActionResult Team()
        {
            ViewBag.Chairman = (from m in dbcontext.UserRegisters
                                join p in dbcontext.Categories on m.Profession equals p.id

                                select new jointest
                                {
                                    image=m.image,
                                    username = m.username,
                                    categoryname = p.CategoryName,
                                    Post = m.Post
                                });


            //ViewBag.Chairman = dbcontext.UserRegisters.ToList();
            return View();
        }
        public ActionResult CategoryShow()
        {
            ViewBag.CategoryShowPage = dbcontext.UserRegisters.ToList();
            return View();
        }
        public ActionResult TeamDetail(int? id)
        {
            if (id == null)
            {
                // Handle the case where id is not provided
                return RedirectToAction("Team", "Home"); // Redirect to some action like Index
            }
            ViewBag.TeamDetail = (from m in dbcontext.UserRegisters
                                  join p in dbcontext.Categories on m.Profession equals p.id
                                  where m.id == id  // Filtering based on the id parameter
                                  select new jointest
                                  {
                                      username = m.username,
                                      categoryname = p.CategoryName,
                                      Stateid = dbcontext.Loc_State.Where(state => state.State_Code == m.State).Select(state => state.State_Name).FirstOrDefault(),
                                      Districtid = dbcontext.Loc_Districtn.Where(district => district.District_Code == m.District).Select(district => district.District_Name).FirstOrDefault(),
                                      PhoneNumber = m.PhoneNumber,
                                      PhoneNumber2 = m.PhoneNumber2,
                                      Address = m.Address,
                                      image = m.image,
                                      Village = m.Village,
                                      Description = m.Description,
                                      Post = m.Post,
                                      useremail = m.useremail
                                  }).ToList(); // Use FirstOrDefault() if you expect only one result

            ViewBag.Bloglist = dbcontext.Blogs.Where(item => item.id == id);

            return View();
        }



        public ActionResult QandA(UserQuesAn QA)
        {
            QA.rts = DateTime.Now;
            List<SelectListItem> categorylist = dbcontext.Categories.Select(d => new SelectListItem
            {
                Value = d.id.ToString(),
                Text = d.CategoryName
            }).ToList();
            ViewBag.ProfessionId = categorylist;
            UserQuesAn user = dbcontext.UserQuesAns.Add(QA);
            if (user != null)
            {
                dbcontext.SaveChanges();
                ModelState.Clear();
            }
            return View();
        }
    }
}
