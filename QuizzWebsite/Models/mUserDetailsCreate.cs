using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizzWebsite.Models
{
    public class mUserDetailsCreate
    {
        public string connection { get; set; }
        public string email { get; set; }
        public bool email_verified { get; set; }
        public string family_name { get; set; }
        public string given_name { get; set; }
        public string name { get; set; }
        public string nickname { get; set; }
        public string picture { get; set; }
        public string password { get; set; }
        public string user_id { get; set; }
        public string username { get; set; }
        public AppMetadata app_metadata { get; set; }
    }
}
