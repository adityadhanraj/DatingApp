using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            this._context = context;
        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if(user == null){
                return null;
            }
            if(!VerifyPassword(password,user.PasswordHash,user.PasswordSalt))
            {
                return null;
            }
            return user;
        }

        private bool VerifyPassword(string password, byte[] passwordhash, byte[] passwordsalt)
        {
            using(var hash = new System.Security.Cryptography.HMACSHA512(passwordsalt))
            {
                var computedHash = hash.ComputeHash(passwordsalt);
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if(computedHash[i] != passwordhash[i]) return false;   
                }
                return true;
            }

        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordhash,passwordsalt;
            CreatePassword(password,out passwordhash,out passwordsalt);
            user.PasswordHash = passwordhash;
            user.PasswordSalt = passwordsalt;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;

        }

        private void CreatePassword(string password, out byte[] passwordhash, out byte[] passwordsalt)
        {
            using(var hash = new System.Security.Cryptography.HMACSHA512())
            {
                passwordsalt = hash.Key;
                passwordhash = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x => x.UserName == username)) return true;
            return false;
        }
    }
}