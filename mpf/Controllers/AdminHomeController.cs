using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mpf.Models;

namespace mpf.Controllers
{
    public class AdminHomeController : Controller
    {
        mpfdbEntities1 dbcontext = new mpfdbEntities1();
        // GET: AdminHome
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Category(int? id)

        {
            var model = new Category();
            if (id != 0)
            {
                if (id != null)
                {
                    model = dbcontext.Categories.Find(id);
                    if (model == null)
                    {
                        HttpNotFound();
                    }

                }
            }
            model.Categorylist = dbcontext.Categories.ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Category(Category category, HttpPostedFileBase image, int? id)
        {
            var model = new Category();

            if (id == null)
            {
                if (ModelState.IsValid)
                {
                    category.status = true;
                    category.rts = DateTime.Now;
                    int? maxId = dbcontext.Categories.Max(entity => (int?)entity.id) ?? 0;
                    int nextId = maxId.GetValueOrDefault() + 1;
                    if (id == null)
                        if (image != null && image.ContentLength > 0)
                        {
                            string fileName = nextId + "_1.jpg";
                            var encoderParams = new EncoderParameters(1);
                            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                            System.Drawing.Image.FromStream(image.InputStream).Save(Server.MapPath("~/Content/mpfimages/category/" + fileName), ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid), encoderParams);
                            System.Drawing.Image.FromStream(image.InputStream).GetThumbnailImage(500, 300, () => false, IntPtr.Zero).Save(Server.MapPath("~/Content/mpfimages/category/" + fileName));
                            category.image = fileName;
                        }
                    Category cate = dbcontext.Categories.Add(category);
                    if (cate != null)
                    {
                        dbcontext.SaveChanges();
                        ModelState.Clear();
                        ViewBag.status = "Data Inserted";
                    }
                }
            }

            else
            {
                category.status = true;
                category.rts = DateTime.Now;
                id = category.id;

                if (image != null && image.ContentLength > 0)
                {
                    string fileName = id + "_1.jpg";
                    var encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                    System.Drawing.Image.FromStream(image.InputStream).Save(Server.MapPath("~/Content/mpfimages/category/" + fileName), ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid), encoderParams);
                    System.Drawing.Image.FromStream(image.InputStream).GetThumbnailImage(300, 300, () => false, IntPtr.Zero).Save(Server.MapPath("~/Content/mpfimages/category/" + fileName));
                    category.image = fileName;
                }
                var file = "";
                file = id + "_1.jpg";
                dbcontext.Entry(category).State = EntityState.Modified;
                category.image = file;
                if (dbcontext.SaveChanges() > 0)
                {
                    ViewBag.status = "Edit Successful";
                    return RedirectToAction("Category/0", "AdminHome");
                }
                else
                {
                    ViewBag.status = "Edit Not Successful";
                    return RedirectToAction("Category/0", "AdminHome");
                }


            }
            model.Categorylist = dbcontext.Categories.ToList();

            return View();
        }

        public ActionResult Delete(int id)
        {
            Category category = dbcontext.Categories.Find(id);
            dbcontext.Categories.Remove(category);
            if (dbcontext.SaveChanges() > 0)
            {
                ViewBag.status = "Data Deleted";
                return RedirectToAction("Category");
            }
            else
            {
                ViewBag.status = "Data Not Deleted";
                return RedirectToAction("Category");
            }
        }


