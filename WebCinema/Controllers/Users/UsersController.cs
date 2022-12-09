using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using WebCinema.ViewModel.Users;

namespace WebCinema.Controllers.Users
{
    public class UsersController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_userManager.Users.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser newIdentityUser = new IdentityUser { Email = model.Email, UserName = model.Email };
                var result = await _userManager.CreateAsync(newIdentityUser, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Delete(string id)
        {
            IdentityUser identityUser = await _userManager.FindByIdAsync(id);
            if(identityUser == null)
            {
                return NotFound();
            }
            DeleteUserViewModel model = new DeleteUserViewModel
            {
                Email = identityUser.Email,
                Id = identityUser.Id,
                UserRoles = await _userManager.GetRolesAsync(identityUser)
            }; 
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(DeleteUserViewModel model)
        {
            IdentityUser identityUser = await _userManager.FindByIdAsync(model.Id);
            if(identityUser != null) 
            {
                IdentityResult result = await _userManager.DeleteAsync(identityUser);
            }
            return RedirectToAction("Index");
        }

    }
}
