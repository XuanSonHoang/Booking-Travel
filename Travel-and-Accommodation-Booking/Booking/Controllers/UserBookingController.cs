using AutoMapper;
using BookingHotel.Core.IServices;
using BookingHotel.Core.Models.Domain;
using BookingHotel.Core.Models.DTOs;
using BookingHotel.Core.Services.Communication;
using BookingHotel.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Controllers
{
    [ApiController]
    public class UserBookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper  _mapper;
        public UserBookingController(IBookingService bookingService, IMapper mapper)
        {
            _mapper = mapper;
            _bookingService = bookingService;
        }

        [HttpGet]
        [Route("api/user/bookings")]
        public async Task<IActionResult> bookings()
        {
            var result = await _bookingService.GetAllAsync();

            if(result == null)
            {
                return null;
            }
            var resouces = _mapper.Map<IEnumerable<Booking>, IEnumerable<BookingDTO>>(result);

            return Ok(resouces);
        }

        [HttpPost]
        [Route("api/user/bookings")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDTO bookingDTO)
        {
            // Xử lý yêu cầu tạo mới đặt vé của người dùng
            var result = _bookingService.CreateBooking(bookingDTO);
            //mapping
            var bookingModel = _mapper.Map<Booking>(bookingDTO);
            if(result == null)
            {
                return BadRequest();
            }

            return Ok(bookingModel);
        }

        [HttpDelete]
        [Route("api/user/bookings/{id}")]
        //[Route("{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var result = _bookingService.RemoveAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("api/user/bookings/{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            // Xử lý yêu cầu lấy thông tin đặt vé của người dùng theo ID
            var result = await _bookingService.GetByIdAsync(id);
            if (result == null)
            {
                //return string not found
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("api/user/bookings/{id}/invoice")]
        public async Task<IActionResult> GetBookingInvoice(int id)
        {
            // Xử lý yêu cầu lấy hóa đơn của đặt vé của người dùng theo ID
            var result = await _bookingService.GetInvoiceByBookingId(id);
            if (result == null)
            {
                //return string not found
                return NotFound();
            }
            return Ok(result);
        }
    }
}
