using System;

namespace NaRegua_API.Models.Hairdresser
{
    public class WorkAvailability
    {
        public DateTime Date { get; set; }
        public TimeSpan StartHour { get; set; }
        public TimeSpan EndHour { get; set; }
    }
}
