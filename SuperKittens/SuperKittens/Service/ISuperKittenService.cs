using System.Collections.Generic;
using SuperKittens.Models;

namespace SuperKittens.Service
{
    public interface ISuperKittenService
    {
        ICollection<SuperKitten> GetAll();
        SuperKitten GetById(int id);
        SuperKitten Create(SuperKitten superKitten);
        SuperKitten Update(SuperKitten superKitten);
        bool Delete(int id);
    }
}