using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        int val = 0;
        SqlConnection cn = new SqlConnection("Data Source=mazenet-test;Initial Catalog=Northwind;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
        SqlCommand cmd = new SqlCommand("SELECT [dbo].[ValidateUser] (@username,@pwd)", cn);
        cmd.Parameters.AddWithValue("@username", model.Username);
        cmd.Parameters.AddWithValue("@pwd", model.Password);
        cn.Open();
        SqlDataReader dr=cmd.ExecuteReader();
        if (dr.HasRows) {
            dr.Read();
            val = Convert.ToInt32(dr[0]);
          
        
        }
        cn.Close();
        cn.Dispose();


        if (val == 1)
        {
            
                      var claims = new[]
           {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            //cryptograhic

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            //Printing token value
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });


        }
        else
        {
            return Unauthorized();
        }


        //if (model.Username == "admin" && model.Password == "123")
        //{
        //Generate token
        //var claims = new[]
        //{
        //    new Claim(ClaimTypes.Name, model.Username),
        //    new Claim(ClaimTypes.Role, "Admin")
        //};

        ////cryptograhic

        //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //var token = new JwtSecurityToken(
        //    issuer: _config["Jwt:Issuer"],
        //    audience: _config["Jwt:Audience"],
        //    claims: claims,
        //    expires: DateTime.Now.AddMinutes(60),
        //    signingCredentials: creds);

        ////Printing token value
        //return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        //}


    }
}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}
