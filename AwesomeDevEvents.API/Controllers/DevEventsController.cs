using AwesomeDevEvents.API.Entities;
using AwesomeDevEvents.API.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDevEvents.API.Controllers
{
    [Route("api/dev-events")]
    [ApiController]
    public class DevEventsController : ControllerBase
    {
        private readonly DevEventsDbContext _context;
        public DevEventsController(DevEventsDbContext context)
        {
            _context = context;
        }

        [HttpGet("getAll")]
        public IActionResult GetAll() 
        {
            try
            {
                var devEvents = _context.DevEvents.Where(d => !d.IsDeleted).ToList();

                return Ok(devEvents);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getById/{id}")]   
        public IActionResult GetById(Guid id) 
        {
            try
            {
                var devEvents = _context.DevEvents
                    .Include(de => de.Speakers)
                    .SingleOrDefault(d => d.Id == id);

                if (devEvents == null)
                {
                    return NotFound();
                }

                return Ok(devEvents);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public IActionResult Post(DevEvent devEvent) 
        {
            try
            {
                _context.DevEvents.Add(devEvent);

                _context.SaveChanges();

                return CreatedAtAction(nameof(GetById), new { id = devEvent.Id}, devEvent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        public IActionResult Update(Guid id, DevEvent devEvent)
        {
            try
            {
                var devEvents = _context.DevEvents.SingleOrDefault(d => d.Id == id);

                if (devEvents == null)
                {
                    return NotFound();
                }

                devEvents.Update(devEvent.Title, devEvent.Description, devEvent.StartDate, devEvent.EndDate);

                _context.DevEvents.Update(devEvent);
                _context.SaveChanges();

                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var devEvents = _context.DevEvents.SingleOrDefault(d => d.Id == id);

                if (devEvents == null)
                {
                    return NotFound();
                }

                devEvents.Delete();

                _context.SaveChanges();

                return NoContent();
            }
            catch(Exception ex) 
            { 
                return BadRequest(ex.Message); 
            }
        }

        [HttpPost("{id}/include-speakers")]
        public IActionResult PostSpeaker(Guid id,DevEventSpeaker speaker) 
        {
            try
            {
                speaker.DevEventId = id;

                var devEvents = _context.DevEvents.Any(d => d.Id == id);

                if (!devEvents)
                {
                    return NotFound();
                }

                _context.DevEventsSpeakers.Add(speaker);
                _context.SaveChanges();

                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete("{speakerId}/delete-speakers")]
        public IActionResult DeleteSpeaker(Guid speakerId) 
        {
            try
            {
                var speaker = _context.DevEventsSpeakers.SingleOrDefault(deSpeaker =>  deSpeaker.Id == speakerId);


                if (speaker == null)
                {
                    return NotFound();
                }

                _context.Remove(speaker);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
