using Microsoft.Data.SqlClient;
using MakerCheckerMVC.Models;

namespace MakerCheckerMVC.Repositories;

public class UserRepository
{
    private readonly IConfiguration _config;

    public UserRepository(IConfiguration config)
    {
        _config = config;
    }

    public User? ValidateUser(string username, string password)
    {
        using SqlConnection con = new(
            _config.GetConnectionString("DefaultConnection"));

        string sql = @"SELECT * FROM Users
                       WHERE UserName=@u AND Password=@p";

        SqlCommand cmd = new(sql, con);
        cmd.Parameters.AddWithValue("@u", username);
        cmd.Parameters.AddWithValue("@p", password);

        con.Open();

        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            return new User
            {
                Id = (int)dr["Id"],
                UserName = dr["UserName"].ToString()!,
                Role = dr["Role"].ToString()!
            };
        }

        return null;
    }
}