using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperKittens.Models;
using SuperKittens.Repository;

namespace SuperKittens.Service
{
    public class SuperKittensService
    {
        private static readonly SuperKittensRepository KittenRepository = new SuperKittensRepository();


        public async Task<List<SuperKitten>> GetAll()
        {
            var res = await KittenRepository.GetAll();
            return res.OrderBy(o => o.Name).ToList();
        }

        public async Task<SuperKitten> GetById(int id)
        {
            return await KittenRepository.GetById(id);
        }

        public SuperKitten Create(SuperKitten superKitten)
        {
            return KittenRepository.Create(superKitten);
        }

        public async Task<SuperKitten> Update(SuperKitten superKitten)
        {
            return await KittenRepository.Update(superKitten);
        }

        public bool Delete(int id)
        {
            return KittenRepository.Delete(id);
        }
    }
}
