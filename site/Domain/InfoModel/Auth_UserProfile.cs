using System;
using System.Collections.Generic;

namespace Domain.InfoModel
{
    public class Auth_UserProfile
    {
        public int Id { get; set; }

        public String Username { get; set; }

        public String RealName { get; set; }

        public String Email { get; set; }

        public Boolean IsEnabled { get; set; }

        public Boolean IsSuperUser { get; set; }

        public IList<int> SelectGroupIds { get; set; }

        public Auth_UserProfile()
        {
            SelectGroupIds=new List<int>();
        }
    }
}
