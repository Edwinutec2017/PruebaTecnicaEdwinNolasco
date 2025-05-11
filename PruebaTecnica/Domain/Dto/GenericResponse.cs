using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class GenericResponse<T>
    {
        public required ResponseStatus Status { get; set; }
        public required T Item { get; set; }
    }
    public class ResponseStatus
    {
        public HttpStatusCode HttpCode { get; set; }
        public required string Message { get; set; }
    }
}
    

