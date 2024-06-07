using CalendarWebApi.DataAccess;
using CalendarWebApi.DTO;
using CalendarWebApi.Models;

namespace CalendarWebApi.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IRepository _repository;
        public CalendarService(IRepository repository) {
            _repository = repository;
        }
        public async Task<Calendar> AddEvent(CreateForm calendarEvent)
        {
            var calendar = new Calendar {
                Name = calendarEvent?.Name!,
                Location = calendarEvent?.Location!,
                Time = calendarEvent.Time ?? 0,
                EventOrganizer = calendarEvent?.EventOrganizer!,
                Members = calendarEvent?.Members!
            };
            return await _repository.AddEvent(calendar);
        }

        public async Task<Calendar> DeleteEvent(int id)
        {
            var calendarEvent = await _repository.GetCalendar(id);
            return await _repository.DeleteEvent(calendarEvent);
        }

        public async Task<List<Calendar>> GetCalendar()
        {
            return await _repository.GetCalendar();
        }

        public async Task<Calendar> GetCalendar(int id)
        {
            return await _repository.GetCalendar(id);
        }

        public async Task<List<Calendar>> GetCalendar(Query query)
        {
            if (query != null) {
                var queryModel = new EventQueryModel();
                if (query.Id.HasValue) {
                    queryModel.Id = query.Id.Value;
                }
                queryModel.EventOrganizer = query.EventOrganizer;
                queryModel.Location = query.Location;
                queryModel.Name = query.Name;
                return await _repository.GetCalendar(queryModel);
            }
            return null;
        }

        public async Task<List<Calendar>> GetEventsSorted()
        {
            return await _repository.GetEventsSorted();
        }

        public async Task<Calendar> UpdateEvent(int id, UpdateForm calendarEvent)
        {
            var calendar = await _repository.GetCalendar(id);
            if (calendar != null) {
                calendar.Name = calendarEvent.Name ?? calendar.Name;
                calendar.EventOrganizer = calendarEvent.EventOrganizer ?? calendar.EventOrganizer;
                calendar.Location = calendarEvent.Location ?? calendar.Location;
                calendar.Members = calendarEvent.Members ?? calendar.Members;
                calendar.Time = calendarEvent.Time ?? calendar.Time;
                return await _repository.UpdateEvent(calendar);
            }

            return new Calendar();
        }
    }
}
