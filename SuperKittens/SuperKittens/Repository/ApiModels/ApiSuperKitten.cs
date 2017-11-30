using SuperKittens.Models;
// ReSharper disable UnusedMember.Local
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace SuperKittens.Repository.ApiModels
{
    internal class ApiSuperKitten
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

}
