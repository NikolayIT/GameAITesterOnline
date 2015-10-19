namespace OnlineGames.Services.AiPortal
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using OnlineGames.Data.Models;
    using OnlineGames.Services.AiPortal.Models;

    public interface IRemoteDataService
    {
        Task<User> Login(string username, string password);
    }

    public class RemoteDataService : IRemoteDataService
    {
        // TODO: Pass as methods (or constructor) parameters
        private const string ApiKey = "3d33a038e0dbcaa7121c4f133dc474d7";

        private const string BaseAddress = "https://telerikacademy.com";

        private const string ApiCheckUserLoginUrlFormat = "/Api/Users/CheckUserLogin?apiKey={0}&usernameoremail={1}&password={2}";

        private readonly HttpClient client;

        public RemoteDataService()
        {
            this.client = new HttpClient { BaseAddress = new Uri(BaseAddress) };
            this.client.DefaultRequestHeaders.Add("Connection", "close");
        }

        public async Task<User> Login(string username, string password)
        {
            var url = string.Format(ApiCheckUserLoginUrlFormat, ApiKey, username, password);
            var remoteUser = await this.RemoteGet<CheckUserLoginApiModel>(url);
            if (remoteUser.IsValid)
            {
                var user = new User { UserName = remoteUser.UserName, AvatarUrl = remoteUser.SmallAvatarUrl };
                if (remoteUser.IsAdmin)
                {
                    // TODO: Add to admin role
                }

                return user;
            }
            else
            {
                return null;
            }
        }

        private async Task<T> RemoteGet<T>(string url)
        {
            var response = await this.client.GetAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<T>(jsonString);
            return model;
        }
    }
}
