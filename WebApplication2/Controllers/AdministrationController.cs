using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;
using Microsoft.AspNetCore.Hosting;
using Spire.Doc;
using Spire.Doc.Documents;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Dynamic;
using System.Globalization;

namespace WebApplication2.Controllers
{

    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        OrderContext db;
        private readonly IHostingEnvironment hostingEnvironment;
        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                            UserManager<IdentityUser> userManager, OrderContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            db = context;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            if (User.IsInRole("Admin"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Watch", "Home");
            }

        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult ListRoles()
        {
            if (User.IsInRole("Admin"))
            {
                var roles = roleManager.Roles;
                return View(roles);
            }
            else
            {
                return RedirectToAction("Watch", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {

            if (User.IsInRole("Admin"))
            {
                var role = await roleManager.FindByIdAsync(id);
                var model = new EditRoleViewModel
                {
                    Id = role.Id,
                    RoleName = role.Name
                };
                if (role == null)
                {
                    ViewBag.ErrorMessage = id;
                    return View("azaza");
                }

                foreach (var user in userManager.Users)
                {
                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        model.Users.Add(user.UserName);
                    }

                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Watch", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if (User.IsInRole("Admin"))
            {
                var role = await roleManager.FindByIdAsync(model.Id);
                if (role == null)
                {
                    return View("azaza");
                }
                else
                {
                    role.Name = model.RoleName;
                    var result = await roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }
                }

            }
            else
            {
                return RedirectToAction("Watch", "Home");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            if (User.IsInRole("Admin"))
            {
                ViewBag.roleId = roleId;
                var role = await roleManager.FindByIdAsync(roleId);

                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Role with id{roleId} cannot be found";
                    return View("NotFound");
                }
                var model = new List<UserRoleViewModel>();
                foreach (var user in userManager.Users)
                {
                    var userRoleViewModel = new UserRoleViewModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };
                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        userRoleViewModel.IsSelected = true;
                    }
                    else
                    {
                        userRoleViewModel.IsSelected = false;
                    }
                    model.Add(userRoleViewModel);
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Watch", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id={roleId} cannot be found";
                return View();
            }
            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!(model[i].IsSelected) && (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }
            return RedirectToAction("EditRole", new { Id = roleId });
        }
        public IActionResult Watch(string sortOrder, string articolo, string client,string year, DateTime date,string machine,bool accepted,bool declined,bool not, int id,bool accepted_u,string numord,bool closed)
        {
            if (User.IsInRole("Admin") || User.IsInRole("User"))
            {
                var stagio = from s in db.Stagio
                             orderby s.Expr1 descending
                             select s.Expr1;
                SelectList years = new SelectList(stagio);
                ViewBag.Years = years;

                    var all = from s in db.News
                              where s.Codsta.ToString().Contains(year)
                              select s;
                    var reparti = from s in db.Repartis
                                  select s;
                IQueryable<Opa> align = from p in db.News
                                        where p.Codsta.ToString().Contains(year)
                                        join t in db.Repartis on p.Reparto equals t.Jrres
                                        join f in db.Colori on p.Colore equals f.Valore
                                        join kio in db.Checks on (p.Numord + "/" + p.Rigord) equals kio.TaskNum
                                        into var1
                                        from kio in var1.DefaultIfEmpty()
                                        select new Opa
                                        {
                                            Id = kio.Id,
                                            Articolo = p.Articolo,
                                            Color = p.Colore + "-" + f.Descrizione,
                                            TaskNum = p.Numord.ToString() + "/" + p.Rigord.ToString(),
                                            Client = p.Codsot,
                                            Total = p.QtaOrd.ToString(),
                                            Machine = t.Jrdesc,
                                            Year = p.Codsta,
                                            Status = kio.Status,
                                            Stato = p.StatoOperazione,
                                            Selection = kio.Selection.ToString(),
                                            Linea= p.Linea,
                                            Fustella = p.Fustella
                                            };

                    if (!String.IsNullOrEmpty(articolo))
                    {
                        align = align.Where(s => s.Articolo.Contains(articolo));
                    }
                    if (!String.IsNullOrEmpty(client))
                    {
                        align = align.Where(s => s.Client.Contains(client));
                    }
                    if (!String.IsNullOrEmpty(machine))
                    {
                        align = align.Where(s => s.Machine.Contains(machine));
                    }
                    if (!String.IsNullOrEmpty(numord))
                    {
                        align = align.Where(s => s.TaskNum.Contains(numord));
                    }
                    if (closed)
                    {
                    align = align.Where(s => s.Stato.Contains("Close"));
                    align = align.OrderByDescending(s=>s.Status);
                }
                    if (accepted && !declined && !not && !accepted_u)
                    {
                        align = align.Where(s => s.Status.Contains("a"));
                    align = align.OrderByDescending(s=>s.Status);
                }
                    if (!accepted && declined && !not && !accepted_u)
                    {
                        align = align.Where(s => s.Status.Contains("c"));
                    align = align.OrderByDescending(s=>s.Status);
                }
                    if (!accepted && !declined && !not && accepted_u)
                    {
                        align = align.Where(s => s.Status.Contains("b"));
                    align = align.OrderByDescending(s=>s.Status);
                }
                    if ((!accepted && !declined && not && !accepted_u) || id == 1)
                    {
                        align = align.Where(s => s.Status.Contains("d"));
                    align = align.OrderByDescending(s=>s.Status);
                }
                    if (accepted && accepted_u && !declined && !not)
                    {
                        align = align.Where(s => s.Status.Contains("a") || s.Status.Contains("b"));
                    align = align.OrderByDescending(s=>s.Status);
                }
                    if (accepted && !accepted_u && declined && !not)
                    {
                        align = align.Where(s => s.Status.Contains("a") || s.Status.Contains("c"));
                    align = align.OrderByDescending(s=>s.Status);
                }
                    if (accepted && !accepted_u && !declined && not)
                    {
                        align = align.Where(s => s.Status.Contains("a") || s.Status.Contains("d"));
                    align = align.OrderByDescending(s=>s.Status);
                }
                    if (!accepted && accepted_u && declined && !not)
                    {
                        align = align.Where(s => s.Status.Contains("b") || s.Status.Contains("c"));
                    align = align.OrderByDescending(s=>s.Status);
                }
                    if (!accepted && accepted_u && !declined && not)
                    {
                        align = align.Where(s => s.Status.Contains("b") || s.Status.Contains("d"));
                    align = align.OrderByDescending(s=>s.Status);
                }
                    if (!accepted && !accepted_u && declined && not)
                    {
                        align = align.Where(s => s.Status.Contains("c") || s.Status.Contains("d"));
                    align = align.OrderByDescending(s=>s.Status);
                }
                    if (accepted && accepted_u && declined && !not)
                    {
                        align = align.Where(s => s.Status.Contains("b") || s.Status.Contains("c") || s.Status.Contains("a"));
                    align = align.OrderByDescending(s=>s.Status);
                }
                    if (!accepted && accepted_u && declined && not)
                    {
                        align = align.Where(s => s.Status.Contains("b") || s.Status.Contains("c") || s.Status.Contains("d"));
                    align = align.OrderByDescending(s=>s.Status);
                }
                    if (accepted && !accepted_u && declined && not)
                    {
                        align = align.Where(s => s.Status.Contains("a") || s.Status.Contains("c") || s.Status.Contains("d"));
                    align = align.OrderByDescending(s=>s.Status);
                }
                    if (accepted && accepted_u && !declined && not)
                    {
                        align = align.Where(s => s.Status.Contains("b") || s.Status.Contains("d") || s.Status.Contains("a"));
                    align = align.OrderByDescending(s=>s.Status);
                }
                    if (accepted && accepted_u && declined && not)
                    {
                        align = align.Where(s => s.Status.Contains("b") || s.Status.Contains("c") || s.Status.Contains("a") || s.Status.Contains("d"));
                    align = align.OrderByDescending(s=>s.Status);
                }
               // align = align.OrderByDescending(s=>s.Status);

                   
                    return View(align.ToList());
            }

            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpGet]
        public IActionResult View(int id)
        {
            ViewBag.Orders = from s in db.Checks
                             where s.Id == id select s;
            ViewBag.MinDef = from s in db.DefId
                             where s.IdCheck == id && s.Def == "min"
                             join p in db.Defects on s.DefectId equals p.DefectId
                             select new DefectsViewModel
                             {
                                 Comment = s.Comment,
                                 NameDef = p.Nameua
                             };
            ViewBag.MaxDef = from s in db.DefId
                             where s.IdCheck == id && s.Def == "max"
                             join p in db.Defects on s.DefectId equals p.DefectId
                             select new DefectsViewModel
                             {
                                 Comment = s.Comment,
                                 NameDef = p.Nameua
                             };

            ViewBag.Pics = from i in db.Users where i.Id == id select i;
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        public IActionResult View(string commentary,int id1)
        {
            (from p in db.Checks
             where p.Id == id1
             select p).ToList()
                                            .ForEach(x => x.Commentary = commentary);
            db.SaveChanges();
            return RedirectToAction(actionName: "Decision", routeValues: new { id=id1,status="decline" });
        }
        public IActionResult Decision(int id, string status)
        {
            History history = new History();
            if (status == "accept")
            {
                (from p in db.Checks
                 where p.Id == id
                 select p).ToList()
                                            .ForEach(x => x.Status = "a");
                string tasknum = (from p in db.Checks
                                 where p.Id == id
                                 select p.TaskNum).FirstOrDefault();
                string year = (from p in db.Checks
                               where p.Id == id
                               select p.Stagione).FirstOrDefault();
                history.Action = User.Identity.Name+" accepted " + tasknum;
                history.Date = DateTime.Now;
                history.TaskNum = tasknum;
                history.Status = "accept";
                history.Year = year;
                history.Wuser = User.Identity.Name;
                db.Histories.Add(history);
                db.SaveChanges();
            }
            else if (status == "decline") {
                (from p in db.Checks
                 where p.Id == id
                 select p).ToList()
                            .ForEach(x => x.Status = "c");
                string tasknum = (from p in db.Checks
                                  where p.Id == id
                                  select p.TaskNum).FirstOrDefault();
                string year = (from p in db.Checks
                               where p.Id == id
                               select p.Stagione).FirstOrDefault();
                history.Action = User.Identity.Name + " declined " + tasknum;
                history.Date = DateTime.Now;
                history.Status = "decline";
                history.TaskNum = tasknum;
                history.Year = year;
                history.Wuser = User.Identity.Name;
                db.Histories.Add(history);
                db.SaveChanges();
            }
            (from p in db.Checks
             where p.Id == id
             select p).ToList()
                    .ForEach(x => x.Date_checked = DateTime.Now);
            (from p in db.Checks
             where p.Id == id
             select p).ToList()
                    .ForEach(x => x.WhoChecked = User.Identity.Name);
            db.SaveChanges();
            return RedirectToAction("Watch", "Administration");
        }
        public IActionResult Report(int id)
        {
            if (User.IsInRole("Admin"))
            {
                string fileName = "report_" + DateTime.Now.ToString().Replace(":", ".");
                var report = from s in db.Checks
                             where s.Id == id
                             select s;
                var min_defects = from s in db.DefId
                                  where s.IdCheck == id && s.Def == "min"
                                  join i in db.Defects on s.DefectId equals i.DefectId
                                  select new DefectsViewModel
                                  {
                                      Comment=s.Comment,
                                      NameDef=i.Nameua
                                  };
                var max_defects = from s in db.DefId
                                  where s.IdCheck == id && s.Def == "max"
                                  join i in db.Defects on s.DefectId equals i.DefectId
                                  select new DefectsViewModel
                                  {
                                      Comment = s.Comment,
                                      NameDef = i.Nameua
                                  };
                string mindef = "";
                string maxdef = "";
                foreach(var item in min_defects)
                {
                    mindef += item.NameDef + " " + item.Comment + "/";
                }
                foreach(var item in max_defects)
                {
                    maxdef += item.NameDef + " " + item.Comment + "/";
                }
                foreach (var check in report)
                {
                    Document document = new Document();
                    document.LoadFromFile(@"C:\Users\1\Desktop\Temp\Last\WebApplication2\wwwroot\reports\Control.rtf");
                    document.Replace("<0>", check.Date.ToShortDateString().ToString(), true, true);
                    document.Replace("<1>", check.Articolo, true, true);
                    document.Replace("<2>", check.Color, true, true);
                    document.Replace("<3>", check.TaskNum, true, true);
                    document.Replace("<4>", check.Client, true, true);
                    document.Replace("<5>", check.Total.ToString(), true, true);
                    document.Replace("<6>", check.Selection.ToString(), true, true);
                    document.Replace("<7>", check.Machine, true, true);
                    if (mindef != "")
                    {
                        String[] MinDef = mindef.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                        int f = 34 + MinDef.Length;
                        for (int j = 34; j < MinDef.Length + 34; j++)
                        {
                            document.Replace($"<{j.ToString()}>", MinDef[j - 34], true, true);
                        }
                        while (f != 60)
                        {
                            document.Replace($"<{f.ToString()}>", "", true, true);
                            f++;
                        }
                    }
                    else
                    {
                        int f = 34;
                        while (f != 60)
                        {
                            document.Replace($"<{f.ToString()}>", "", true, true);
                            f++;
                        }
                    }
                    if(maxdef != "")
                    {
                        String[] MaxDef = maxdef.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                        int p = 8 + MaxDef.Length;
                        for (int i = 8; i < MaxDef.Length + 8; i++)
                        {
                            document.Replace($"<{i.ToString()}>", MaxDef[i - 8], true, true);
                        }
                        while (p != 34)
                        {
                            document.Replace($"<{p.ToString()}>", "", true, true);
                            p++;
                        }
                    }
                    else
                    {
                        int p = 8;
                        while (p != 34)
                        {
                            document.Replace($"<{p.ToString()}>", "", true, true);
                            p++;
                        }
                    }
                    document.SaveToFile($@"C:\Users\1\Desktop\Temp\Last\WebApplication2\wwwroot\reports\{fileName}.pdf", FileFormat.PDF);
                }
                return PhysicalFile($@"C:\Users\1\Desktop\Temp\Last\WebApplication2\wwwroot\reports\{fileName}.pdf", "application/pdf", $"{fileName}.pdf");
            } 
            else
            {
                return RedirectToAction("Watch", "Home");
            }

        }
        public IActionResult Delete(int id)
        {
            History history = new History();
            var delete = (from p in db.Checks
                         where p.Id == id
                         select p).FirstOrDefault();
            var photoDelete = from p in db.Users
                               where p.Id == id
                               select p;
            var defectDelete = from p in db.DefId
                               where p.IdCheck == id
                               select p;
            foreach(var item in defectDelete)
            {
                db.DefId.Remove(item);
            }
            foreach(var item in photoDelete)
            {
                System.IO.File.Delete(@$"wwwroot\images\{item.Name}");
                db.Users.Remove(item);
            }
            history.Action = User.Identity.Name + " deleted " + delete.TaskNum;
            history.Date = DateTime.Now;
            history.Status = "decline";
            history.Year = delete.Stagione;
            history.Wuser = User.Identity.Name;
            db.Histories.Add(history);
            db.Checks.Remove(delete);
            db.SaveChanges();
            return RedirectToAction("Watch","Administration");
        }

        [HttpGet]
        public IActionResult Statistics(string date1,string client,string linea,string fustella,string year,string machine)
        {
            var stagio = from s in db.Stagio
                         orderby s.Expr1 descending
                         select s.Expr1;
            SelectList years = new SelectList(stagio);
            ViewBag.Years = years;
            var linea1 = (from s in db.Checks
                         where s.Linea!=null
                         select s.Linea).Distinct();
            var fustella1 = (from s in db.Checks
                            where s.Fustella != null
                            select s.Fustella).Distinct();
            var cust = (from s in db.Checks
                        select s.Client).Distinct();
            var reparto = (from s in db.Checks
                           select s.Machine).Distinct();
            SelectList reparto1 = new SelectList(reparto);
            SelectList custo = new SelectList(cust);
            SelectList lineas = new SelectList(linea1);
            SelectList fustellas = new SelectList(fustella1);
            ViewBag.Linea = lineas;
            ViewBag.Fustella = fustellas;
            ViewBag.Customer = custo;
            ViewBag.Reparto = reparto1;
            DateTime from1 = DateTime.Now;
            DateTime to = DateTime.Now;
            string fro = "";
            string t = "";
            if (date1 != null)
            {
                String[] mas = date1.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                String[] from_start = fro.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                String[] to_end = t.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                 from1 = DateTime.ParseExact(mas[0].Trim(), "MM/dd/yyyy", null);
                to = DateTime.ParseExact(mas[1].Trim(), "MM/dd/yyyy", null);
                ViewBag.From = from1.ToShortDateString();
                ViewBag.To = to.ToShortDateString();
                DateTime smth = from1;
            }
            var check = from s in db.Checks
                        where s.Date_user >= from1 && s.Date_user <= to
                        select s;
            if (!String.IsNullOrEmpty(client))
            {
                check = check.Where(s => s.Client.Contains(client));
            }
            if (!String.IsNullOrEmpty(linea))
            {
                check = check.Where(s => s.Linea.Contains(linea));
            }
            if (!String.IsNullOrEmpty(fustella))
            {
                check = check.Where(s => s.Fustella.Contains(fustella));
            }
            if (!String.IsNullOrEmpty(year))
            {
                check = check.Where(s => s.Stagione.Contains(year));
            }
            if (!String.IsNullOrEmpty(machine))
            {
                check = check.Where(s => s.Machine.Contains(machine));
            }
            /*if(!empty){
             * check=check.Contains(s=>s.status);
             * }
             */
            var accepted = check.Where(s => s.Status == "a");
            var declined = check.Where(s => s.Status == "c");
            var okbycheck = check.Where(s => s.Status == "b");
            var uncheck = check.Where(s => s.Status == "d");
            var all_ac = accepted.Count() + declined.Count();
            /************************/
            var withdef = check.Where(s => s.Defect == '+');
            ViewBag.perwithdef = withdef.Count();
            double withoutdef = check.Where(s => s.Defect == '-').Count();
            double withdefper = Math.Round(withdef.Count() / (withdef.Count() + withoutdef) * 100, 2);
            double withdef_a = withdef.Where(s => s.Status == "a").Count();
            double withdef_c = withdef.Where(s => s.Status == "c").Count();
            double withdef_d = withdef.Where(s => s.Status == "d").Count();
            double withdef_a_per = Math.Round(withdef_a / withdef.Count() * 100, 2);
            double withdef_c_per = Math.Round(withdef_c / withdef.Count() * 100, 2);
            double withdef_d_per = 100 - (withdef_a_per + withdef_c_per);
            double withoutdefperr = Math.Round(100 - withdefper,2);
            ViewBag.Depo = withdefper.ToString().Replace(",",".");
            ViewBag.Depo1 = withoutdefperr.ToString().Replace(",", ".");
            ViewBag.A = withdef_a_per.ToString().Replace(",", ".");
            ViewBag.C = withdef_c_per.ToString().Replace(",", ".");
            ViewBag.D = withdef_d_per.ToString().Replace(",", ".");
            ViewBag.CountA = withdef_a;
            ViewBag.CountC = withdef_c;
            ViewBag.CountD = withdef_d;
            ViewBag.AllIn = check.Count();
            /***********************/
            var count1 = (from s in db.Checks
                          where s.Status == "d" || s.Status == "b"
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
            var count5 = (from s in db.Checks
                          where s.Status == "d"
                          select s).Count();
            ViewBag.Count5 = count5;
            DateTime date=new DateTime();
            
            count1 = (from s in db.Checks
                          where ((s.Status == "d" || s.Status == "b")&&s.Date_user>=from1&&s.Date_user<=to )
                          select s).Count();
            ViewBag.Count1 = count1;
            count2 = (from s in db.Checks
                      where ((s.Status == "a") && s.Date_user >= from1 && s.Date_user <= to)
                      select s).Count();
            ViewBag.Count2 = count2;
            count4 = (from s in db.Checks
                      where ((s.Status == "c") && s.Date_user >= from1 && s.Date_user <= to)
                      select s).Count();
            ViewBag.Count4 = count4;
            count5 = (from s in db.Checks
                      where ((s.Status == "d") && s.Date_user >= from1 && s.Date_user <= to)
                      select s).Count();
            var all = (from s in db.Checks
                      where (s.Date_user >= from1 && s.Date_user <= to)
                      select s).Count();
            var count7 = (from s in db.Checks
                          where ((s.Defect == '+') && s.Date_user >= from1 && s.Date_user <= to)
                          select s).Count();
            count3 = (from s in db.Checks
                          where ((s.Defect == '-') && s.Date_user >= from1 && s.Date_user <= to)
                      select s).Count();
            var count8 = (from s in db.Checks
                          where ((s.Status == "c") && s.Date_user >= from1 && s.Date_user <= to)
                          select s).Count();
            var count9 = count7 + count3;
            ViewBag.Count9 = count9;
            ViewBag.Count8 = count8;
            ViewBag.Count3 = count3;
            ViewBag.Count5 = count5;
            ViewBag.Added = all;
            ViewBag.Count5 = count5;
            ViewBag.Count7 = count7;
            string perDeff = (((double)count7 / (double)all) * 100).ToString();
            string perODeff = (((double)count3 / (double)all) * 100).ToString();
          /*  if (perDeff.Contains(",") || perODeff.Contains(","))
            {
                perDeff.Replace(",", "oeo");
                perODeff.Replace(",", "oreo");
            }*/
            ViewBag.perODeff = Math.Round(Convert.ToDouble(perODeff),2);
            double perAcc = ((double)count2 / (double)all) * 100;
            double perDec = ((double)count8 / (double)all) * 100;
            double perDef = ((double)count7 / (double)all) * 100;
            
            ViewBag.perDeff = Math.Round(perDef,2);
            ViewBag.perAcc = perAcc;
            ViewBag.perDec = perDec;
            ViewBag.perAcc = Math.Round(ViewBag.PerAcc, 2);
            ViewBag.perDec= Math.Round(ViewBag.PerDec, 2);


            return View();
        }
        [HttpGet]
        public IActionResult Defects()
        {
            ViewBag.Defects = from s in db.Defects
                              select s;
            return View();
        }
        [HttpGet]
        public IActionResult EditDefect(int id)
        {
            ViewBag.Defect = (from s in db.Defects
                             where s.DefectId == id
                             select s).FirstOrDefault();
            return View();
        }
        [HttpPost]
        public IActionResult EditDefect(int id,string NameUA,string NameIT, string NameEN)
        {
                (from p in db.Defects
                 where p.DefectId == id
                 select p).ToList()
                        .ForEach(x => x.Nameen = NameEN);

                (from p in db.Defects
                 where p.DefectId == id
                 select p).ToList()
                       .ForEach(x => x.Nameit = NameIT);
            

                (from p in db.Defects
                 where p.DefectId == id
                 select p).ToList()
                        .ForEach(x => x.Nameua = NameUA);
            
            db.SaveChanges();
            return RedirectToAction("Defects");
        }
        [HttpGet]
        public IActionResult Adddefect()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Adddefect(string NameUA,string NameIT,string NameEN)
        {
            Defects defect = new Defects
            {
                Nameen = NameEN,
                Nameit = NameIT,
                Nameua = NameUA
            };
            db.Defects.Add(defect);
            db.SaveChanges();
            return RedirectToAction("Defects");
        }
        [HttpGet]
        public IActionResult Actions(string? TaskNum,string? Status,string? Users)
        {
            if (User.IsInRole("Admin"))
            {
                var stagio = from s in db.Stagio
                             orderby s.Expr1 descending
                             select s.Expr1;
                var userList = (from s in db.Histories
                                select s.Wuser).Distinct();
                var taskList = (from s in db.Histories
                                select s.TaskNum).Distinct();
                var statusList = (from s in db.Histories
                                  select s.Status).Distinct();
                SelectList task = new SelectList(taskList);
                SelectList user = new SelectList(userList);
                SelectList status = new SelectList(statusList);
                SelectList years = new SelectList(stagio);
                ViewBag.TaskNum = task;
                ViewBag.User = user;
                ViewBag.Status = status;
                var Hist = from s in db.Histories
                           select s;
                if (!String.IsNullOrEmpty(TaskNum))
                {
                    Hist = Hist.Where(s => s.TaskNum.Contains(TaskNum));
                }
                if (!String.IsNullOrEmpty(Status))
                {
                    Hist = Hist.Where(s => s.Status.Contains(Status));
                }
                if (!String.IsNullOrEmpty(Users))
                {
                    Hist = Hist.Where(s => s.Wuser.Contains(Users));
                }
                ViewBag.History = Hist;
                return View();
            }
            else return RedirectToAction("Login", "Account");
            
        }
    }

}
