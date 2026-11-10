
using Microsoft.AspNetCore.Mvc;
using RaceDay.Mvc.Models;
using RaceDay.Mvc.Services;
using RaceDay.Mvc.Security;

namespace RaceDay.Mvc.Controllers
{
    [SessionAuthorize("Organiser")]
    public class OrganiserEnrolmentsController : Controller
    {
        private readonly ApiService _api;

        public OrganiserEnrolmentsController(ApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var enrolments =
                    await _api.GetAsync<List<EnrolmentViewModel>>(
                        "api/enrolments");

                int organiserId = int.Parse(
                    HttpContext.Session.GetString("UserId")!);

                var myEnrolments = enrolments?
                    .Where(e =>
                        e.Category != null &&
                        e.Category.Event != null &&
                        e.Category.Event.OrganiserId == organiserId)
                    .ToList() ?? new List<EnrolmentViewModel>();

                return View(myEnrolments);
            }
            catch
            {
                TempData["Error"] =
                    "Unable to retrieve enrolments.";

                return View(new List<EnrolmentViewModel>());
            }
        }
    }
}
