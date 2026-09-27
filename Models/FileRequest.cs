namespace MakerCheckerMVC.Models;
public class FileRequest
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string FileType { get; set; }
    public byte[] FileData { get; set; }
    public string Status { get; set; }
    public int MakerId { get; set; }
}