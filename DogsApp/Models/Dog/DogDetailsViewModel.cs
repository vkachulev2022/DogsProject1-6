namespace DogsApp.Models.Dog
{
    public class DogDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public string BreedName { get; set; }

        public string? Picture { get; set; }
    }
}
