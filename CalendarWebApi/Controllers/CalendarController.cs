using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalendarWebApi.DataAccess;
using CalendarWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CalendarWebApi.DTO;
using Mapster;
using CalendarWebApi.Services;

namespace CalendarWebApi.Controllers
{
    [ApiController]
    [Route("api/calendar")]
    public class CalendarController : ControllerBase
    {
        private readonly ILogger<CalendarController> _logger;
        private readonly ICalendarService _calendarService;

        public CalendarController(ILogger<CalendarController> logger, ICalendarService calendarService)
        {
            _logger = logger;
            _calendarService =  calendarService;
        }

        [HttpPost]
        public async Task<ActionResult> AddEvent(CreateForm calendarForm) {
            var createdCalendar = await _calendarService.AddEvent(calendarForm);
            return CreatedAtAction(nameof(GetCalendarByQuery), new {id = createdCalendar.Id}, createdCalendar);
        }
       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCalendarEvent(int id) {
            var deleted = await _calendarService.DeleteEvent(id);
            return (deleted != null) ? NoContent(): NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCalendarEvent(int id, UpdateForm updateForm){
            var updated = await _calendarService.UpdateEvent(id, updateForm);
            return (updated != null)  ? Ok(): NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Calendar>>> GetCalendar(){
            var calendarEvents = await _calendarService.GetCalendar();
            if (calendarEvents != null) {
                return Ok(calendarEvents);
            } else {
                return Ok(new List<Calendar>());
            }
        }

        [HttpGet("query")]
        public async Task<ActionResult<IEnumerable<Calendar>>> GetCalendarByQuery(
            [FromQuery] string? eventOrganizer,
            [FromQuery] int? id,
            [FromQuery] string? location,
            [FromQuery] string? name) {
            var query = new Query();
            if (!string.IsNullOrEmpty(eventOrganizer)) {
                query.EventOrganizer = eventOrganizer;
            } 
            if (!string.IsNullOrEmpty(location)) {
                query.Location = location;
            }
            if (!string.IsNullOrEmpty(name)) {
                query.Name = name;
            }

            if (id.HasValue) {
                query.Id = id.Value;
            }
            
            var calendarEvents = await _calendarService.GetCalendar(query);
            if (calendarEvents != null) {
                return Ok(calendarEvents);
            } else {
                return new List<Calendar>();
            }
        }
       
        [HttpGet("sort")]
        public async Task<ActionResult<IEnumerable<Calendar>>> GetCalendarSortTime() {
            var sortedCalendar = await _calendarService.GetEventsSorted();
            return Ok(sortedCalendar);
        }
    }
}
