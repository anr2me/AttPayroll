using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using NSpec;
using Service.Service;
using Core.Interface.Service;
using Data.Context;
using System.Data.Entity;
using Data.Repository;
using Validation.Validation;

namespace TestValidation
{

    public class SpecAttendanceData : nspec
    {

        DataBuilder d;
        void before_each()
        {
            // something to do before checked
            var db = new AttPayrollEntities();
            using (db)
            {
                db.DeleteAllTables();
                d = new DataBuilder();
                d.PopulateData();

                

            }
        }

        void WorkingTime_validation()
        {

            it["validates_workingTime"] = () =>
            {
                d.wt1.Errors.Count().should_be(0);
            };

            it["workingTime_with_no_code"] = () =>
            {
                WorkingTime obj = new WorkingTime()
                {
                    Name = "no code",
                    MinCheckIn = DateTime.Now,
                    CheckIn = DateTime.Now.AddHours(3),
                    MaxCheckIn = DateTime.Now.AddHours(6),
                    BreakOut = DateTime.Now.AddHours(7),
                    BreakIn = DateTime.Now.AddHours(8),
                    MinCheckOut = DateTime.Now.AddHours(9),
                    CheckOut = DateTime.Now.AddHours(12),
                    MaxCheckOut = DateTime.Now.AddHours(15),
                };
                obj = d._workingTimeService.CreateObject(obj, d._workingDayService);
                obj.Errors.Count().should_not_be(0);
            };

            it["workingTime_with_same_code"] = () =>
            {
                WorkingTime obj = new WorkingTime()
                {
                    Code = d.wt1.Code,
                    Name = "name",
                    MinCheckIn = DateTime.Now,
                    CheckIn = DateTime.Now.AddHours(3),
                    MaxCheckIn = DateTime.Now.AddHours(6),
                    BreakOut = DateTime.Now.AddHours(7),
                    BreakIn = DateTime.Now.AddHours(8),
                    MinCheckOut = DateTime.Now.AddHours(9),
                    CheckOut = DateTime.Now.AddHours(12),
                    MaxCheckOut = DateTime.Now.AddHours(15),
                };
                obj = d._workingTimeService.CreateObject(obj, d._workingDayService);
                obj.Errors.Count().should_not_be(0);
            };

            it["workingTime_with_no_name"] = () =>
            {
                WorkingTime obj = new WorkingTime()
                {
                    Code = "no_name",
                    MinCheckIn = DateTime.Now,
                    CheckIn = DateTime.Now.AddHours(3),
                    MaxCheckIn = DateTime.Now.AddHours(6),
                    BreakOut = DateTime.Now.AddHours(7),
                    BreakIn = DateTime.Now.AddHours(8),
                    MinCheckOut = DateTime.Now.AddHours(9),
                    CheckOut = DateTime.Now.AddHours(12),
                    MaxCheckOut = DateTime.Now.AddHours(15),
                };
                obj = d._workingTimeService.CreateObject(obj, d._workingDayService);
                obj.Errors.Count().should_not_be(0);
            };

            it["workingTime_with_no_mincheckin"] = () =>
            {
                WorkingTime obj = new WorkingTime()
                {
                    Code = "code",
                    Name = "no mincheckin",
                    CheckIn = DateTime.Now.AddHours(3),
                    MaxCheckIn = DateTime.Now.AddHours(6),
                    BreakOut = DateTime.Now.AddHours(7),
                    BreakIn = DateTime.Now.AddHours(8),
                    MinCheckOut = DateTime.Now.AddHours(9),
                    CheckOut = DateTime.Now.AddHours(12),
                    MaxCheckOut = DateTime.Now.AddHours(15),
                };
                obj = d._workingTimeService.CreateObject(obj, d._workingDayService);
                obj.Errors.Count().should_not_be(0);
            };

            it["workingTime_with_no_checkin"] = () =>
            {
                WorkingTime obj = new WorkingTime()
                {
                    Code = "code",
                    Name = "no checkin",
                    MinCheckIn = DateTime.Now,
                    MaxCheckIn = DateTime.Now.AddHours(6),
                    BreakOut = DateTime.Now.AddHours(7),
                    BreakIn = DateTime.Now.AddHours(8),
                    MinCheckOut = DateTime.Now.AddHours(9),
                    CheckOut = DateTime.Now.AddHours(12),
                    MaxCheckOut = DateTime.Now.AddHours(15),
                };
                obj = d._workingTimeService.CreateObject(obj, d._workingDayService);
                obj.Errors.Count().should_not_be(0);
            };

            it["workingTime_with_no_maxcheckin"] = () =>
            {
                WorkingTime obj = new WorkingTime()
                {
                    Code = "code",
                    Name = "no maxcheckin",
                    MinCheckIn = DateTime.Now,
                    CheckIn = DateTime.Now.AddHours(3),
                    BreakOut = DateTime.Now.AddHours(7),
                    BreakIn = DateTime.Now.AddHours(8),
                    MinCheckOut = DateTime.Now.AddHours(9),
                    CheckOut = DateTime.Now.AddHours(12),
                    MaxCheckOut = DateTime.Now.AddHours(15),
                };
                obj = d._workingTimeService.CreateObject(obj, d._workingDayService);
                obj.Errors.Count().should_not_be(0);
            };

            it["workingTime_with_no_breakout"] = () =>
            {
                WorkingTime obj = new WorkingTime()
                {
                    Code = "code",
                    Name = "no breakout",
                    MinCheckIn = DateTime.Now,
                    CheckIn = DateTime.Now.AddHours(3),
                    MaxCheckIn = DateTime.Now.AddHours(6),
                    BreakIn = DateTime.Now.AddHours(8),
                    MinCheckOut = DateTime.Now.AddHours(9),
                    CheckOut = DateTime.Now.AddHours(12),
                    MaxCheckOut = DateTime.Now.AddHours(15),
                };
                obj = d._workingTimeService.CreateObject(obj, d._workingDayService);
                obj.Errors.Count().should_not_be(0);
            };

            it["workingTime_with_no_breakin"] = () =>
            {
                WorkingTime obj = new WorkingTime()
                {
                    Code = "code",
                    Name = "no breakin",
                    MinCheckIn = DateTime.Now,
                    CheckIn = DateTime.Now.AddHours(3),
                    MaxCheckIn = DateTime.Now.AddHours(6),
                    BreakOut = DateTime.Now.AddHours(7),
                    MinCheckOut = DateTime.Now.AddHours(9),
                    CheckOut = DateTime.Now.AddHours(12),
                    MaxCheckOut = DateTime.Now.AddHours(15),
                };
                obj = d._workingTimeService.CreateObject(obj, d._workingDayService);
                obj.Errors.Count().should_not_be(0);
            };

            it["workingTime_with_no_mincheckout"] = () =>
            {
                WorkingTime obj = new WorkingTime()
                {
                    Code = "code",
                    Name = "no breakin",
                    MinCheckIn = DateTime.Now,
                    CheckIn = DateTime.Now.AddHours(3),
                    MaxCheckIn = DateTime.Now.AddHours(6),
                    BreakOut = DateTime.Now.AddHours(7),
                    BreakIn = DateTime.Now.AddHours(8),
                    CheckOut = DateTime.Now.AddHours(12),
                    MaxCheckOut = DateTime.Now.AddHours(15),
                };
                obj = d._workingTimeService.CreateObject(obj, d._workingDayService);
                obj.Errors.Count().should_not_be(0);
            };

            it["workingTime_with_no_checkout"] = () =>
            {
                WorkingTime obj = new WorkingTime()
                {
                    Code = "code",
                    Name = "no breakin",
                    MinCheckIn = DateTime.Now,
                    CheckIn = DateTime.Now.AddHours(3),
                    MaxCheckIn = DateTime.Now.AddHours(6),
                    BreakOut = DateTime.Now.AddHours(7),
                    BreakIn = DateTime.Now.AddHours(8),
                    MinCheckOut = DateTime.Now.AddHours(9),
                    MaxCheckOut = DateTime.Now.AddHours(15),
                };
                obj = d._workingTimeService.CreateObject(obj, d._workingDayService);
                obj.Errors.Count().should_not_be(0);
            };

            it["workingTime_with_no_maxcheckout"] = () =>
            {
                WorkingTime obj = new WorkingTime()
                {
                    Code = "code",
                    Name = "no breakin",
                    MinCheckIn = DateTime.Now,
                    CheckIn = DateTime.Now.AddHours(3),
                    MaxCheckIn = DateTime.Now.AddHours(6),
                    BreakOut = DateTime.Now.AddHours(7),
                    BreakIn = DateTime.Now.AddHours(8),
                    MinCheckOut = DateTime.Now.AddHours(9),
                    CheckOut = DateTime.Now.AddHours(12),
                };
                obj = d._workingTimeService.CreateObject(obj, d._workingDayService);
                obj.Errors.Count().should_not_be(0);
            };

            it["update_workingTime_with_no_code"] = () =>
            {
                d.wt1.Code = "";
                d._workingTimeService.UpdateObject(d.wt1, d._workingDayService);
                d.wt1.Errors.Count().should_not_be(0);
            };

            it["delete_workingTime_having_employeeworkingtime"] = () =>
            {
                d._workingTimeService.SoftDeleteObject(d.wt1, d._employeeWorkingTimeService);
                d.wt1.Errors.Count().should_not_be(0);
            };

        }

