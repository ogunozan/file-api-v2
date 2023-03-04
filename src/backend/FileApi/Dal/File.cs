using System;

namespace Dal
{
    public sealed class File : EntityBase, IModifiedEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Extension { get; set; }

        public string Path { get; set; }

        public long EntityId { get; set; }

        public string EntityName { get; set; }

        public long Size { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? InsertedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public long? InsertedUserId { get; set; }

        public long? ModifiedUserId { get; set; }
    }
}
