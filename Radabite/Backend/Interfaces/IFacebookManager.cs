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
        FacebookGetPostsResult GetPosts(User user, DateTime startTime, DateTime endTime);
        FacebookGetPostsResult GetPhotos(User user, DateTime startTime, DateTime endTime);
        string GetAccessToken(FacebookClient fb);
        FacebookPublishResult PublishStatus(User user, string message);
        string GetFacebookLongTermAccessCode(string shortTermAccessToken);
        string GetProfilePictureUrl(User user);


        double ConvertToUnixTimestamp(DateTime date);
        DateTime ConvertFromUnixTimestamp(double timestamp);


    }
}
