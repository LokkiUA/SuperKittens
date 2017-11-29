using System.Collections.Generic;
using System.Linq;
using SuperKittens.Models;
using SuperKittens.Repository;

namespace SuperKittens.Service
{
    public class SuperKittensService : ISuperKittenService
    {
        private static readonly SuperKittensRepository KittenRepository = new SuperKittensRepository();


        public ICollection<SuperKitten> GetAll()
        {
            return KittenRepository.GetAll().OrderBy(o => o.Name).ToList();
        }

        public SuperKitten GetById(int id)
        {
            return KittenRepository.GetById(id);
        }

        public SuperKitten Create(SuperKitten superKitten)
        {
            return KittenRepository.Create(superKitten);
        }

        public SuperKitten Update(SuperKitten superKitten)
        {
            return KittenRepository.Update(superKitten);
        }

        public bool Delete(int id)
        {
            return KittenRepository.Delete(id);
        }
    }
}
