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

        public async Task<SuperKitten> Create(SuperKitten superKitten, byte[] picture)
        {
            var res = await KittenRepository.Create(superKitten);
            await KittenRepository.AddPicture(res.Id, picture);
            return res;
        }

        public async Task<SuperKitten> Update(SuperKitten superKitten, byte[] picture)
        {
            var res = await KittenRepository.Update(superKitten);
            await KittenRepository.AddPicture(superKitten.Id, picture);
            return res;
        }

        public async Task<bool> Delete(int id)
        {
            return await KittenRepository.Delete(id);
        }
    }
}
