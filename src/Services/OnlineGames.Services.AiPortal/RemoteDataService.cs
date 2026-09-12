// <copyright file="RemoteDataService.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Services.AiPortal
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using OnlineGames.Services.AiPortal.Models;

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

        public async Task<CheckUserLoginApiModel> Login(string username, string password)
        {
            var url = string.Format(ApiCheckUserLoginUrlFormat, ApiKey, username, password);
            var remoteUser = await this.RemoteGet<CheckUserLoginApiModel>(url);
            return remoteUser;
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
