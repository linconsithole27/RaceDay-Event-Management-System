using Microsoft.AspNetCore.Mvc;
using RaceDay.Mvc.Models;
using RaceDay.Mvc.Services;
using RaceDay.Mvc.Security;




namespace RaceDay.Mvc.Controllers
{

    [SessionAuthorize("Organiser")]
    public class OrganiserCategoriesController : Controller
    {

        private readonly ApiService _api;

        public OrganiserCategoriesController(ApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index(int eventId)
        {
            try
            {
                var events = await _api.GetAsync<List<EventViewModel>>(
                    "api/events");

                int organiserId = int.Parse(
                    HttpContext.Session.GetString("UserId")!);

                bool ownsEvent = events?.Any(e =>
                    e.EventId == eventId &&
                    e.OrganiserId == organiserId) ?? false;

                if (!ownsEvent)
                    return Forbid();

                var categories =
                    await _api.GetAsync<List<CategoryViewModel>>(
                        $"api/events/{eventId}/categories");

                ViewBag.EventId = eventId;

                ViewBag.EventName = events!
                    .First(e => e.EventId == eventId).EventName;

                return View(categories ?? new List<CategoryViewModel>());
            }
            catch
            {
                TempData["Error"] = "Unable to retrieve categories.";
                return View(new List<CategoryViewModel>());
            }
        }

        [HttpGet]
        public IActionResult Create(int eventId)
        {
            ViewBag.EventId = eventId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            int eventId, CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EventId = eventId;
                return View(model);
            }

            try
            {
                var events = await _api.GetAsync<List<EventViewModel>>(
                    "api/events");

                int organiserId = int.Parse(
                    HttpContext.Session.GetString("UserId")!);

                if (!(events?.Any(e =>
                    e.EventId == eventId &&
                    e.OrganiserId == organiserId) ?? false))
                    return Forbid();

                bool success = await _api.PostAsync(
                    $"api/events/{eventId}/categories",
                    new { categoryName = model.CategoryName });

                if (success)
                {
                    TempData["Success"] = "Category created successfully.";
                    return RedirectToAction("Index", new { eventId });
                }

                ModelState.AddModelError("", "Unable to create category.");
            }
            catch
            {
                ModelState.AddModelError("", "API connection failed.");
            }

            ViewBag.EventId = eventId;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, int eventId)
        {
            var categories =
                await _api.GetAsync<List<CategoryViewModel>>(
                    $"api/events/{eventId}/categories");

            var category = categories?.FirstOrDefault(
                c => c.CategoryId == id);

            if (category == null)
                return NotFound();

            ViewBag.EventId = eventId;
            ViewBag.CategoryId = id;

            return View(new CategoryFormViewModel
            {
                CategoryName = category.CategoryName
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, int eventId, CategoryFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EventId = eventId;
                ViewBag.CategoryId = id;
                return View(model);
            }

            bool success = await _api.PutAsync(
                $"api/categories/{id}",
                new { categoryName = model.CategoryName });

            if (success)
                TempData["Success"] = "Category updated successfully.";

            return RedirectToAction("Index", new { eventId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int eventId)
        {
            bool success = await _api.DeleteAsync(
                $"api/categories/{id}");

            TempData[success ? "Success" : "Error"] =
                success
                    ? "Category deleted successfully."
                    : "Unable to delete category.";

            return RedirectToAction("Index", new { eventId });
        }
    }

}
