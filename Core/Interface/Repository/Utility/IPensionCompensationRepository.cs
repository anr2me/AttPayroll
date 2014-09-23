using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPensionCompensationRepository : IRepository<PensionCompensation>
    {
        IQueryable<PensionCompensation> GetQueryable();
        IList<PensionCompensation> GetAll();
        PensionCompensation GetObjectById(int Id);
        PensionCompensation CreateObject(PensionCompensation pensionCompensation);
        PensionCompensation UpdateObject(PensionCompensation pensionCompensation);
        PensionCompensation SoftDeleteObject(PensionCompensation pensionCompensation);
        bool DeleteObject(int Id);
    }
}