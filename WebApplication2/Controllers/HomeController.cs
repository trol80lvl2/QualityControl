using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail; 
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using WebApplication2.Models;


namespace WebApplication2.Controllers
{

    public class HomeController : Controller
    {
        OrderContext db;
        private readonly IHostingEnvironment hostingEnvironment;
        public UserManager<IdentityUser> userManager { get; }
        public HomeController(OrderContext context, IHostingEnvironment hostingEnvironment,UserManager<IdentityUser> userManager)
        {
            this.hostingEnvironment = hostingEnvironment;
            db = context;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            var count1 = (from s in db.Checks
                         where s.Status == "d"
                         select s).Count();
            ViewBag.Count1 = count1;
            var count2 = (from s in db.Checks
                          where s.Status == "a"
                          select s).Count();
            ViewBag.Count2 = count2;
            var count3 = (from s in db.Checks
                          where s.Status == "b"
                          select s).Count();
            ViewBag.Count3 = count3;
            var count4 = (from s in db.Checks
                          where s.Status == "c"
                          select s).Count();
            ViewBag.Count4 = count4;
            var credentials = new NetworkCredential("checkorder.rif1@gmail.com", "gritsin123");
            // Mail message
            var mail = new MailMessage()
            {
                From = new MailAddress("noreply@codinginfinite.com"),
                Subject = "Email Sender App",
                Body = $"You have {count1} unchecked reports\r\nhttps://localhost:44325/administration/watch/1"
            };
            mail.IsBodyHtml = true;
            mail.To.Add(new MailAddress("roman.it.rif1@gmail.com"));
            // Smtp client
            var client = new SmtpClient()
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Credentials = credentials
            };
            client.Send(mail);
            return View();


        }
        public  IActionResult Addition(string sortOrder,OrderCreateViewModel check,int Bolla,int Bolla2,string Bolla3,List<string> minidef, List<string> maxidef)
        {
            if (User.IsInRole("User"))
            {
                ExamplContext bd = new ExamplContext();
                PhotouContext bd1 = new PhotouContext();
                ViewBag.Defect = from s in db.Defects
                                 select s;
                ViewBag.Years = (from s in bd.Stagio
                                 orderby s.Expr1 descending
                                 select s);
                string TaskNum = Bolla.ToString() + "/" + Bolla2.ToString();
                var ord = (from s in db.Checks
                          where s.TaskNum == TaskNum && s.Stagione == Bolla3
                          select s).FirstOrDefault();
                ViewBag.hist = (from s in db.Histories
                            where s.TaskNum == TaskNum && s.Year == Bolla3
                            select s).FirstOrDefault();
                ViewBag.ord = ord;
                if (ord != null)
                {
                    HttpContext.Session.SetString("Status", ord.Status);
                    HttpContext.Session.SetString("Id", ord.Id.ToString());
                }

                if (check.Articolo != null)
                {
                    string uniqueFileName = null;
                    SqlConnection conn1 = new SqlConnection("Data Source=192.168.204.41\\Bucket;Initial Catalog=UA_Stage;Persist Security Info=True;User ID=swstage;Password=@UAStage2019.");
                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.Connection = conn1;
                    conn1.Open();
                    cmd1.CommandType = CommandType.Text;
                    cmd1.CommandText = $"SELECT MAX(Id) FROM [dbo].[checks]";
                    object reader = cmd1.ExecuteScalar();
                    FileInfo file;
                    MagickImage image;
                    string sthj = check.Date.ToString();
                    ImageOptimizer optimizer = new ImageOptimizer();            
                    
                    conn1.Close();
                    Check newCheck = new Check
                    {
                        Date = check.Date,
                        Articolo = check.Articolo,
                        Color = check.Color,
                        TaskNum = check.TaskNum,
                        Client = check.Client,
                        Total = check.Total,
                        Selection = check.Selection,
                        Machine = check.Machine,
                        UserId = User.Identity.Name.ToString(),
                        Status = "d",
                        Date_user = DateTime.Now,
                        Stagione = check.Stagione,
                        Linea = check.Linea,
                        Fustella = check.Fustella
                    };
                    History history = new History();
                    var status = HttpContext.Session.GetString("Status");
                    var idslav = HttpContext.Session.GetString("Id");

                    if (check.CanGo)
                    {
                        newCheck.Status = "b";
                        newCheck.Defect = '-';
                    }
                    else
                    {
                        newCheck.Defect = '+';
                    }
                    if (status!=null&&status=="c")
                    {
                        history.Date = DateTime.Now;
                        history.Status = "update";
                        history.Action = User.Identity.Name + " " + "updated " + check.TaskNum + " check";
                        history.TaskNum = check.TaskNum;
                        history.Wuser = User.Identity.Name;
                        db.Histories.Add(history);
                        newCheck.Status = "d";
                        (from i in db.Checks
                            where i.Id.ToString() == idslav
                            select i).ToList()
                                            .ForEach(x => x.Status=newCheck.Status);
                        (from i in db.Checks
                         where i.Id.ToString() == idslav
                         select i).ToList()
                    .ForEach(x => x.MinDef = newCheck.MinDef);
                        (from i in db.Checks
                         where i.Id.ToString() == idslav
                         select i).ToList()
                    .ForEach(x => x.MaxDef = newCheck.MaxDef);
                        db.SaveChanges();
                    }
                    else
                    {
                        history.Date = DateTime.Now;
                        history.Status = "add";
                        history.Action = User.Identity.Name + " " + "added " + check.TaskNum + " check";
                        history.TaskNum = check.TaskNum;
                        history.Wuser = User.Identity.Name;
                        db.Histories.Add(history);
                        db.Checks.Add(newCheck);
                    }
                    var id = 0;
                    db.SaveChanges();
                    int idLast = (from s in db.Checks
                                  orderby s.Id descending
                                  select s.Id).FirstOrDefault();
                    /*Defect block*/
                    if (minidef != null)
                    {
                        for(int i = 0; i < minidef.Count; i++)
                        {
                            DefId defio = new DefId();
                            String[] ArrSplit = minidef[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            defio.DefectId = Convert.ToInt32(ArrSplit[0].Replace(".",""));
                            for(int j = 1; j < ArrSplit.Length; j++)
                            {
                                defio.Comment += ArrSplit[j] + " ";
                            }
                            defio.Def = "min";
                            defio.IdCheck = idLast;
                            db.Add(defio);
                        }
                        db.SaveChanges();
                    }
                    if (maxidef != null)
                    {
                        for (int i = 0; i < maxidef.Count; i++)
                        {
                            DefId defio = new DefId();
                            String[] ArrSplit = maxidef[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            defio.DefectId = Convert.ToInt32(ArrSplit[0].Replace(".", ""));
                            for (int j = 1; j < ArrSplit.Length; j++)
                            {
                                defio.Comment += ArrSplit[j] + " ";
                            }
                            defio.Def = "max";
                            defio.IdCheck = idLast;
                            db.Add(defio);
                        }
                        db.SaveChanges();
                    }

                    /*Photo block*/
                    if (check.Photo != null && check.Photo.Count > 0)
                    {
                        foreach (IFormFile photos in check.Photo)
                        {
                            User photo = new User();
                            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                            uniqueFileName = Guid.NewGuid().ToString() + "_" + photos.FileName;
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                            using (var copy = new FileStream(filePath, FileMode.Create))
                            {
                                photos.CopyTo(copy);
                            }
                            using (image = new MagickImage(filePath))
                            {
                                image.Quality = 75;
                                image.Resize(1920, 1080);
                                image.Format = MagickFormat.Jpeg;
                                image.Write(filePath);
                            }
                            photo.Id = idLast;
                            photo.Name = uniqueFileName;
                            db.Users.Add(photo);
                            db.SaveChanges();
                        }
                    }


                    return RedirectToAction("Addition", "Home");
                }
                var colors = from u in bd.Colori select u;

                var slavik = (from k in bd.News where k.Numord == Bolla && k.Rigord == Bolla2 && k.Codsta.ToString().Contains(Bolla3) && k.Fase == "CONF" select k).FirstOrDefault();
               
                var slavik2 = (from i in bd.Colori
                              join k in bd.News on i.Valore equals k.Colore
                              where k.Numord == Bolla && k.Rigord == Bolla2 && k.Fase == "CONF"
                              select i).FirstOrDefault();
                var slavik3 = (from i in bd.Clients
                              join k in bd.News on i.Codclf equals k.Codsot
                              where k.Numord == Bolla && k.Rigord == Bolla2 && k.Fase == "CONF"
                              select i).FirstOrDefault();
                ViewBag.History = (from i in db.Histories
                                   orderby  i.Date descending
                                   select i).Take(20);
                ViewBag.Machine = from i in bd.Repartis
                                   where i.Jrres == slavik.Reparto
                                   select i;
                ViewBag.HistoryNum = from i in db.Histories
                                     where i.TaskNum == TaskNum && (i.Status=="decline"||i.Status=="accept"||i.Status=="update")
                                     select i;
                 ViewBag.Slavik = slavik;
                if (slavik != null)
                {
                    ViewBag.Ispe = Math.Round((decimal)ViewBag.Slavik.QtaOrd / 10);
                }
                ViewBag.Client = slavik3;

                //var colors = from u in bd.Colori where u.Valore==check.Color select u;
                var color = from u in bd.Colori select u;

                ViewBag.Color = slavik2;


                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";


                ViewBag.Bolla = Bolla;


                return View();
            }
            else return RedirectToAction("Login", "Account");

        }
        public IActionResult Watch(string sortOrder, bool accepted, bool declined, bool accepted_u, bool not, string year)
        {
            ExamplContext bd = new ExamplContext();
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var orders = from s in db.Checks
                         where s.Stagione.Contains(year)
                         select s;
            var stagio = from s in bd.Stagio
                         orderby s.Expr1 descending
                         select s.Expr1;
            SelectList years = new SelectList(stagio);
            ViewBag.Years = years;

            if (accepted && !declined && !not && !accepted_u)
            {
                orders = orders.Where(s => s.Status.Contains("a"));
            }
            if (!accepted && declined && !not && !accepted_u)
            {
                orders = orders.Where(s => s.Status.Contains("c"));
            }
            if (!accepted && !declined && !not && accepted_u)
            {
                orders = orders.Where(s => s.Status.Contains("b"));
            }
            if ((!accepted && !declined && not && !accepted_u))
            {
                orders = orders.Where(s => s.Status.Contains("d"));
            }
            if (accepted && accepted_u && !declined && !not)
            {
                orders = orders.Where(s => s.Status.Contains("a") || s.Status.Contains("b"));
            }
            if (accepted && !accepted_u && declined && !not)
            {
                orders = orders.Where(s => s.Status.Contains("a") || s.Status.Contains("c"));
            }
            if (accepted && !accepted_u && !declined && not)
            {
                orders = orders.Where(s => s.Status.Contains("a") || s.Status.Contains("d"));
            }
            if (!accepted && accepted_u && declined && !not)
            {
                orders = orders.Where(s => s.Status.Contains("b") || s.Status.Contains("c"));
            }
            if (!accepted && accepted_u && !declined && not)
            {
                orders = orders.Where(s => s.Status.Contains("b") || s.Status.Contains("d"));
            }
            if (!accepted && !accepted_u && declined && not)
            {
                orders = orders.Where(s => s.Status.Contains("c") || s.Status.Contains("d"));
            }
            if (accepted && accepted_u && declined && !not)
            {
                orders = orders.Where(s => s.Status.Contains("b") || s.Status.Contains("c") || s.Status.Contains("a"));
            }
            if (!accepted && accepted_u && declined && not)
            {
                orders = orders.Where(s => s.Status.Contains("b") || s.Status.Contains("c") || s.Status.Contains("d"));
            }
            if (accepted && !accepted_u && declined && not)
            {
                orders = orders.Where(s => s.Status.Contains("a") || s.Status.Contains("c") || s.Status.Contains("d"));
            }
            if (accepted && accepted_u && !declined && not)
            {
                orders = orders.Where(s => s.Status.Contains("b") || s.Status.Contains("d") || s.Status.Contains("a"));
            }
            if (accepted && accepted_u && declined && not)
            {
                orders = orders.Where(s => s.Status.Contains("b") || s.Status.Contains("c") || s.Status.Contains("a") || s.Status.Contains("d"));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    orders = orders.OrderByDescending(s => s.Client);
                    break;
                case "Date":
                    orders = orders.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(s => s.Date);
                    break;
                default:
                    orders = orders.OrderBy(s => s.Client);
                    break;
            }
            orders = orders.OrderBy(s => s.Status);
            return View(orders.ToList());
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Orders = from s in db.Checks where s.Id == id select s;
            ViewBag.Pics = from i in db.Users where i.Id == id select i;
            ViewBag.Id = id;
            return View();
        }

    }
}