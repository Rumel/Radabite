using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radabite.Backend.Interfaces
{
    interface IFacebookManager
    {
        void GetPosts(string userId, string accessToken, DateTime startTime, DateTime endTime);
        string GetAccessToken(FacebookClient fb);


        double ConvertToFacebookTime(DateTime startTime);
        DateTime ConvertFromUnixTime(double startTime);

    }
}
