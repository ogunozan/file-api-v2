using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetTopologySuite.Geometries;
using Ozcorps.Core.Extensions;
using Ozcorps.Logger;

namespace Dal
{
    public partial class BaseDbContext : DbContext
    {
        private readonly IOzLogger _Logger;

        public virtual DbSet<File> Files { get; set; }

        public BaseDbContext(DbContextOptions<BaseDbContext> _options,
            IOzLogger _logger) : base(_options)
        {
            _Logger = _logger;
        }

        protected override void OnModelCreating(ModelBuilder _modelBuilder)
        {
            _modelBuilder.HasPostgresExtension("postgis");

            _modelBuilder.Entity<File>(_entity =>
            {
                _entity.ToTable("file");

                _entity.Property(e => e.Id).HasColumnName("id");

                _entity.Property(e => e.InsertedDate).HasColumnName("inserted_date");

                _entity.Property(e => e.InsertedUserId).HasColumnName("inserted_user_id");

                _entity.Property(e => e.IsActive).HasColumnName("is_active");

                _entity.Property(e => e.Description).HasColumnName("description");

                _entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                _entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                _entity.Property(e => e.ModifiedUserId).HasColumnName("modified_user_id");

                _entity.Property(e => e.EntityId).HasColumnName("entity_id");

                _entity.Property(e => e.Name).HasColumnName("name");

                _entity.Property(e => e.EntityName).HasColumnName("entity_name");

                _entity.Property(e => e.Extension).HasColumnName("extension");

                _entity.Property(e => e.Path).HasColumnName("path");

                _entity.Property(e => e.Size).HasColumnName("size");
            });
        }

        public override int SaveChanges()
        {
            var _entities = ChangeTracker.Entries().
                Where(x => x.State != EntityState.Unchanged &&
                    x.State != EntityState.Detached).
                Select(x => new EntityLog
                {
                    State = x.State,
                    Entity = x.Entity,
                    OriginalValues = x.OriginalValues
                }).
                ToList();

            var _result = base.SaveChanges();

            _ = LogAuditAsync(_entities);

            return _result;
        }

        private async Task LogAuditAsync(IEnumerable<EntityLog> _entities) =>
            await Task.Run(() =>
            {
                try
                {
                    var _logs = new List<AuditLog>();

                    foreach (var _entity in _entities)
                    {
                        var _entityName = _entity.Entity.GetType().Name;

                        var _primaryKey = _entity.OriginalValues.Properties.
                            FirstOrDefault(prop => prop.IsPrimaryKey()).Name;

                        var _id = _entity.OriginalValues.GetValue<long>(_primaryKey);

                        long? _userId = 0;

                        if (_entity.OriginalValues.Properties.Any(x => x.Name == "UserId"))
                        {
                            _userId = _entity.OriginalValues.GetValue<long>("UserId");
                        }
                        else if (_entity.State == EntityState.Added &&
                            _entity.OriginalValues.Properties.
                                Any(x => x.Name == "InsertedUserId"))
                        {
                            _userId = _entity.OriginalValues.GetValue<long?>("InsertedUserId");
                        }
                        else if (_entity.State == EntityState.Modified &&
                            _entity.OriginalValues.Properties.
                                Any(x => x.Name == "ModifiedUserId"))
                        {
                            _userId = _entity.OriginalValues.GetValue<long?>("ModifiedUserId");
                        }

                        var _geoloc = _entity.OriginalValues.Properties.
                            Any(x => x.Name == "Geoloc") ?
                                _entity.OriginalValues.GetValue<Geometry>("Geoloc").ToWkt() :
                                null;

                        var _log = new AuditLog
                        {
                            Date = DateTime.Now,
                            Entity = _entityName,
                            EntityId = _id,
                            Geoloc = _geoloc,
                            Json = JsonSerializer.Serialize(_entity.Entity),
                            Operation = _entity.State.ToString(),
                            UserId = _userId ?? 0
                        };

                        _logs.Add(_log);
                    }

                    _Logger.Audit(_logs);
                }
                catch (Exception _ex)
                {
                    _Logger.Error(_ex);
                }
            });

        private class EntityLog
        {
            public EntityState State { get; set; }

            public object Entity { get; set; }

            public PropertyValues OriginalValues { get; set; }
        }
    }
}
