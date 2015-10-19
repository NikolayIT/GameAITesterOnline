namespace OnlineGames.Data.Models
{
    using System;
    using System.Collections.Generic;

    using OnlineGames.Data.Common.Models;

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : BaseModel<int>
    {
        public User()
        {
            this.Salt = Guid.NewGuid().ToString();
            this.Roles = new HashSet<Role>();
        }

        public string UserName { get; set; }

        public string AvatarUrl { get; set; }

        public string Salt { get; set; }
        
        public string PasswordHash { get; set; }

        public UserProvider Provider { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
