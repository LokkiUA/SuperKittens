using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SuperKittens.Models;

namespace SuperKittens.Repository
{
    internal class SuperKittensRepository
    {
        private class Picture
        {
            public int Id { get; set; }
            public string Url { get; set; }
        }

        private class ApiSuperKitten
        {
            public ApiSuperKitten()
            {

            }

            public ApiSuperKitten(SuperKitten superKitten)
            {
                Id = superKitten.Id;
                Name = superKitten.Name;
                LastName = superKitten.LastName;
                Picture = null;
            }
            public int Id { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public Picture Picture { get; set; }

            public SuperKitten ToSuperKitten()
            {
                return new SuperKitten
                {
                    Id = Id,
                    LastName = LastName,
                    Name = Name,
                    PictureUrl = Picture?.Url
                };
            }
        }
        private const string ApiRoot = "http://soprasteriasuperkittensapi.azurewebsites.net/api/SuperKittens";

        private readonly HttpClient _client = new HttpClient { Timeout = new TimeSpan(0, 0, 10) };

        public async Task<ICollection<SuperKitten>> GetAll()
        {
            try
            {
                var responce = await Task.Run(() => _client.GetAsync(ApiRoot));
                if (responce.IsSuccessStatusCode)
                {

                    var content = await responce.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ApiSuperKitten>>(content).Select(s => s.ToSuperKitten()).ToList();

                }
            }
            catch (Exception ex)
            {
                // who cares?
            }
            return new List<SuperKitten>();
        }

        public async Task<SuperKitten> GetById(int id)
        {
            var responce = await Task.Run(() => _client.GetAsync($"{ApiRoot}/{id}"));
            if (responce.IsSuccessStatusCode)
            {
                var content = await responce.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ApiSuperKitten>(content).ToSuperKitten();
            }
            return new SuperKitten();
        }

        public async Task<SuperKitten> Create(SuperKitten superKitten)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new ApiSuperKitten(superKitten)), Encoding.UTF8, "application/json");
            var responce = await _client.PostAsync($"{ApiRoot}", content);
            if (responce.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuperKitten>(await responce.Content.ReadAsStringAsync()).ToSuperKitten();
            }
            return null;
        }

        public async Task<SuperKitten> Update(SuperKitten superKitten)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new ApiSuperKitten(superKitten)), Encoding.UTF8, "application/json");
            var responce = await _client.PutAsync($"{ApiRoot}/{superKitten.Id}", content);
            if (!responce.IsSuccessStatusCode)
            {
                return null;
            }
            return superKitten;
        }

        public async Task<SuperKitten> AddPicture(int id, byte[] picture)
        {
            var form = new MultipartFormDataContent();

            var imageContent = new ByteArrayContent(picture);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            form.Add(imageContent, "file", Guid.NewGuid().ToString());
            var unused = await Task.Run(() => _client.PutAsync($"{ApiRoot}/{id}/picture", form));
            return await GetById(id);
        }

        public async Task<bool> Delete(int id)
        {
            var responce = await _client.DeleteAsync($"{ApiRoot}/{id}");
            return responce.IsSuccessStatusCode;
        }
    }
}
