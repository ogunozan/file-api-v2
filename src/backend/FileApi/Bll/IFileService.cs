using Bll;
using Dto;
using Dal;
using System.Collections.Generic;

namespace Bll
{
    public interface IFileService : IService
    {
        IEnumerable<File> GetAll(long _entityId, string _entityName);

        void Add(RequestAddFileDto _dto);

        ResponseGetFileDto Get(long _id);

        File Delete(long _id);
    }
}
