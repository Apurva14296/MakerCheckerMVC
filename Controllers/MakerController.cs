using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MakerCheckerMVC.Models;
using MakerCheckerMVC.Repositories;


[Authorize(Roles = "Maker")]
public class MakerController : Controller
{
    private readonly FileRepository _repo;

    public MakerController(FileRepository repo)
    {
        _repo = repo;
    }

    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
public async Task<IActionResult> Upload(IFormFile file)
{
    if (file == null || file.Length == 0)
        return View();

    byte[] data;

    using (var ms = new MemoryStream())
    {
        await file.CopyToAsync(ms);
        data = ms.ToArray();
    }

    FileRequest request = new FileRequest
    {
        FileName = file.FileName,
        FileType = file.ContentType,
        FileData = data,
        MakerId = 1
    };

    _repo.Save(request);

    ViewBag.Message = "File saved in database successfully.";
    return View();
}
}