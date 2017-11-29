using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

        private readonly HttpClient _client = new HttpClient { Timeout = new TimeSpan(0, 0, 2) };
        private readonly List<SuperKitten> _data = new List<SuperKitten>
        {
            new SuperKitten
            {
                Id = 1,
                LastName = "Test",
                Name = "Test",
                PictureUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/0/06/Kitten_in_Rizal_Park%2C_Manila.jpg/1200px-Kitten_in_Rizal_Park%2C_Manila.jpg"
            },
            new SuperKitten
            {
                Id = 1,
                LastName = "Test 2",
                Name = "Test 2",
                PictureUrl = "http://www.petwave.com/~/media/Images/Center/Care-and-Nutrition/Cat/Kittensv2/Kitten-3.ashx"
            }
        };
        public async Task<ICollection<SuperKitten>> GetAll()
        {
            var responce = await Task.Run(() => _client.GetAsync(ApiRoot));
            if (responce.IsSuccessStatusCode)
            {
                var content = await responce.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ApiSuperKitten>>(content).Select(s => s.ToSuperKitten()).ToList();
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

        public SuperKitten Create(SuperKitten superKitten)
        {
            superKitten.Id = _data.Max(s => s.Id) + 1;
            _data.Add(superKitten);
            return superKitten;
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

        public SuperKitten AddPicture(int id, byte[] picture)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(int id)
        {
            _data.Remove(_data.FirstOrDefault(r => r.Id == id));
            return true;
        }
    }
}
