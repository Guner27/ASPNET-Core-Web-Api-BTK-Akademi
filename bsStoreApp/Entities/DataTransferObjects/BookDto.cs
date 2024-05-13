namespace Entities.DataTransferObjects
{
    public record BookDto
    {
        public int Id { get; init; } //init anahtar sözcüğü Readonly  özellik kazandırır.
        public String Title { get; init; }
        public decimal Price { get; init; }
    }
}
