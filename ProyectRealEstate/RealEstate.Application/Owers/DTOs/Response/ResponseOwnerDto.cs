using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Owers.DTOs.Response
{
    public class ResponseOwnerDto
    {
        public string? IdOwner { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Photo { get; set; } = default!;
        public DateTime Birthday { get; set; }
        public List<ResponsePropertyDto> Properties { get; set; } = new();
    }
}
