using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class WorkingTimeService : IWorkingTimeService
    {
        private IWorkingTimeRepository _repository;
        private IWorkingTimeValidator _validator;
        public WorkingTimeService(IWorkingTimeRepository _workingTimeRepository, IWorkingTimeValidator _workingTimeValidator)
        {
            _repository = _workingTimeRepository;
            _validator = _workingTimeValidator;
        }

        public IWorkingTimeValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<WorkingTime> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<WorkingTime> GetAll()
        {
            return _repository.GetAll();
        }

        public WorkingTime GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public WorkingTime GetObjectByCode(string code)
        {
            return _repository.GetObjectByCode(code);
        }

        public WorkingTime CreateObject(WorkingTime workingTime, IWorkingDayService _workingDayService)
        {
            workingTime.Errors = new Dictionary<String, String>();
            if(_validator.ValidCreateObject(workingTime, this))
            {
                workingTime.WorkInterval = (decimal)workingTime.CheckOut.Subtract(workingTime.CheckIn).TotalMinutes;
                workingTime.BreakInterval = (decimal)workingTime.BreakIn.Subtract(workingTime.BreakOut).TotalMinutes;
                _repository.CreateObject(workingTime);
                // Also create WorkingDays
                for (int i = 0; i < 7; i++)
                {
                    WorkingDay workingDay = new WorkingDay
                    {
                        WorkingTimeId = workingTime.Id,
                        Code = workingTime.Code + i.ToString(), // ((DayOfWeek)i).ToString()
                        Name = ((DayOfWeek)i).ToString(),
                        MinCheckIn = workingTime.MinCheckIn,
                        CheckIn = workingTime.CheckIn,
                        MaxCheckIn = workingTime.MaxCheckIn,
                        MinCheckOut = workingTime.MinCheckOut,
                        CheckOut = workingTime.CheckOut,
                        MaxCheckOut = workingTime.MaxCheckOut,
                        BreakOut = workingTime.BreakOut,
                        BreakIn = workingTime.BreakIn,
                        WorkInterval = workingTime.WorkInterval,
                        BreakInterval = workingTime.BreakInterval,
                        CheckInTolerance = workingTime.CheckInTolerance,
                        CheckOutTolerance = workingTime.CheckOutTolerance,
                    };
                    workingDay.IsEnabled = (i != 0 && i != 6);
                    _workingDayService.CreateObject(workingDay, this);
                }
            }
            return workingTime;
        }

        public WorkingTime UpdateObject(WorkingTime workingTime, IWorkingDayService _workingDayService)
        {
            if (_validator.ValidUpdateObject(workingTime, this))
            {
                workingTime.WorkInterval = (decimal)workingTime.CheckOut.Subtract(workingTime.CheckIn).TotalMinutes;
                workingTime.BreakInterval = (decimal)workingTime.BreakIn.Subtract(workingTime.BreakOut).TotalMinutes;
                _repository.UpdateObject(workingTime);
                // Also updates WorkingDays
                for (int i = 0; i < 7; i++)
                {
                    string code = workingTime.Code + i.ToString(); // ((DayOfWeek)i).ToString()
                    WorkingDay workingDay = _workingDayService.GetObjectByCode(code);
                    if (workingDay == null)
                    {
                        workingDay = new WorkingDay();
                    }
                    if(workingDay != null)
                    {
                        workingDay.WorkingTimeId = workingTime.Id;
                        workingDay.Code = code;
                        workingDay.Name = ((DayOfWeek)i).ToString();
                        //workingDay.IsEnabled = (i != 0 && i != 6);
                        workingDay.MinCheckIn = workingTime.MinCheckIn;
                        workingDay.CheckIn = workingTime.CheckIn;
                        workingDay.MaxCheckIn = workingTime.MaxCheckIn;
                        workingDay.MinCheckOut = workingTime.MinCheckOut;
                        workingDay.CheckOut = workingTime.CheckOut;
                        workingDay.MaxCheckOut = workingTime.MaxCheckOut;
                        workingDay.BreakOut = workingTime.BreakOut;
                        workingDay.BreakIn = workingTime.BreakIn;
                        workingDay.WorkInterval = workingTime.WorkInterval;
                        workingDay.BreakInterval = workingTime.BreakInterval;
                        workingDay.CheckInTolerance = workingTime.CheckInTolerance;
                        workingDay.CheckOutTolerance = workingTime.CheckOutTolerance;
                        _workingDayService.CreateOrUpdateObject(workingDay, this);
                    };
                    
                }
            }
            return workingTime;
        }

        public WorkingTime SoftDeleteObject(WorkingTime workingTime, IEmployeeWorkingTimeService _employeeWorkingTimeService)
        {
            return (workingTime = _validator.ValidDeleteObject(workingTime, _employeeWorkingTimeService) ?
                    _repository.SoftDeleteObject(workingTime) : workingTime);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(WorkingTime workingTime)
        {
            IQueryable<WorkingTime> workingTimes = _repository.FindAll(x => x.Code == workingTime.Code && !x.IsDeleted && x.Id != workingTime.Id);
            return (workingTimes.Count() > 0 ? true : false);
        }

        public DateTime SetTimeZone(DateTime dateTime, TimeZoneInfo timeZoneInfo, decimal additionalMinutesOffset = 0)
        {
            // Add TimeZone info to the new DateTime
            //string winTZ = fpMachine.TimeZone.ToUpper(); // FPDevice.Convertion.IanaToWindows(fpMachine.TimeZone);
            //TimeZoneInfo destTZ = TimeZoneInfo.GetSystemTimeZones().Where(x => x.Id.ToUpper() == winTZ).FirstOrDefault();
            DateTimeOffset dto = new DateTimeOffset(dateTime, timeZoneInfo.GetUtcOffset(dateTime.AddMinutes((double)additionalMinutesOffset)));
            return dto.LocalDateTime;
        }

    }
}