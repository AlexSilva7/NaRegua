﻿using NaRegua_API.Common.Contracts;
using System.Linq;
using static NaRegua_API.Models.Hairdresser.Requests;
using static NaRegua_API.Models.Hairdresser.Responses;

namespace NaRegua_API.Models.Hairdresser
{
    public static class Extensions
    {
        public static Hairdresser ToDomain(this HairdresserRequest input)
        {
            return new Hairdresser
            {
                Name = input.Name,
                Document = input.Document,
                Phone = input.Phone,
                Email = input.Email,
                Username = input.Username,
                Password = input.Password,
                SaloonCode = input.SaloonCode
            };
        }

        public static WorkAvailability ToDomain(this WorkAvailabilityRequest input)
        {
            return new WorkAvailability
            {
                StartHour = input.StartHour,
                EndHour = input.EndHour
            };
        }

        public static HairdresserResult ToResult(this Hairdresser input) 
        {
            return new HairdresserResult
            {
                Name = input.Name,
                Document = input.Document,
                Phone = input.Phone,
                Email = input.Email,
                SaloonCode = input.SaloonCode,
                IsCustomer = input.IsCustomer
            };
        } 

        public static ListHairdresserResponse ToResponse(this ListHairdresserResult input)
        {
            return new ListHairdresserResponse
            {
                Resources = input.Resources.Select(item => item.ToDomain())
            };
        }

        public static HairdresserResponse ToDomain(this HairdresserResult input)
        {
            return new HairdresserResponse
            {
                Name = input.Name,
                Document = input.Document,
                Email = input.Email,
                SaloonCode = input.SaloonCode,
                IsCustomer = input.IsCustomer
            };
        }

        public static ProfessionalAvailabilityResponse ToResponse(this ProfessionalAvailabilityResult input)
        {
            return new ProfessionalAvailabilityResponse
            {
                Document = input.Document,
                Resources = input.Resources
            };
        }

        public static AppointmentsListResponse ToResponse(this AppointmentsListResult input)
        {
            return new AppointmentsListResponse
            {
                Resources = input.Resources.Select(item => item.ToDomain())
            };
        }

        public static AppointmentsResponse ToDomain(this AppointmentsResult input)
        {
            return new AppointmentsResponse
            {
                CustomerName = input.CustomerName,
                CustomerDocument = input.CustomerDocument,
                CustomerPhone = input.CustomerPhone,
                Scheduling = input.Scheduling
            };
        }
    }
}
