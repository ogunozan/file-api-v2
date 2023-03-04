using AutoMapper;
using Dal;
using Dto;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bll
{
    public class FileService : ServiceBase, IFileService
    {
        private readonly IRepository<Dal.File> _Repository;

        private readonly IMapper _Mapper;

        private readonly string _UploadPath;

        public FileService(IUnitOfWork _unitofWork,
            IConfiguration _config,
            IMapper _mapper) : base(_unitofWork)
        {
            _Repository = _unitofWork.GetRepository<Dal.File>();

            _UploadPath = _config["UploadPath"];

            _Mapper = _mapper;
        }

        public void Add(RequestAddFileDto _dto)
        {
            var _folderPath = Path.Combine(_UploadPath,
                _dto.EntityName,
                _dto.EntityId.ToString());

            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }

            foreach (var _file in _dto.Files)
            {
                var _fileName = Guid.NewGuid();

                var _filePath = Path.Combine(_folderPath,
                    _fileName.ToString() + "_" + _file.FileName);

                using Stream _stream = new FileStream(_filePath, FileMode.Create);

                _file.CopyTo(_stream);

                var _entityFile = _Mapper.Map<Dal.File>(_dto);

                _entityFile.Extension = _file.ContentType;

                _entityFile.Name = _file.FileName;

                _entityFile.Path = _filePath;

                _entityFile.Size = _file.Length;

                _entityFile.SetModifiedEntity(0, _isInsert: true);

                _Repository.Add(_entityFile);
            }

            Save();
        }

        public Dal.File Delete(long _id)
        {
            var _file = _Repository.Get(x => x.Id == _id && !x.IsDeleted);

            if (_file == null)
            {
                return null;
            }

            var _path = _file.Path;

            if (System.IO.File.Exists(_path))
            {
                System.IO.File.Delete(_path);
            }

            _file.IsDeleted = true;

            _file.SetModifiedEntity(0, true);

            Save();

            return _file;

        }

        public ResponseGetFileDto Get(long _id)
        {
            var _file = _Repository.Get(x => x.Id == _id && !x.IsDeleted);

            if (_file == null)
            {
                return null;
            }

            return new()
            {
                Name = _file.Name,
                Source = System.IO.File.ReadAllBytes(_file.Path),
            };
        }

        public IEnumerable<Dal.File> GetAll(long _entityId, string _entityName) =>
            _Repository.GetAll(x => x.EntityId == _entityId &&
                x.EntityName == _entityName && !x.IsDeleted);

        public void Save() => _UnitOfWork.Save();
    }
}
