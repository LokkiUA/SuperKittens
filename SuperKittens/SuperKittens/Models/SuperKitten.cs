namespace SuperKittens.Models
{
    public class SuperKitten
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        private string _pictureUrl;
        public string PictureUrl
        {
            get => string.IsNullOrEmpty(_pictureUrl) ? "https://upload.wikimedia.org/wikipedia/commons/thumb/0/06/Kitten_in_Rizal_Park%2C_Manila.jpg/1200px-Kitten_in_Rizal_Park%2C_Manila.jpg" : _pictureUrl;
            set => _pictureUrl = value;
        }
    }
}
