namespace Project_C.Models
{
    public class Image
    {
        public Guid Id { get; set; }

        public string ImageTitle { get; set; }
        public byte[] ImageData { get; set; }

        //every image is related to a product or a store.
        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }

        public Guid? StoreId { get; set; }
        public Store? Store { get; set; }

        public string Type { get; set; }
    }
}
