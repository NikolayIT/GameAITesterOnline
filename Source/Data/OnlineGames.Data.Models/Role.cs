namespace OnlineGames.Data.Models
{
    using System.Collections.Generic;

    using OnlineGames.Data.Common.Models;

    public class Role : BaseModel<int>
    {
        public Role()
        {
            this.Users = new HashSet<User>();
        }

        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
