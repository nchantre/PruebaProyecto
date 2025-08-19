namespace RealEstate.Application.Owers.DTOs.Filter
{
    public class PropertySearchParamsDto
    {

        public string? Name { get; set; }
        public string? Address { get; set; }
        public decimal? Price { get; set; }          // Igual a
        public string? CodeInternal { get; set; }
        public int? Year { get; set; }

        // (Opcional) Paginación y ordenamiento a nivel Owner
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public bool OrderByNameDesc { get; set; } = false;
    }
}
