using BilgeShop.Business.Services;
using BilgeShop.Data.Dto;
using BilgeShop.Data.Entities;
using BilgeShop.WebUI.Extensions;
using BilgeShop.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BilgeShop.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("Hesabim")]
        public IActionResult Index()
        {
            var loggedUserId = User.GetUserId();

            var user = _userService.GetUserById(loggedUserId);

            var viewModel = new AccountViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Address = user.Address
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Update(AccountViewModel formData)
        {
            if(!ModelState.IsValid)
            {
                return View("index" , formData);
            }

            var user = new UserDto
            {
                Id = formData.Id,
                FirstName = formData.FirstName,
                LastName = formData.LastName,
                Email = formData.Email,
                Address = formData.Address
            };

            _userService.UpdateUser(user);
            TempData["UpdateMessage"] = "Profil bilgileri güncellendi.";

            return RedirectToAction("index");
        }
    }
}
