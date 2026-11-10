
using Microsoft.AspNetCore.Mvc;
using RaceDay.Mvc.Models;
using RaceDay.Mvc.Services;



namespace RaceDay.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _apiService;

        public AccountController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Role != "Organiser" &&
                model.Role != "Participant")
            {
                ModelState.AddModelError("Role", "Select a valid role.");
                return View(model);
            }

            try
            {
                string endpoint = model.Role == "Organiser"
                    ? "api/auth/login/organiser"
                    : "api/auth/login/participant";

                bool success = await _apiService.LoginAsync(endpoint, new
                {
                    email = model.Email,
                    password = model.Password
                });

                if (!success)
                {
                    ModelState.AddModelError("", "Login failed. Check your details and try again.");
                    return View(model);
                }

                var currentUser =
     await _apiService.GetAsync<CurrentUserDto>("api/auth/me");

                if (currentUser == null ||
                    string.IsNullOrEmpty(currentUser.UserId))
                {
                    HttpContext.Session.Clear();
                    ModelState.AddModelError("", "Unable to retrieve your account details.");
                    return View(model);
                }

                HttpContext.Session.SetString("UserId", currentUser.UserId);
                HttpContext.Session.SetString("UserName", currentUser.Name ?? "");
                HttpContext.Session.SetString("UserEmail", model.Email);
                HttpContext.Session.SetString("UserRole", currentUser.Role ?? "");

                if (model.Role == "Organiser")
                    return RedirectToAction("Index", "Organiser");

                return RedirectToAction("Index", "Participant");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to connect to the API. Check that RaceDay.Api is running.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Role != "Organiser" &&
                model.Role != "Participant")
            {
                ModelState.AddModelError("Role", "Select a valid role.");
                return View(model);
            }

            try
            {
                string endpoint;
                object data;

                if (model.Role == "Organiser")
                {
                    endpoint = "api/auth/register/organiser";

                    data = new
                    {
                        organiserName = model.Name,
                        organiserEmail = model.Email,
                        password = model.Password
                    };
                }
                else
                {
                    endpoint = "api/auth/register/participant";

                    data = new
                    {
                        participantName = model.Name,
                        participantEmail = model.Email,
                        password = model.Password
                    };
                }

                bool success = await _apiService.PostAsync(endpoint, data);

                if (!success)
                {
                    ModelState.AddModelError("", "Registration failed. The email may already be registered.");
                    return View(model);
                }

                TempData["Success"] = "Account created successfully. Please log in.";

                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to register. Check that the API is running.");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _apiService.PostAsync("api/auth/logout", new { });
            }
            catch
            {
                // Clear the local session even if the API is unavailable.
            }

            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    
    [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}