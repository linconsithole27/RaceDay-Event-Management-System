

using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace RaceDay.Mvc.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly JsonSerializerOptions _jsonOptions =
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

        public ApiService(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpRequestMessage CreateRequest(
            HttpMethod method, string endpoint)
        {
            var request = new HttpRequestMessage(
                method, endpoint);

            var cookie = _httpContextAccessor.HttpContext?
                .Session.GetString("ApiAuthCookie");

            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }

            return request;
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            using var request = CreateRequest(
                HttpMethod.Get, endpoint);

            using var response = await _httpClient.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(json);

            return JsonSerializer.Deserialize<T>(
                json, _jsonOptions);
        }

        public async Task<bool> PostAsync<TRequest>(
            string endpoint, TRequest data)
        {
            var json = JsonSerializer.Serialize(data);

            using var request = CreateRequest(
                HttpMethod.Post, endpoint);

            request.Content = new StringContent(
                json, Encoding.UTF8, "application/json");

            using var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(
            string endpoint, TRequest data)
        {
            var json = JsonSerializer.Serialize(data);

            using var request = CreateRequest(
                HttpMethod.Post, endpoint);

            request.Content = new StringContent(
                json, Encoding.UTF8, "application/json");

            using var response = await _httpClient.SendAsync(request);

            var responseJson =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(responseJson);

            if (string.IsNullOrWhiteSpace(responseJson))
                return default;

            return JsonSerializer.Deserialize<TResponse>(
                responseJson, _jsonOptions);
        }

        public async Task<bool> LoginAsync<TRequest>(
            string endpoint, TRequest data)
        {
            var json = JsonSerializer.Serialize(data);

            using var request = new HttpRequestMessage(
                HttpMethod.Post, endpoint);

            request.Content = new StringContent(
                json, Encoding.UTF8, "application/json");

            using var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return false;

            if (response.Headers.TryGetValues(
                "Set-Cookie", out var cookies))
            {
                var authCookie = cookies.FirstOrDefault(
                    c => c.StartsWith("RaceDay.Auth="));

                if (authCookie != null)
                {
                    var cookieValue = authCookie.Split(';')[0];

                    _httpContextAccessor.HttpContext?
                        .Session.SetString(
                            "ApiAuthCookie", cookieValue);

                    return true;
                }
            }

            return false;
        }

        public async Task<bool> PutAsync<TRequest>(
        string endpoint, TRequest data)
        {
            var json = JsonSerializer.Serialize(data);

            using var request = CreateRequest(HttpMethod.Put, endpoint);

            request.Content = new StringContent(
                json, Encoding.UTF8, "application/json");

            using var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            using var request = CreateRequest(HttpMethod.Delete, endpoint);

            using var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }

}

