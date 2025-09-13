using BL.Contracts;
using BL.DTOs;
using BL.Services;
using Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;
using Ui.Models;
using Ui.Services;

namespace Ui.Controllers
{

    public class AccountController : Controller
    {

        private readonly IUserService _userService;
        private readonly GenericApiClient _apiClient;


        public AccountController(IUserService userService, GenericApiClient apiClient)
        {
            _userService = userService;
            _apiClient = apiClient;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return View(registerDto);

            var result = await _userService.RegisterAsync(registerDto);

            if (result.Success)
            {
                return RedirectToAction("Login");
            }
            else 
            { 
                foreach (var error in result.Errors)           
                ModelState.AddModelError("", error);          
            }

            return View(registerDto);
        }


        public IActionResult Logout()
        {
            _userService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var currentUser = await _userService.GetUserByEmailAsync(loginDto.Email);

            var result = await _userService.LoginAsync(loginDto);
            if (result.Success)
            {
                if (currentUser.Role.ToLower() == "user")
                    return RedirectToRoute(new { controller = "Home", action = "Index" });

                else
                   return RedirectToRoute(new { area = "admin", controller = "Home", action = "Index" });
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid login attempt.";
            }

            return View(loginDto);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginDto loginDto)
        //{
        //    var currentUser = await _userService.GetUserByEmailAsync(loginDto.Email);

        //     var result = await _userService.LoginAsync(loginDto);

        //    if (result.Success)
        //    {
        //        return await LoginApiAsync(currentUser, loginDto);
        //    }

        //    return View(loginDto);
        //}


        //private async Task<IActionResult> LoginApiAsync(UserDto user, LoginDto loginDto)
        //{
        //    //// Call the login API using the generic client
        //    LoginApiModel apiResult = await _apiClient.PostAsync<LoginApiModel>("api/auth/login", loginDto);

        //    if (apiResult == null)
        //    {
        //        ModelState.AddModelError(string.Empty, "API error: Unable to process login.");
        //        return View(loginDto);
        //    }

        //    var accessToken = apiResult?.AccessToken.ToString();

        //    if (string.IsNullOrEmpty(accessToken))
        //    {
        //        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        //        return View(loginDto);
        //    }
        //    // Store the access token in the cookie (for subsequent requests)
        //    Response.Cookies.Append("AccessToken", accessToken, new CookieOptions
        //    {
        //        HttpOnly = false,
        //        Secure = true,
        //        Expires = DateTime.UtcNow.AddMinutes(15)  // Adjust token expiry based on your needs
        //    });

        //    Response.Cookies.Append("RefreshToken", apiResult!.RefreshToken.ToString(), new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Secure = true,
        //        Expires = DateTime.UtcNow.AddDays(7)
        //    });

        //    if (user.Role.ToLower() == "user")
        //       return RedirectToRoute(new { controller = "Home", action = "Index" });


        //     return RedirectToRoute(new { area = "admin", controller = "Home", action = "Index" });
        //}





    }
}
