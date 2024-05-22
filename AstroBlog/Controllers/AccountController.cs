using AstroBlog.Models.Domain;
using AstroBlog.Models.ViewModel;
using AstroBlog.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AstroBlog.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IUserRepository _userRepository;
        private readonly ITagRepository _tagrepository;

        public AccountController(UserManager<IdentityUser> userManager,
           SignInManager<IdentityUser> signInManager , IUserRepository userRepository , ITagRepository tagrepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _userRepository = userRepository;
            _tagrepository = tagrepository;
        }
        [HttpGet]
		public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
       {
            if (ModelState.IsValid)
            {
                var identityUser = new IdentityUser
                {
                    UserName = registerViewModel.Username,
                    Email = registerViewModel.Email
                };
                var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);
                if (identityResult.Succeeded)
                {
                    // assign this user the "User" role
                    var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");
                    
					Person person = new Person()
                    {
                        Id = Guid.Parse(identityUser.Id), Email = identityUser.Email, UserName = identityUser.UserName
                       
					};
                    //mapping tags back to domain model
					await _userRepository.AddAsync(person);

                    if (roleIdentityResult.Succeeded)
                    {
                        // Show success notification
                        return RedirectToAction("CheckTag", new { id = person.Id });
                    }
                }
            }
            // Show error notification
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CheckTag(Guid id)
        {
            var tags = await _tagrepository.GetAllAsync();
            var model = new UserViewModel()
            {
               Id=id, Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CheckTag(UserViewModel userViewModel)
        {
          
            var person = await _userRepository.GetAsync(userViewModel.Id);
			var selectedTags = new List<Tag>();
			//map tags from selected tags 
			foreach (var selectedTagId in userViewModel.SelectedTags)
			{
				var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
				var existingTag = await _tagrepository.GetAsync(selectedTagIdAsGuid);
				if (existingTag != null)
				{
					selectedTags.Add(existingTag);
				}
			}
			//mapping tags back to domain model
			person.Tags = selectedTags;
			await _userRepository.UpdateAsync(person);
			return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username,
                loginViewModel.Password, false, false);
            if (signInResult != null && signInResult.Succeeded)
            {
               
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
