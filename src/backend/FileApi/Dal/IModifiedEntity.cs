using System;

namespace Dal
{
    public interface IModifiedEntity
    {
        bool IsActive { get; set; }

        bool IsDeleted { get; set; }

        DateTime? InsertedDate { get; set; }

        DateTime? ModifiedDate { get; set; }

        long? InsertedUserId { get; set; }

        long? ModifiedUserId { get; set; }
    }
}
