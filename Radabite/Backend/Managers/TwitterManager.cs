using Radabite.Backend.Interfaces;
using Radabite.Client.WebClient.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Tweetinvi;

namespace Radabite.Backend.Managers
{
    public class TwitterManager : ITwitterManager
    {
        const string badUserToken = "No Twitter access token found for user.";

        public GetPostsResult GetTweets(Radabite.Backend.Database.User user, DateTime startTime, DateTime endTime)
        {
            if (user.TwitterToken != null)
            {
                
                TwitterCredentials.SetCredentials(ConfigurationManager.AppSettings["twitterAccessToken"], ConfigurationManager.AppSettings["twitterAccessTokenSecret"], ConfigurationManager.AppSettings["twitterConsumerKey"], ConfigurationManager.AppSettings["twitterSecretKey"]);

                var searchParameter = Search.GenerateSearchTweetParameter("from:ReesK5150");
                var tweets = Search.SearchTweets(searchParameter);
                IList<TweetModel> tweetList = new List<TweetModel>();

                foreach (var tweet in tweets)
                {
                    TweetModel tweetModel = new TweetModel();
                    tweetModel.fromName = tweet.Source;
                    tweetModel.message = tweet.Text;
                    tweetModel.retweetsCount = tweet.RetweetCount;
                    tweetModel.favoritesCount = tweet.FavouriteCount;
                    tweetModel.created_time = tweet.CreatedAt;
                    tweetList.Add(tweetModel); // w/ properties we want.
                }

                var result = new GetPostsResult();
                result.posts = tweetList;
                return result;
            }
            else
            {
                return new GetPostsResult { hasErrors = true, errorMessage = badUserToken };
            }

        }
    }
}