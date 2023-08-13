using System;
using CoroDr.IdentityAPI.Models;

namespace CoroDr.IdentityAPI.Repository
{
    public class UserRepository : IUserRepositoryInterface
    {

        private IdentityDbContext _identityDbContext;
        public UserRepository(IdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }
        public User FindUserByUserName(string username)
        {
            var user = _identityDbContext.Users.Where(x => x.Username == username).SingleOrDefault();

            return user ?? new User();
        }

        public bool SaveChanges(User user)
        {
            _identityDbContext.Users.Add(user);
            int statusOfChange = _identityDbContext.SaveChanges();

            return statusOfChange == 1;
        }
    }
}

