using Facebook;
using Newtonsoft.Json.Linq;
using Radabite.Backend.Database;
using Radabite.Client.WebClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radabite.Backend.Interfaces
{
    interface IFacebookManager
    {
        //IList<FacebookPostModel> GetPosts(string userId, string accessToken, DateTime startTime, DateTime endTime);
        IList<FacebookPostModel> GetPosts(string userAccessToken, DateTime startTime, DateTime endTime);
        string GetAccessToken(FacebookClient fb);
        FacebookPublishResult PublishStatus(User user, string message);
        string GetFacebookLongTermAccessCode(string shortTermAccessToken);

        double ConvertToUnixTimestamp(DateTime date);
        DateTime ConvertFromUnixTimestamp(double timestamp);


    }
}
