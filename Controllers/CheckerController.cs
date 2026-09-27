using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MakerCheckerMVC.Repositories;

namespace MakerCheckerMVC.Controllers;

[Authorize(Roles = "Checker")]
public class CheckerController : Controller
{
    private readonly FileRepository _repo;

    public CheckerController(FileRepository repo)
    {
        _repo = repo;
    }

    public IActionResult Index()
    {
        var files = _repo.GetPendingFiles();
        return View(files);
    }

    public IActionResult Approve(int id)
    {
        _repo.UpdateStatus(id, "Approved");
        return RedirectToAction("Index");
    }

    public IActionResult Reject(int id)
    {
        _repo.UpdateStatus(id, "Rejected");
        return RedirectToAction("Index");
    }
}