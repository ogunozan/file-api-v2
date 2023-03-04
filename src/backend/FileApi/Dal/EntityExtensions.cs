using System;

namespace Dal
{
    public static class EntityExtensions
    {
        public static void SetModifiedEntity(this IModifiedEntity _entity,
            long _userId,
            bool _isDeleted = false,
            bool _isActive = true,
            bool _isInsert = false)
        {
            if (!_isInsert)
            {
                _entity.ModifiedDate = DateTime.UtcNow;

                _entity.ModifiedUserId = _userId;
            }
            else
            {
                _entity.InsertedDate = DateTime.UtcNow;

                _entity.InsertedUserId = _userId;
            }

            _entity.IsDeleted = _isDeleted;

            _entity.IsActive = _isActive;
        }

        public static void SetInsertedEntity(this IInsertedEntity _entity, long _userId)
        {
            _entity.InsertedDate = DateTime.UtcNow;

            _entity.InsertedUserId = _userId;
        }
    }
}
