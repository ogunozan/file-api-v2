using Dal;

namespace Bll
{
    public abstract class ServiceBase
    {
        protected readonly IUnitOfWork _UnitOfWork;

        public ServiceBase(IUnitOfWork _unitOfWork)
        {
            _UnitOfWork = _unitOfWork;
        }
    }
}
