using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ITO.ViewModels;
using ITO.Models;
using Microsoft.AspNetCore.Identity;

namespace ITO.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]        
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register (RegisterViewModel model)
        {
            if (ModelState.IsValid)//если валидация прошла
            {
                User user = new User { Email = model.Email, UserName = model.Email, Year = model.Year };
                //добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //установка куки
                    await _signInManager.SignInAsync(user, false);// устанавливаем аутентификационные куки для добавленного пользователя.
                    // этот метод передается объект пользователя, который аутентифицируется, и логическое значение, указывающее,
                    //надо ли сохранять куки в течение продолжительного времени. И далее выполняем переадресацию на главную страницу приложения.
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);//
                    }
                }
            }
            return View(model);// Если добавление прошло неудачно, то добавляем к состоянию модели  
            //ModelState.AddModelError(string.Empty, error.Description);с помощью метода 
            //ModelState все возникшие при добавлении ошибки, и отправленная модель возвращается в представление.
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]       
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    //проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        //return Redirect(model.ReturnUrl);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}