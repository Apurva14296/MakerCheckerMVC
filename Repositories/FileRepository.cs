using MakerCheckerMVC.Models;
using Microsoft.Data.SqlClient;

namespace MakerCheckerMVC.Repositories;

public class FileRepository
{
    private readonly IConfiguration _config;

    public FileRepository(IConfiguration config)
    {
        _config = config;
    }

    public void Save(FileRequest file)
    {
        using SqlConnection con = new SqlConnection(
            _config.GetConnectionString("DefaultConnection"));

        string query = @"INSERT INTO FileRequests
        (FileName, FileType, FileData, Status, MakerId)
        VALUES
        (@FileName, @FileType, @FileData, @Status, @MakerId)";

        SqlCommand cmd = new SqlCommand(query, con);

        cmd.Parameters.AddWithValue("@FileName", file.FileName);
        cmd.Parameters.AddWithValue("@FileType", file.FileType);
        cmd.Parameters.AddWithValue("@FileData", file.FileData);
        cmd.Parameters.AddWithValue("@Status", "Pending");
        cmd.Parameters.AddWithValue("@MakerId", file.MakerId);

        con.Open();
        cmd.ExecuteNonQuery();
    }

    public List<FileRequest> GetPendingFiles()
{
    List<FileRequest> list = new();

    using SqlConnection con = new SqlConnection(
        _config.GetConnectionString("DefaultConnection"));

    string query = "SELECT * FROM FileRequests WHERE Status='Pending'";

    SqlCommand cmd = new SqlCommand(query, con);
    con.Open();

    SqlDataReader dr = cmd.ExecuteReader();

    while (dr.Read())
    {
        list.Add(new FileRequest
        {
            Id = Convert.ToInt32(dr["Id"]),
            FileName = dr["FileName"].ToString(),
            FileType = dr["FileType"].ToString(),
            Status = dr["Status"].ToString(),
            MakerId = Convert.ToInt32(dr["MakerId"])
        });
    }

    return list;
}

public void UpdateStatus(int id, string status)
{
    using SqlConnection con = new SqlConnection(
        _config.GetConnectionString("DefaultConnection"));

    string query = "UPDATE FileRequests SET Status=@Status WHERE Id=@Id";

    SqlCommand cmd = new SqlCommand(query, con);

    cmd.Parameters.AddWithValue("@Id", id);
    cmd.Parameters.AddWithValue("@Status", status);

    con.Open();
    cmd.ExecuteNonQuery();
}
}