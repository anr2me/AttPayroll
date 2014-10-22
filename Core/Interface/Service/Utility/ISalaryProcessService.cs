using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalaryProcessService
    {
        string ProcessEmployee(Nullable<int> EmployeeId, DateTime yearMonth, int NoSlip = 1, string Disiapkan_oleh = null, string Disetujui_oleh = null, string Dikoreksi_oleh = null);
    }
}