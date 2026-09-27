using Microsoft.AspNetCore.Mvc;
using MakerCheckerMVC.Models;
using MakerCheckerMVC.Repositories;
using MakerCheckerMVC.Services;
namespace MakerCheckerMVC.Controllers;

public class AuthController : Controller
{
    private readonly UserRepository _repo;
    private readonly JwtService _jwt;

public AuthController(UserRepository repo, JwtService jwt)
{
    _repo = repo;
    _jwt = jwt;
}

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
public IActionResult Login(LoginModel model)
{
    var user = _repo.ValidateUser(model.UserName, model.Password);

    if (user == null)
    {
        ViewBag.Error = "Invalid Login";
        return View();
    }

    string token = _jwt.GenerateToken(user);

    Response.Cookies.Append("JwtToken", token,
        new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTimeOffset.Now.AddHours(2)
        });

    if (user.Role == "Maker")
        return RedirectToAction("Upload", "Maker");

    return RedirectToAction("Index", "Checker");
}

public IActionResult Logout()
{
    Response.Cookies.Delete("JwtToken");
    return RedirectToAction("Login");
}
}