using Dal;

namespace Dal
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BaseDbContext _Context;

        public UnitOfWork(BaseDbContext _context) => 
            _Context = _context;
     
        public IRepository<T> GetRepository<T>() where T : EntityBase => 
            new Repository<T>(_Context);

        public void Save() => 
            _Context.SaveChanges();
    }
}
