using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IOtherIncomeValidator
    {

        bool ValidCreateObject(OtherIncome otherIncome, IOtherIncomeService _otherIncomeService);
        bool ValidUpdateObject(OtherIncome otherIncome, IOtherIncomeService _otherIncomeService);
        bool ValidDeleteObject(OtherIncome otherIncome);
        bool isValid(OtherIncome otherIncome);
        string PrintError(OtherIncome otherIncome);
    }
}