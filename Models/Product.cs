namespace RedisExampleApp.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; } //burada '?' işareti koymamızın amacı nullable olabilir diye ekliyoruz.
        public decimal Price { get; set; }

    }
}
