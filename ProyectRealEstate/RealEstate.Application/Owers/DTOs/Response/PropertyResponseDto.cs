using RealEstate.Application.Owers.DTOs.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Owers.DTOs.Response
{
    public class PropertyResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string CodeInternal { get; set; }
        public int Year { get; set; }

        public OwnerDto Owner { get; set; }
        public List<ImageDto> Images { get; set; }
        public List<TraceDto> Traces { get; set; }
    }

    public class OwnerDto
    {
        public string IdOwner { get; set; }
        public string Name { get; set; }
        // Otros campos necesarios
    }

    public class ImageDto
    {
        public string File { get; set; }
        public bool Enabled { get; set; }
    }

    public class TraceDto
    {
        public DateTime DateSale { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
    }
}
