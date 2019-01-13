using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ChickenAPI.Data;
using Newtonsoft.Json;

namespace SaltyEmu.Communication.HTTP
{
    internal class HttpRepository<TDto, TKey> : IAsyncRepository<TDto, TKey> where TDto : class
    {
        private readonly HttpClient _client;
        private readonly string _prefix;
        private readonly string _token;

        public HttpRepository()
        {
            _client = new HttpClient { BaseAddress = new Uri($"") };
            _prefix = "client";
        }

        public async Task<IEnumerable<TDto>> GetAsync()
        {
            HttpResponseMessage response = await _client.GetAsync(new Uri(_prefix + "/"));

            if (!response.IsSuccessStatusCode)
            {
                // todo proper handling
                return null;
            }

            IEnumerable<TDto> list = JsonConvert.DeserializeObject<IEnumerable<TDto>>(await response.Content.ReadAsStringAsync());
            return list;
        }

        public async Task<TDto> GetByIdAsync(TKey id)
        {
            HttpResponseMessage response = await _client.GetAsync(new Uri(_prefix + "/?id=" + id));

            if (!response.IsSuccessStatusCode)
            {
                // todo proper handlings
                return null;
            }

            return JsonConvert.DeserializeObject<TDto>(await response.Content.ReadAsStringAsync());
        }

        public Task<IEnumerable<TDto>> GetByIdsAsync(IEnumerable<TKey> ids) => throw new NotImplementedException();

        public async Task<TDto> SaveAsync(TDto obj)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");
            HttpResponseMessage postResponse = _client.PostAsync("/", content).Result;
            if (postResponse.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TDto>(await postResponse.Content.ReadAsStringAsync());
            }

            throw new HttpRequestException(postResponse.Headers.ToString());
        }

        public Task SaveAsync(IEnumerable<TDto> objs)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var content = new StringContent(JsonConvert.SerializeObject(objs), Encoding.Default, "application/json");
            HttpResponseMessage postResponse = _client.PostAsync("/", content).Result;
            if (postResponse.IsSuccessStatusCode)
            {
                return Task.CompletedTask;
            }

            throw new HttpRequestException(postResponse.Headers.ToString());
        }

        public Task DeleteByIdAsync(TKey id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            var content = new StringContent(JsonConvert.SerializeObject(id), Encoding.Default, "application/json");
            HttpResponseMessage postResponse = _client.DeleteAsync("/?id=" + id).Result;
            if (postResponse.IsSuccessStatusCode)
            {
                return Task.CompletedTask;
            }

            throw new HttpRequestException(postResponse.Headers.ToString());
        }

        public Task DeleteByIdsAsync(IEnumerable<TKey> ids) => throw new NotImplementedException();
    }
}