using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperKittens.Models;

namespace SuperKittens.Repository
{
    public interface ISuperKittenRepository
    {
        ICollection<SuperKitten> GetAll();
        SuperKitten GetById(int id);
        SuperKitten Create(SuperKitten superKitten);
        SuperKitten Update(SuperKitten superKitten);
        bool Delete(int id);
    }
    internal class SuperKittensRepository : ISuperKittenRepository
    {
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
        public ICollection<SuperKitten> GetAll()
        {
            return _data;
        }

        public SuperKitten GetById(int id)
        {
            return _data.FirstOrDefault(r => r.Id == id);
        }

        public SuperKitten Create(SuperKitten superKitten)
        {
            superKitten.Id = _data.Max(s => s.Id) + 1;
            _data.Add(superKitten);
            return superKitten;
        }

        public SuperKitten Update(SuperKitten superKitten)
        {
            var temp = _data.FirstOrDefault(r => r.Id == superKitten.Id);
            _data.Remove(temp);
            _data.Add(superKitten);
            return superKitten;
        }

        public bool Delete(int id)
        {
            _data.Remove(_data.FirstOrDefault(r => r.Id == id));
            return true;
        }
    }
}
