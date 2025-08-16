namespace RealEstate.Application.Owers.DTOs
{
    public class OwnertDto
    {
        public string? IdOwner { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Photo { get; set; } = default!;
        public DateTime Birthday { get; set; }
        public List<PropertyDto> Properties { get; set; } = new();
    }
}



