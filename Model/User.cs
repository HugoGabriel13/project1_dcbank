using System.ComponentModel.DataAnnotations;

namespace TestApi.Models
{
    public class User
    {
        [Key]
        public string Login { get; set; }
        public string Password { get; set; }

        public User(string login, string password)
        {
            Login = login ?? throw new ArgumentNullException(nameof(login));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }
    }
}
