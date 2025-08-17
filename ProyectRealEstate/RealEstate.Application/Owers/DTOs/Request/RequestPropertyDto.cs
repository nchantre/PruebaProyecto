using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RealEstate.Application.Owers.DTOs.Request
{
  


    public class RequestPropertyDto
    {
    
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public decimal Price { get; set; }
        public string CodeInternal { get; set; } = default!;
        public int Year { get; set; }
        public List<PropertyImageDto> Images { get; set; } = new();
        public List<PropertyTraceDto> Traces { get; set; } = new();
    }
}
