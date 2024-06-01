using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;
using ThanhBuoi.Models.DTO;
using ThanhBuoi.Services;

namespace ThanhBuoi.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    public class Payment : ControllerBase
    {
        private readonly MomoServices _momoServices;

        public Payment(MomoServices momoServices)
        {
            _momoServices = momoServices;
        }

        [HttpPost("Pay")]
        public async Task<IActionResult> Pay([FromBody] PaymentDTO paymentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MomoPaymentResponseDTO momoPaymentResponseDTO = await _momoServices.Pay(paymentDTO);

            if (momoPaymentResponseDTO.ResultCode == 0)
            {
                return Ok(momoPaymentResponseDTO);
            }
            else
            {
                return BadRequest("Payment failed. Please try again later.");
            }


        }
        [HttpPost("CallBack")]
        public async Task<IActionResult> CallBack()
        {
            // Read the request body
            string requestBody;
            using (var reader = new System.IO.StreamReader(Request.Body))
            {
                requestBody = await reader.ReadToEndAsync();
            }

            // Deserialize the request body to a PaymentDTO object
            var paymentDTO = JsonConvert.DeserializeObject<PaymentDTO>(requestBody);

            // Validate the input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Call the Momo service to process the payment
            MomoPaymentResponseDTO momoPaymentResponseDTO = await _momoServices.Pay(paymentDTO);

            // Check if the payment was successful
            if (momoPaymentResponseDTO.ResultCode == 0)
            {
                // Return the payment details with HTTP status code 200 (OK)
                return Ok(momoPaymentResponseDTO);
            }
            else
            {
                // Return a meaningful error message with HTTP status code 400 (Bad Request)
                return BadRequest("Payment failed. Please try again later.");
            }
        }
    }
}
