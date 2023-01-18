namespace Project_C.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Place { get; set; }
        public string PlaceAsString { get; set; }
        public string VideoLink { get; set; }

        //Every product has a list of Stores it can be bought at, it can be empty. (Many to many)
        // https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration
        public ICollection<Store>? Stores { get; set; }

        //every product has one image
        public byte[] ProductImage { get; set; }
    }
}
