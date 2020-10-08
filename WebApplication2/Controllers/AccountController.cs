using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication2.Controllers
{

    public class AccountController : Controller
    {
        OrderContext db;
        public UserManager<IdentityUser> userManager { get; }
        public SignInManager<IdentityUser> signInManager { get; }
        public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager,OrderContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            db = context;
        }

       

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
           await signInManager.SignOutAsync();
            return RedirectToAction("login", "account");
        }
        [HttpPost]
        public async  Task<IActionResult> Registration(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                   await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            SqlConnection conn1 = new SqlConnection("Data Source=192.168.204.41\\Bucket;Initial Catalog=UA_Stage;Persist Security Info=True;User ID=swstage;Password=@UAStage2019.");
            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = conn1;
            conn1.Open();
            cmd1.CommandType = CommandType.Text;
            cmd1.CommandText = $"SELECT COUNT(*) FROM [ifx].[get_login]('{model.Login}', '{model.Password}', 8)";
            object reader = cmd1.ExecuteScalar();
            conn1.Close();
            if (/*ModelState.IsValid*/ (int)reader == 1)
            {
                var result2 = await signInManager.PasswordSignInAsync(model.Login, model.Password,
                model.RememberMe, false);
                if (!result2.Succeeded)
                {
                    var user = new IdentityUser { UserName = model.Login };
                    var result1 = await userManager.CreateAsync(user, model.Password);
                    var result11= await userManager.AddToRoleAsync(user, "User");
                    if (result1.Succeeded)
                    {
                        var result3= await signInManager.PasswordSignInAsync(model.Login, model.Password,
                model.RememberMe, false);
                        if (result3.Succeeded)
                        {
                            return RedirectToAction("addition", "home");
                        }
                    }
                }
                else
                {
                    return RedirectToAction("addition", "home");
                }
               
                    ModelState.AddModelError("", "Invalid Login Attempt");
                
            }

            return View(model);
        }
    }
}