        [HttpGet]
        public ActionResult Services(int? id)
        {
            var servicesmodel = new Service();

            if (id != 0)
            {
                if (id == null)
                {
                    List<SelectListItem> categorylist = dbcontext.Categories.Select(d => new SelectListItem
                    {
                        Value = d.id.ToString(),
                        Text = d.CategoryName
                    }).ToList();
                    ViewBag.ProfessionId = categorylist;
                }
                else
                {
                    servicesmodel = dbcontext.Services.Find(id);
                    List<SelectListItem> categorylist = dbcontext.Categories.Select(d => new SelectListItem
                    {
                        Value = d.id.ToString(),
                        Text = d.CategoryName
                    }).ToList();
                    ViewBag.ProfessionId = categorylist;

                }
            }
            else
            {
                List<SelectListItem> categorylist = dbcontext.Categories.Select(d => new SelectListItem
                {
                    Value = d.id.ToString(),
                    Text = d.CategoryName
                }).ToList();
                ViewBag.ProfessionId = categorylist;
            }

            servicesmodel.Serviceslist = dbcontext.Services.ToList();

            return View();
        }
        [HttpPost]
        public ActionResult Services(Service servicesmodels, int? id)
        {
            var servicesmodel = new Service();

            if (id == null)
            {
                servicesmodels.status = true;
                servicesmodels.rts = DateTime.Now;
                List<SelectListItem> categorylist = dbcontext.Categories.Select(d => new SelectListItem
                {
                    Value = d.id.ToString(),
                    Text = d.CategoryName
                }).ToList();
                ViewBag.ProfessionId = categorylist;
                Service servicesinsert = dbcontext.Services.Add(servicesmodels);
                if (servicesinsert != null)
                {
                    dbcontext.SaveChanges();
                    ModelState.Clear();
                    ViewBag.status = "Data Inserted";
                }
            }
            else
            {
                servicesmodels.status = true;
                servicesmodels.rts = DateTime.Now;
                List<SelectListItem> categorylist = dbcontext.Categories.Select(d => new SelectListItem
                {
                    Value = d.id.ToString(),
                    Text = d.CategoryName
                }).ToList();
                ViewBag.ProfessionId = categorylist;
                dbcontext.Entry(servicesmodels).State = EntityState.Modified;
                if (dbcontext.SaveChanges() > 0)
                {
                    ViewBag.status = "Edit Successfull";
                    return RedirectToAction("Services/0", "AdminHome");
                }
                else
                {
                    ViewBag.status = "Edit Successfull";
                    return RedirectToAction("Services/0", "AdminHome");
                }

            }
            servicesmodel.Serviceslist = dbcontext.Services.ToList();
            return View();

        }

