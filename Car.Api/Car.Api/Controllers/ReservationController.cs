using CarRental.Core.Entities;
using CarRental.Infa.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReservationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/reservations
        [HttpGet]
        public IActionResult GetAll()
        {
            var reservations = _context.Reservations.ToList();
            return Ok(reservations);
        }

        // GET: api/reservations/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation == null)
                return NotFound();
            return Ok(reservation);
        }

        // POST: api/reservations
        [HttpPost]
        public IActionResult Create([FromBody] Reservation reservation)
        {
            if (reservation == null)
                return BadRequest();

            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            return Ok(reservation);
        }

        // PUT: api/reservations/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Reservation updatedReservation)
        {
            var existing = _context.Reservations.Find(id);
            if (existing == null)
                return NotFound();

            existing.CarId = updatedReservation.CarId;
            existing.StartDate = updatedReservation.StartDate;
            existing.EndDate = updatedReservation.EndDate;
            existing.Status = updatedReservation.Status;

            _context.SaveChanges();
            return Ok(existing);
        }

        // DELETE: api/reservations/5
        [HttpDelete("{id}")]
        public IActionResult Cancel(int id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation == null)
                return NotFound();

            _context.Reservations.Remove(reservation);
            _context.SaveChanges();
            return Ok("Reservation cancelled successfully");
        }
    }
}
