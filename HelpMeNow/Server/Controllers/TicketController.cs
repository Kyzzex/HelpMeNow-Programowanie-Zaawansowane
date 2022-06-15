using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace HelpMeNow.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {

        private readonly DataContext _context;

        public IHttpContextAccessor _httpContextAccessor;



        public TicketController(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)); //------> Wyszukiwanie Id zalogowanego użytkownika

        public string GetUserEmail() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name); //------> Wyszukiwanie adresu Email zalogowanego użytkownika


        [HttpPost]
        public async Task<ActionResult<Ticket>> CreateTicket(Ticket ticket) //---------------> Tworzenie ticketu
        {

            _context.Tickets.Add(ticket);
            ticket.UserId = GetUserId();
            ticket.Email = GetUserEmail();
            await _context.SaveChangesAsync();
            return ticket;





        }


        [HttpGet]
        public async Task<ActionResult<List<Ticket>>> get() //---------------> Pobranie listy ticketów
        {
            return Ok(await _context.Tickets.ToListAsync());
        }

        [HttpGet("MyTickets")]
        public async Task<ActionResult<List<Ticket>>> GetOnlyMyTickets() //---------------> Pobranie listy moich ticketów
        {
            
            return Ok(await _context.Tickets
                 .Where(u => u.UserId.Equals(GetUserId()))
                    .ToListAsync()
                );
        }


        [HttpGet("{Id}")] //--------------------> Pobieranie ticketu po jego numerze Id
        public async Task<ActionResult<Ticket>> GetTicketbyId(int Id)
        {

            return (await _context.Tickets.FindAsync(Id));

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Ticket>> DeleteTicket(int id) //--------------> Usuwanie ticketu
        {
            var dbticket = await _context.Tickets.FindAsync(id);
            if (dbticket == null)
            {
                return BadRequest();
            }
            _context.Tickets.Remove(dbticket);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> PutTicket(Ticket ticket) //---------------------> Edycja Ticketu
        {
            var dbticket = await _context.Tickets.FirstOrDefaultAsync(i => i.Id == ticket.Id);

            if (dbticket == null)
            {
                return BadRequest();
            }
            dbticket.Title = ticket.Title;
            dbticket.Description = ticket.Description;
            dbticket.isOpen = ticket.isOpen;
            dbticket.Solution=ticket.Solution;
            dbticket.Prio=ticket.Prio;
            await _context.SaveChangesAsync();
            return Ok(dbticket);
        }


        [HttpPut("closeticket")]
        public async Task<IActionResult>CloseTicket(Ticket ticket) //---------------------> Zamknięcie ticketu
        {
            var dbticket = await _context.Tickets.FirstOrDefaultAsync(i => i.Id == ticket.Id);

            if (dbticket == null)
            {
                return BadRequest();
            }
           
            dbticket.isOpen = false;
            await _context.SaveChangesAsync();
            return Ok(dbticket);
        }
    }



}
