using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace ConsoleApplication
{
    public class Program
    {
        private static readonly HttpClient client;

        private static readonly HttpClientHandler handler = new HttpClientHandler();

        private static readonly Random ran = new Random();

        private static readonly CookieContainer cookies = new CookieContainer();

        private static readonly Uri uri = new Uri("http://localhost:5000/");

        static Program()
        {
            client = new HttpClient(handler);
            handler.CookieContainer = cookies;
        }

        public static void Main(string[] args)
        {
            //CreateAccounts().Wait();
            PostLinks().Wait();
        }

        public static async Task CreateAccounts()
        {
            string username = "iiii";
            string password = "fopafwnoa";
            for(int i = 0; i < 9000; i++)
            {
                var values = new Dictionary<string, string>
                {
                    { "UserName", username + i },
                    { "Password", password + i },
                    { "ConfirmPassword", password + i }
                };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("http://localhost:5000/Account/Register", content);

                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(i);
            }
        }

        public static async Task<List<News>> GetNews(string source)
        {
            var response = await client.GetAsync(
                "https://newsapi.org/v1/articles?source=" +
                source +
                "&sortBy=top&apiKey=235eeba5730b435a8d8e4740495ffbfe");
            return JsonConvert
                    .DeserializeObject<NewsResponse>(await response.Content.ReadAsStringAsync())
                    .Articles;
        }

        public static async Task PostLinks()
        {
            var newsList = await GetNews("google-news");
            newsList.AddRange(await GetNews("abc-news-au"));
            newsList.AddRange(await GetNews("bbc-sport"));

            var subs = new []{ "news", "google-news", "abc-news-au", "bbc-sport"};
            
            foreach (var news in newsList)
            {
                await Login(ran.Next(9000));

                var link = news.Url.Contains("https") ? news.Url.Remove(0, 8) : news.Url.Remove(0, 7);

                var values = new Dictionary<string, string>
                        {
                            { "Title", news.Title },
                            { "Link", link },
                            { "UrlToImage", news.UrlToImage },
                            { "Subreddit",  subs[ran.Next(4)]}
                        };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("http://localhost:5000/api/post", content);

                string location = null;
                IEnumerable<string> values1;
                if (response.Headers.TryGetValues("Location", out values1))
                    location = values1.First();
            
                await CommentPost(int.Parse(location.Split('/')[5]));
                
                await UpvoteLink(int.Parse(location.Split('/')[5]), ran.Next(100));
            }

            // Post hard coded content
             
            await Login(ran.Next(9000));

            var _values = new Dictionary<string, string>
                    {
                        { "Title", "Check out the source" },
                        { "Link", "github.com/marvinkopf/Reddit" },
                        { "UrlToImage", "https://avatars3.githubusercontent.com/u/12144728?v=3&s=460" },
                        { "Subreddit",  "reddit"}
                    };

            var _content = new FormUrlEncodedContent(_values);

            var _response = await client.PostAsync("http://localhost:5000/api/post", _content);
        }

        public static async Task CommentPost(int id)
        {
            foreach(int i in Enumerable.Range(0, 9000).OrderByDescending(i => ran.Next()).Take(10))
            {
                await Login(i);

                var values = new Dictionary<string, string>
                    {
                        { "Txt", RandomString(ran.Next(200) + 5) },
                        { "PostId", id.ToString() }
                    };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("http://localhost:5000/api/comment", content);

                string location = "";
                IEnumerable<string> values1;
                if (response.Headers.TryGetValues("Location", out values1))
                    location = values1.First();

                await UpvoteComment(location.Split('/')[5]);
            
                await CommentComment(location.Split('/')[5], id.ToString());
            }
        }

        public static async Task CommentComment(string parentId, string postId, int depth = 0)
        {
            while (ran.Next(2) != 0 && depth < 3)
            {
                await Login(ran.Next(9000));

                var values = new Dictionary<string, string>
                    {
                        { "Txt", RandomString(ran.Next(200) + 5) },
                        { "PostId", postId },
                        { "ParentId", parentId }
                    };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("http://localhost:5000/api/comment", content);

                string location = null;
                IEnumerable<string> values1;
                if (response.Headers.TryGetValues("Location", out values1))
                    location = values1.First();

                await UpvoteComment(location.Split('/')[5]);

                await CommentComment(location.Split('/')[5], postId, depth + 1);
            }
        }

        public static async Task UpvoteComment(string commentId)
        {
            foreach (var i in Enumerable.Range(0, 9000).OrderByDescending(i => ran.Next()).Take(ran.Next(10)))
            {
                await Login(i);

                var values = new Dictionary<string, string>();

                var content = new FormUrlEncodedContent(values);

                await client.PostAsync(uri + "api/comment/" + commentId + "/upvote", content);
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 \n.!?-+,;";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[ran.Next(s.Length)]).ToArray());
        }

        public static async Task UpvoteLink(int postId, int count)
        {
            foreach(var i in Enumerable.Range(0, 9000).OrderByDescending(i => ran.Next()).Take(count))
            {
                await Login(i);

                var values = new Dictionary<string, string>();

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync(uri + "api/post/" + postId + "/upvote", content);
            }
        }

        public static async Task Login(int id)
        {
            var loginv = new Dictionary<string, string>
                    {
                        { "UserName", "iiii" + id },
                        { "Password", "fopafwnoa" + id }
                    };

            var content = new FormUrlEncodedContent(loginv);

            var response = await client.PostAsync("http://localhost:5000/LoginNoRedirect", content);
        }
    }
}
