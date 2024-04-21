using ProyectoAutoescuelaMvc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProyectoAutoescuelaMvc.Services;
using System.Security.Claims;

namespace ProyectoAutoescuelaMvc.Controllers
{
	public class ManagedController : Controller
	{
		private ServiceAutoescuela service;

		public ManagedController(ServiceAutoescuela service)
		{ 
			this.service = service;
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginModel model) 
		{
			string token = await this.service.GetTokenAsync(model.UserName, model.Password);
			if (token == null)
			{
				ViewData["MENSAJE"] = "Usuario / Password Incorrectos";
				return View();
			}
			else 
			{
				//ViewData["MENSAJE"] = "Ya tienes tu token";
				HttpContext.Session.SetString("TOKEN", token);
				ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
				identity.AddClaim(new Claim(ClaimTypes.Name, model.UserName));
				identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, model.Password));
				identity.AddClaim(new Claim("TOKEN", token));
				ClaimsPrincipal userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(identity));
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
				{
					ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
				});
				return RedirectToAction("Index", "Home");

			}
			
		}

		public async Task<IActionResult> Logout()
		{ 
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}

	}
}
