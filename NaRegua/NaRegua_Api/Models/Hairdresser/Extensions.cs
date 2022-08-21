using NaRegua_Api.Common.Contracts;
using static NaRegua_Api.Models.Hairdresser.Requests;
using static NaRegua_Api.Models.Hairdresser.Responses;

namespace NaRegua_Api.Models.Hairdresser
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
                CustomerPhone = input.CustomerPhone,
                Scheduling = input.DateTime
            };
        }

        public static EvaluationAverageResponse ToResponse(this EvaluationAverageResult input)
        {
            return new EvaluationAverageResponse
            {
                Average = input.Average
            };
        }

        public static ProfessionalEvaluation ToDomain(this ProfessionalEvaluationRequest input)
        {
            return new ProfessionalEvaluation
            {
                ProfessionalDocument = input.Document,
                Evaluation = input.Evaluation
            };
        }

        public static AppointmentsResult ToResult(this Scheduling input)
        {
            return new AppointmentsResult
            {
                CustomerName = input.CustomerName,
                CustomerPhone = input.CustomerPhone,
                DateTime = input.DateTime
            };
        }
    }
}