        public ActionResult ServicesDelete(int id)
        {
            Service services = dbcontext.Services.Find(id);
            dbcontext.Services.Remove(services);
            if (dbcontext.SaveChanges() > 0)
            {
                ViewBag.status = "Data Deleted";
                return RedirectToAction("Services");
            }
            else
            {
                ViewBag.status = "Data Not Deleted";
                return RedirectToAction("Services");
            }
        }
        [HttpGet]
        public ActionResult Event(int? id)
        {
            var EventMgmts = new EventMgmt();
            if (id != 0)
            {
                if (id != null)
                {
                    EventMgmts = dbcontext.EventMgmts.Find(id);

                }
            }
            EventMgmts.Eventlist = dbcontext.EventMgmts.ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Event(EventMgmt events, HttpPostedFileBase image, int? id)
        {
            var eventmgnt = new EventMgmt();

            if (id == null)
            {
                if (ModelState.IsValid)
                {
                    events.status = true;

                    events.rts = DateTime.Now;
                    int? maxId = dbcontext.EventMgmts.Max(entity => (int?)entity.id) ?? 0;
                    int nextId = maxId.GetValueOrDefault() + 1; if (id == null)
                        if (image != null && image.ContentLength > 0)
                        {
                            string fileName = nextId + "_1.jpg";
                            var encoderParams = new EncoderParameters(1);
                            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                            System.Drawing.Image.FromStream(image.InputStream).Save(Server.MapPath("~/Content/mpfimages/EventMgmt/" + fileName), ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid), encoderParams);
                            System.Drawing.Image.FromStream(image.InputStream).GetThumbnailImage(300, 300, () => false, IntPtr.Zero).Save(Server.MapPath("~/Content/mpfimages/EventMgmt/" + fileName));
                            events.image = fileName;
                        }
                    EventMgmt even = dbcontext.EventMgmts.Add(events);
                    if (even != null)
                    {
                        dbcontext.SaveChanges();
                        ModelState.Clear();
                        ViewBag.status = "Data Inserted";
                    }
                }
            }
            else
            {
                events.status = true;
                events.rts = DateTime.Now;
                id = events.id;
                if (image != null && image.ContentLength > 0)
                {
                    string fileName = id + "_1.jpg";
                    var encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                    System.Drawing.Image.FromStream(image.InputStream).Save(Server.MapPath("~/Content/mpfimages/EventMgmt/" + fileName), ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid), encoderParams);
                    System.Drawing.Image.FromStream(image.InputStream).GetThumbnailImage(300, 300, () => false, IntPtr.Zero).Save(Server.MapPath("~/Content/mpfimages/EventMgmt/" + fileName));
                    events.image = fileName;
                }
                var file = "";
                file = id + "_1.jpg";
                dbcontext.Entry(events).State = EntityState.Modified;
                events.image = file;
                if (dbcontext.SaveChanges() > 0)
                {
                    ViewBag.status = "Edit Successfull";
                    return RedirectToAction("Event/0", "AdminHome");
                }
                else
                {
                    return RedirectToAction("Event/0", "AdminHome");
                }

            }
            eventmgnt.Eventlist = dbcontext.EventMgmts.ToList();
            return View();
        }

        public ActionResult EventDelete(int id)
        {
            EventMgmt events = dbcontext.EventMgmts.Find(id);
            dbcontext.EventMgmts.Remove(events);
            if (dbcontext.SaveChanges() > 0)
            {
                ViewBag.status = "Data Deleted";
                return RedirectToAction("Event");
            }
            else
            {
                return RedirectToAction("Event");
            }
        }
        [HttpGet]
        public ActionResult UpcomingEvents(int? id)
        {
            var Upcoming = new Upcoming_Events();
            if (id != 0)
            {
                if (id != null)
                {
                    Upcoming = dbcontext.Upcoming_Events.Find(id);
                }
            }
            Upcoming.UpcomingEventsList = dbcontext.Upcoming_Events.ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult UpcomingEvents(Upcoming_Events events, HttpPostedFileBase image, int? id)
        {
            var UpEvents = new Upcoming_Events();

            if (id == null)
            {
                events.status = true;
                events.rts = DateTime.Now;
                int? maxId = dbcontext.Upcoming_Events.Max(entity => (int?)entity.id) ?? 0;
                int nextId = maxId.GetValueOrDefault() + 1;

                if (image != null && image.ContentLength > 0)
                {
                    string fileName = nextId + "_1.jpg";
                    var encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                    System.Drawing.Image.FromStream(image.InputStream).Save(Server.MapPath("~/Content/mpfimages/UpcomingEvents/" + fileName), ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid), encoderParams);
                    System.Drawing.Image.FromStream(image.InputStream).GetThumbnailImage(300, 300, () => false, IntPtr.Zero).Save(Server.MapPath("~/Content/mpfimages/UpcomingEvents/" + fileName));
                    events.image = fileName;
                }
                Upcoming_Events coming = dbcontext.Upcoming_Events.Add(events);
                if (coming != null)
                {
                    dbcontext.SaveChanges();
                    ModelState.Clear();
                    ViewBag.status = "Data Inserted";
                }
            }
            else
            {
                events.status = true;
                events.rts = DateTime.Now;
                id = events.id;
                if (image != null && image.ContentLength > 0)
                {
                    string fileName = id + "_1.jpg";
                    var encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                    System.Drawing.Image.FromStream(image.InputStream).Save(Server.MapPath("~/Content/mpfimages/UpcomingEvents/" + fileName), ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid), encoderParams);
                    System.Drawing.Image.FromStream(image.InputStream).GetThumbnailImage(300, 300, () => false, IntPtr.Zero).Save(Server.MapPath("~/Content/mpfimages/UpcomingEvents/" + fileName));
                    events.image = fileName;
                }
                var file = "";
                file = id + "_1.jpg";
                dbcontext.Entry(events).State = EntityState.Modified;
                events.image = file;
                if (dbcontext.SaveChanges() > 0)
                {
                    ViewBag.status = "Edit Successfull";
                    return RedirectToAction("UpcomingEvents/0");
                }
                else
                {
                    return RedirectToAction("UpcomingEvents/0");
                }

            }
            UpEvents.UpcomingEventsList = dbcontext.Upcoming_Events.ToList();
            return View();
        }
        public ActionResult UpcomingEventsDelete(int id)
        {
            Upcoming_Events events = dbcontext.Upcoming_Events.Find(id);
            dbcontext.Upcoming_Events.Remove(events);
            if (dbcontext.SaveChanges() > 0)
            {
                return RedirectToAction("UpcomingEvents");
            }
            else
            {
                return RedirectToAction("UpcomingEvents");
            }
        }

        [HttpGet]
        public ActionResult News(int? id)
        {
            var news = new NewsTbl();
            if (id != 0)
            {
                if (id != null)
                {
                    news = dbcontext.NewsTbls.Find(id);
                }
            }
            news.NewsList = dbcontext.NewsTbls.ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult News(NewsTbl news, int? id)
        {
            var newss = new NewsTbl();
            if (id == null)
            {
                news.status = true;
                news.rts = DateTime.Now;

                NewsTbl new1 = dbcontext.NewsTbls.Add(news);
                if (new1 != null)
                {
                    dbcontext.SaveChanges();
                    ModelState.Clear();
                    ViewBag.status = "Data Inserted";
                }
            }
            else
            {
                news.status = true;
                news.rts = DateTime.Now;
                dbcontext.Entry(news).State = EntityState.Modified;
                if (dbcontext.SaveChanges() > 0)
                {
                    return RedirectToAction("News/0");
                }
                else
                {
                    return RedirectToAction("News/0");
                }
            }
            news.NewsList = dbcontext.NewsTbls.ToList();
            //newss.NewsList = dbcontext.NewsTbls.Where(u=>u.status==true).ToList();
            return View();
        }
        public ActionResult NewsDelete(int id)
        {
            NewsTbl news = dbcontext.NewsTbls.Find(id);
            dbcontext.NewsTbls.Remove(news);
            if (dbcontext.SaveChanges() > 0)
            {
                return RedirectToAction("News");
            }
            else
            {
                return RedirectToAction("News");
            }
        }
        public ActionResult Activate(int userId)
        {
            var user = dbcontext.NewsTbls.Find(userId);
            if (user != null)
            {
                user.status = true;
                dbcontext.SaveChanges();
            }
            return RedirectToAction("News");
        }
        public ActionResult Deactivate(int userId)
        {
            var user = dbcontext.NewsTbls.Find(userId);
            if (user != null)
            {
                user.status = false;
                dbcontext.SaveChanges();
            }
            return RedirectToAction("News");
        }

        [HttpGet]
        public ActionResult Degree(int? id)
        {
            DegreeTbl degreesss = new DegreeTbl();
            if (id != 0)
            {
                if (id != null)
                {
                    degreesss = dbcontext.DegreeTbls.Find(id);
                }
            }
            degreesss.Degreelist = dbcontext.DegreeTbls.ToList();

            return View();
        }
        [HttpPost]
        public ActionResult Degree(DegreeTbl degree22, int? id)
        {
            DegreeTbl degreesss = new DegreeTbl();
            if (id == null)
            {
                if (ModelState.IsValid)
                {
                    degree22.status = true;
                    degree22.rts = DateTime.Now;
                    DegreeTbl Degreess = dbcontext.DegreeTbls.Add(degree22);
                    if (Degreess != null)
                    {
                        dbcontext.SaveChanges();
                        ModelState.Clear();
                        ViewBag.status = "Data inserted";
                    }
                }
            }
            else
            {
                degree22.status = true;
                degree22.rts = DateTime.Now;
                dbcontext.Entry(degree22).State = EntityState.Modified;
                if (dbcontext.SaveChanges() > 0)
                {
                    ViewBag.status = "Edit Successfull";
                    return RedirectToAction("Degree/0", "AdminHome");
                }
                else
                {
                    return RedirectToAction("Degree/0", "AdminHome");
                }
            }
            degreesss.Degreelist = dbcontext.DegreeTbls.ToList();

            return View();
        }

        public ActionResult DegreeDelete(int? id)
        {
            DegreeTbl degreeee = dbcontext.DegreeTbls.Find(id);
            dbcontext.DegreeTbls.Remove(degreeee);
            if (dbcontext.SaveChanges() > 0)
            {
                return RedirectToAction("Degree", "AdminHome");
            }
            else
            {
                return RedirectToAction("Degree", "AdminHome");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Slider(int? id)
        {
            var slides = new Slider();
            slides.slide = dbcontext.Sliders.ToList();
            if (id != null)
            {
                slides = dbcontext.Sliders.Find(id);
            }

            slides.slide = dbcontext.Sliders.ToList();
            return View(slides);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Slider(Slider sliderss,HttpPostedFileBase image, int? id)
        {
            var slides = new Slider();
            if (id == null)
            {
                if (ModelState.IsValid)
                {
                    sliderss.status = true;
                    sliderss.rts = DateTime.Now;
                    int? maxId = dbcontext.Categories.Max(entity => (int?)entity.id) ?? 0;
                    int nextId = maxId.GetValueOrDefault() + 1;
                    if (image != null && image.ContentLength > 0)
                    {
                        string fileName = nextId + "_1.jpg";
                        var encoderParams = new EncoderParameters(1);
                        encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                        System.Drawing.Image.FromStream(image.InputStream).Save(Server.MapPath("~/Content/mpfimages/Slider/" + fileName), ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid), encoderParams);
                        System.Drawing.Image.FromStream(image.InputStream).GetThumbnailImage(1200, 557, () => false, IntPtr.Zero).Save(Server.MapPath("~/Content/mpfimages/Slider/" + fileName));
                        sliderss.image = fileName;
                    }
                    Slider slidersss = dbcontext.Sliders.Add(sliderss);
                    if (slidersss != null)
                    {
                        dbcontext.SaveChanges();
                        ModelState.Clear();
                        ViewBag.status = "Data Inserted";
                    }
                }
            }
            else
            {
                sliderss.status = true;
                sliderss.rts = DateTime.Now;
                id = sliderss.id;
                if (image != null && image.ContentLength > 0)
                {
                    string fileName = id + "_1.jpg";
                    var encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                    System.Drawing.Image.FromStream(image.InputStream).Save(Server.MapPath("~/Content/mpfimages/Slider/" + fileName), ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid), encoderParams);
                    System.Drawing.Image.FromStream(image.InputStream).GetThumbnailImage(1200, 557, () => false, IntPtr.Zero).Save(Server.MapPath("~/Content/mpfimages/Slider/" + fileName));
                    sliderss.image = fileName;
                }
                var file = "";
                file = id + "_1.jpg";
                dbcontext.Entry(sliderss).State = EntityState.Modified;
                sliderss.image = file;
                if (dbcontext.SaveChanges() > 0)
                {
                    ViewBag.status = "Edit Successfull";
                    return RedirectToAction("Slider");
                }
                else
                {
                    return RedirectToAction("Slider");
                }
            }
            slides.slide = dbcontext.Sliders.ToList();

            return View(slides);
        }

        public ActionResult SliderDelete(int? id)
        {
            Slider Slidersss = dbcontext.Sliders.Find(id);
            dbcontext.Sliders.Remove(Slidersss);
            if (dbcontext.SaveChanges() > 0)
            {
                return RedirectToAction("Slider", "AdminHome");
            }
            else
            {
                return RedirectToAction("Slider", "AdminHome");
            }

            return View();
        }
    }


}