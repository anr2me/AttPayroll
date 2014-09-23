using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ITitleInfoValidator
    {
        TitleInfo VHasUniqueCode(TitleInfo titleInfo, ITitleInfoService _titleInfoService);
        TitleInfo VCreateObject(TitleInfo titleInfo, ITitleInfoService _titleInfoService);
        TitleInfo VUpdateObject(TitleInfo titleInfo, ITitleInfoService _titleInfoService);
        TitleInfo VDeleteObject(TitleInfo titleInfo);
        bool ValidCreateObject(TitleInfo titleInfo, ITitleInfoService _titleInfoService);
        bool ValidUpdateObject(TitleInfo titleInfo, ITitleInfoService _titleInfoService);
        bool ValidDeleteObject(TitleInfo titleInfo);
        bool isValid(TitleInfo titleInfo);
        string PrintError(TitleInfo titleInfo);
    }
}