        void EmployeeAttendance_validation()
        {

            it["validates_employeeAttendance"] = () =>
            {
                d.empatt.Errors.Count().should_be(0);
            };

            it["employeeAttendance_with_no_employee"] = () =>
            {
                EmployeeAttendance obj = new EmployeeAttendance()
                {
                    AttendanceDate = DateTime.Now,
                    CheckIn = DateTime.Now,
                    CheckOut = DateTime.Now.AddHours(8),
                };
                obj = d._employeeAttendanceService.CreateObject(obj, d._employeeService);
                obj.Errors.Count().should_not_be(0);
            };

            it["employeeAttendance_with_no_attendancedate"] = () =>
            {
                EmployeeAttendance obj = new EmployeeAttendance()
                {
                    EmployeeId = d.emp1.Id,
                    CheckIn = DateTime.Now,
                    CheckOut = DateTime.Now.AddHours(8),
                };
                obj = d._employeeAttendanceService.CreateObject(obj, d._employeeService);
                obj.Errors.Count().should_not_be(0);
            };

            it["employeeAttendance_with_no_checkin"] = () =>
            {
                EmployeeAttendance obj = new EmployeeAttendance()
                {
                    EmployeeId = d.emp1.Id,
                    AttendanceDate = DateTime.Now,
                    CheckOut = DateTime.Now.AddHours(8),
                };
                obj = d._employeeAttendanceService.CreateObject(obj, d._employeeService);
                obj.Errors.Count().should_not_be(0);
            };

            it["employeeAttendance_with_no_checkout"] = () =>
            {
                EmployeeAttendance obj = new EmployeeAttendance()
                {
                    EmployeeId = d.emp1.Id,
                    AttendanceDate = DateTime.Now,
                    CheckIn = DateTime.Now.AddHours(8),
                };
                obj = d._employeeAttendanceService.CreateObject(obj, d._employeeService);
                obj.Errors.Count().should_not_be(0);
            };

            it["update_employeeAttendance_with_no_employee"] = () =>
            {
                d.empatt.EmployeeId = 0;
                d._employeeAttendanceService.UpdateObject(d.empatt, d._employeeService);
                d.empatt.Errors.Count().should_not_be(0);
            };

            it["delete_employeeAttendance"] = () =>
            {
                d._employeeAttendanceService.SoftDeleteObject(d.empatt);
                d.empatt.Errors.Count().should_be(0);
            };

        }

        

    }
}