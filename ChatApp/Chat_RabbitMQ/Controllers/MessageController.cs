using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat_RabbitMQ.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chat_RabbitMQ.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IRabbitMQService rabbitMQService;
        public MessageController(IRabbitMQService _rabbitMQService){
            rabbitMQService = _rabbitMQService;
        }

        // [HttpPost("send")]
        // public IActionResult SendMessage([FromQuery] string queueName, [FromBody] string message)
        // {
        //     if (string.IsNullOrEmpty(message))
        //     {
        //         return BadRequest("Message cannot be null or empty.");
        //     }

        //     rabbitMQService.SendMessage(queueName, message);
        //     return Ok("Message sent successfully.");
        // }

        [HttpPost("send")]
        public IActionResult SendMessage([FromQuery] string queueName, [FromQuery] string Sender, [FromBody] string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return BadRequest("Message cannot be null or empty.");
            }
            if (string.IsNullOrEmpty(Sender))
            {
                return BadRequest("Sender cannot be null or empty.");
            }


            // Get the sender's username (you can pass this from the frontend or use the session)
            var sender = Sender; // Or use session/context to get the sender's username
            Console.WriteLine($"Sender: {sender}");
            // Send the message with the sender's username
            rabbitMQService.SendMessage(queueName, sender, message);
            return Ok("Message sent successfully.");
        }

        // [HttpGet("receive")]
        // public IActionResult ReceiveMessage([FromQuery] string queueName)
        // {
        //     var message = rabbitMQService.ReceiveMessage(queueName);
        //     if (message == null)
        //     {
        //         return NotFound("No messages available.");
        //     }

        //     return Ok(message);
        // }

        [HttpGet("receive")]
        public IActionResult ReceiveMessage([FromQuery] string queueName)
        {
            var (sender, message) = rabbitMQService.ReceiveMessage(queueName);
            if (sender == null || message == null)
            {
                return NotFound("No messages available.");
            }

            // Return a structured response
            return Ok(new { Sender = sender, Message = message });
        }
    
        // [HttpGet("GetAllUser")]
        // public async Task<IActionResult> AllUser(){
        //     // var users = await 
        // }
    }
}