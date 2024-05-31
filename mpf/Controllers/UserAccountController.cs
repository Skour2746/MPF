using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using mpf.Models;
namespace mpf.Controllers
{
    public class UserAccountController : Controller
    {
        mpfdbEntities1 dbcontext = new mpfdbEntities1();
        // GET: UserAccount


        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult UserRegister()
        {
            return RedirectToAction("UserRegister", "UserAccount");
        }
        [HttpPost]
        public ActionResult UserRegister(UserRegister user)
        {
            if (ModelState.IsValid)
            {
                user.rts = DateTime.Now;
                UserRegister userss = dbcontext.UserRegisters.Add(user);
                if (userss != null)
                {
                    dbcontext.SaveChanges();
                    ModelState.Clear();
                    ViewBag.status = "You are Registered";
                }
            }

            return RedirectToAction("Index", "UserAccount");
        }

        [HttpGet]
        public ActionResult UserLogin()
        {
            return RedirectToAction("UserLogin", "UserAccount");

        }

        [HttpPost]
        public ActionResult UserLogin(UserRegister e)
        {

            var user = dbcontext.UserRegisters
               .FirstOrDefault(u => u.useremail == e.useremail && u.userpass == e.userpass);

            if (user != null)
            {
                Session["id"] = user.id;
                return RedirectToAction("Professionls", "UserAccount");
            }
            else
            {
                ViewBag.status = "invalid username or password";
                return RedirectToAction("Index", "UserAccount");
            }
        }

        public ActionResult DashBoard()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Professionls()
        {
            int? userId = Session["id"] as int?;

            UserRegister user = dbcontext.UserRegisters.Find(userId);

            if (user == null)
            {
                ViewBag.status = "User not found";
                return RedirectToAction("Index", "UserAccount"); // Redirect to the home page or handle as needed
            }
            List<SelectListItem> categorylist = dbcontext.Categories.Select(d => new SelectListItem
            {
                Value = d.id.ToString(),
                Text = d.CategoryName
            }).ToList();
            ViewBag.ProfessionId = categorylist;

            List<SelectListItem> stateListItems = dbcontext.Loc_State.Select(d => new SelectListItem
            {
                Value = d.State_Code.ToString(),
                Text = d.State_Name
            }).ToList();
            ViewBag.StateId = stateListItems;
            TempData["msg"] = ViewBag.ProfessionId;
            return View(user);
        }
        public JsonResult getDistrictById(int id)
        {
            var districtlist = dbcontext.Loc_Districtn.Where(m => m.State_Code == id);
            return Json(new SelectList(districtlist, "District_Code", "District_Name"), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Professionls(UserRegister professional, HttpPostedFileBase image, int? id)
        {
            int? userId = Session["id"] as int?;
            UserRegister user = dbcontext.UserRegisters.Find(userId);

            if (user == null)
            {
                ViewBag.status = "User not found";
                return RedirectToAction("Index");
            }
            professional.status = true;
            professional.rts = DateTime.Now;
            List<SelectListItem> categorylist = dbcontext.Categories.Select(d => new SelectListItem
            {
                Value = d.id.ToString(),
                Text = d.CategoryName
            }).ToList();
            ViewBag.ProfessionId = categorylist;

            List<SelectListItem> stateListItems = dbcontext.Loc_State.Select(d => new SelectListItem
            {
                Value = d.State_Code.ToString(),
                Text = d.State_Name
            }).ToList();
            ViewBag.StateId = stateListItems;
            professional.id = user.id;

            if (image != null && image.ContentLength > 0)
            {
                string fileName = user.id + "_1.jpg";
                var encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                System.Drawing.Image.FromStream(image.InputStream).Save(Server.MapPath("~/Content/mpfimages/Professional/" + fileName), ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid), encoderParams);
                System.Drawing.Image.FromStream(image.InputStream).GetThumbnailImage(495, 330, () => false, IntPtr.Zero).Save(Server.MapPath("~/Content/mpfimages/Professional/" + fileName));
                professional.image = fileName;
            }
            var file = "";
            file = id + "_1.jpg";
            dbcontext.Entry(user).CurrentValues.SetValues(professional);
            professional.image = file;
            if (dbcontext.SaveChanges() > 0)
            {

                ViewBag.status = "Edit Successful";
                return RedirectToAction("Professionls/0", "UserAccount");
            }
            else
            {
                return RedirectToAction("Professionls/0", "UserAccount");
            }
        }

        [HttpGet]
        public ActionResult Blog()
        {
            int? userId = Session["id"] as int?;

            UserRegister user = dbcontext.UserRegisters.Find(userId);

            if (user == null)
            {
                ViewBag.status = "User not found";
                return RedirectToAction("Index", "UserAccount"); // Redirect to the home page or handle as needed
            }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Blog(Blog blogss, HttpPostedFileBase image)
        {
            int? userId = Session["id"] as int?;
            UserRegister user = dbcontext.UserRegisters.Find(userId);
            int? maxId = dbcontext.Blogs.Max(entity => (int?)entity.id) ?? 0;
            int nextId = maxId.GetValueOrDefault() + 1;
            if (user == null)
            {
                ViewBag.status = "User not found";
                return RedirectToAction("Index");
            }
            blogss.UserId = userId.Value;
            blogss.status = true;
            blogss.rts = DateTime.Now;
            if (image != null && image.ContentLength > 0)
            {
                string fileName = nextId + "_1.jpg";
                var encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                System.Drawing.Image.FromStream(image.InputStream).Save(Server.MapPath("~/Content/mpfimages/Blog/" + fileName), ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid), encoderParams);
                System.Drawing.Image.FromStream(image.InputStream).GetThumbnailImage(495, 330, () => false, IntPtr.Zero).Save(Server.MapPath("~/Content/mpfimages/Blog/" + fileName));
                blogss.image = fileName;
            }
            Blog blogsss = dbcontext.Blogs.Add(blogss);
            if (blogsss != null)
            {
                dbcontext.SaveChanges();
                ModelState.Clear();
                ViewBag.status = "Data inserted";
            }

            return View();
        }

        public ActionResult BlogShow()
        {
            int? userId = Session["id"] as int?;
            UserRegister user = dbcontext.UserRegisters.Find(userId);

            if (user == null)
            {
                ViewBag.status = "User not found";
                return RedirectToAction("Index");
            }

            ViewBag.BlogShow = (from m in dbcontext.Blogs
                                join p in dbcontext.UserRegisters on m.UserId equals p.id
                                where m.UserId == userId // filtering based on UserId
                                select new BlogShow
                                {
                                    image = p.image,
                                    Heading = m.Heading,
                                    Description = m.Description,
                                    image1 = m.image,
                                    rts = m.rts
                                    // Make sure to include other properties you need
                                }).ToList();

            return View();
        }

        public ActionResult QuestionsPage()
        {
            int? userId = Session["id"] as int?;

            UserRegister user = dbcontext.UserRegisters.Find(userId);

            if (user == null)
            {
                ViewBag.status = "User not found";
                return RedirectToAction("Index", "UserAccount"); // Redirect to the home page or handle as needed
            }

            ViewBag.QuesList = dbcontext.UserQuesAns.ToList();
            return View();
        }
        [HttpGet]
        public ActionResult AnswerForm(int? id)
        {

            var model = new UserQuesAn();
            model = dbcontext.UserQuesAns.Find(id);

            return View(model);
        }
        [HttpPost]
        public ActionResult AnswerForm()
        {
         
            return View();
        }

    }


}
