using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Dto
{
    public class FileDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public long Size { get; set; }

        public DateTime InsertedDate { get; set; }
    }

    public class RequestAddFileDto
    {
        public IEnumerable<IFormFile> Files { get; set; }

        public long EntityId { get; set; }

        public string EntityName { get; set; }

        public long UserId { get; set; }
    }

    public class ResponseGetFileDto
    {
        public string Name { get; set; }

        public byte[] Source { get; set; }
    }
}
