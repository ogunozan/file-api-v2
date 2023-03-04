using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using AutoMapper;
using Ozcorps.Ozt;
using Bll;
using Ozcorps.Logger;
using Ozcorps.Core.Models;
using Dto;

namespace FileApi.Controllers
{
    [ApiController]
    // [OztActionFilter]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IOzLogger _Logger;

        private readonly IFileService _FileService;

        private readonly IMapper _Mapper;

        public FileController(IOzLogger _logger, IFileService _fileService, IMapper _mapper)
        {
            _Logger = _logger;

            _FileService = _fileService;

            _Mapper = _mapper;
        }

        [HttpGet]
        [Route("{_entityName}/{_entityId}")]
        public Response GetFiles(long _entityId, string _entityName)
        {
            var _result = new Response();

            try
            {
                var _files = _FileService.GetAll(_entityId, _entityName).ToList();

                _result.Data = _Mapper.Map<IEnumerable<FileDto>>(_files);

                _result.Success = true;
            }
            catch (Exception _ex)
            {
                _Logger.Error(_ex);
            }

            return _result;
        }

        [HttpPost]
        public Response Add([FromForm(Name = "file")] IEnumerable<IFormFile> _files,
            [FromForm(Name = "entityName")] string _entityName,
            [FromForm(Name = "entityId")] long _entityId)
        {
            var _result = new Response();

            try
            {
                if (_files == null ||
                    !_files.Any() ||
                    string.IsNullOrEmpty(_entityName) ||
                    _entityId == 0)
                {
                    _result.Message = "Hatalı parametre!";

                    return _result;
                }

                _FileService.Add(new RequestAddFileDto
                {
                    EntityId = _entityId,
                    EntityName = _entityName,
                    Files = _files,
                    UserId = Request.HttpContext.GetUserId()
                });

                _result.Success = true;
            }
            catch (Exception _ex)
            {
                _Logger.Error(_ex);
            }

            return _result;
        }

        [HttpDelete]
        public Response DeleteFile(long _id)
        {
            var _result = new Response();

            try
            {
                var _file = _FileService.Delete(_id);

                if (_file == null)
                {
                    _result.Message = "Dosya Silinemedi";

                    return _result;
                }

                _result.Data = _Mapper.Map<FileDto>(_file);

                _result.Success = true;
            }
            catch (Exception _ex)
            {

                _Logger.Error(_ex);
            }

            return _result;
        }

        [HttpGet]
        [Route("{_id}")]
        public FileResult GetFile(long _id)
        {
            var _result = File(new byte[1], MediaTypeNames.Application.Octet, "not_found.txt");

            try
            {
                var _file = _FileService.Get(_id);

                if (_file == null)
                {
                    return _result;
                }

                _result = File(_file.Source, MediaTypeNames.Application.Octet, _file.Name);
            }
            catch (Exception _ex)
            {
                _Logger.Error(_ex);
            }

            return _result;
        }

        [HttpGet]
        [Route("GetFileByteArray/{_id}")]
        public Response GetFileByteArray(long _id)
        {
            var _result = new Response();

            try
            {
                var _file = _FileService.Get(_id);

                if (_file == null)
                {
                    _result.Message = "file couldn't found!";
                    
                    return _result;
                }

                _result.Data = _file;

                _result.Success = true;

            }
            catch (Exception _ex)
            {
                _Logger.Error(_ex);
            }

            return _result;
        }

    }
}
