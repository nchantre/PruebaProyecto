namespace RealEstate.Domain.Entities.Filter
{
    public class PropertySearchParams
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public decimal? Price { get; set; }
        public string? CodeInternal { get; set; }
        public int? Year { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public bool OrderByNameDesc { get; set; } = false;
    }
}
