using Bll;
using Dal;
using Dto;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ozcorps.Core.Encryptors;
using Ozcorps.Logger;
using Ozcorps.Ozt;

var _builder = WebApplication.CreateBuilder(args);

_builder.Services.AddControllers();

_builder.Services.AddEndpointsApiExplorer();

_builder.Services.AddSwaggerGen();

_builder.Services.AddCors(o => o.AddPolicy("CorsPolicy",
    builder => builder.AllowAnyOrigin().
        AllowAnyMethod().
        AllowAnyHeader()));

_builder.Services.AddDbContext<BaseDbContext>(o => o.UseNpgsql(
    _builder.Configuration.GetConnectionString("Postgre"),
    x => x.UseNetTopologySuite()));

_builder.Services.Configure<OztConfig>(_builder.Configuration.GetSection("Ozt"));

_builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

_builder.Services.AddScoped<IFileService, FileService>();

_builder.Services.AddSingleton<IEncryptor, RsaEncryptor>();

_builder.Services.AddSingleton<OztTool>();

_builder.Services.AddSingleton<IOzLogger, TextLogger>();

_builder.Services.AddAutoMapper(typeof(MappingProfile));

var _app = _builder.Build();

if (_app.Environment.IsDevelopment())
{
    _app.UseSwagger();
    
    _app.UseSwaggerUI();
}

_app.UseHttpsRedirection();

_app.UseAuthorization();

_app.MapControllers();

_app.UseCors("CorsPolicy");

_app.Run();
