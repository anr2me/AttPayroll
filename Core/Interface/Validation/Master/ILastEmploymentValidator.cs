using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ILastEmploymentValidator
    {
        LastEmployment VCreateObject(LastEmployment lastEmployment, ILastEmploymentService _lastEmploymentService);
        LastEmployment VUpdateObject(LastEmployment lastEmployment, ILastEmploymentService _lastEmploymentService);
        LastEmployment VDeleteObject(LastEmployment lastEmployment);
        bool ValidCreateObject(LastEmployment lastEmployment, ILastEmploymentService _lastEmploymentService);
        bool ValidUpdateObject(LastEmployment lastEmployment, ILastEmploymentService _lastEmploymentService);
        bool ValidDeleteObject(LastEmployment lastEmployment);
        bool isValid(LastEmployment lastEmployment);
        string PrintError(LastEmployment lastEmployment);
    }
}