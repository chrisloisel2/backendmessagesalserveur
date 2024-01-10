using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace YourNamespace
{
	[ApiController]
	[Route("api/messages")]
	public class MessageController : ControllerBase
	{

		private readonly MyDbContext _context;

		public MessageController(MyDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult GetMessages()
		{
			var messages = _context.Messages;
			return Ok(messages);
		}

		[HttpGet("{id}")]
		public IActionResult GetMessage(int id)
		{
			var messages = _context.Messages.Find(id);

			return Ok(messages);
		}

		[HttpPost]
		public IActionResult CreateMessage([FromBody] string message)
		{
			_context.Messages.Add(new Message { Text = message });
			_context.SaveChanges();
			return Ok();
		}

		[HttpPut("{id}")]
		public IActionResult UpdateMessage(int id, [FromBody] string message)
		{
			var messages = _context.Messages.Find(id);
			messages.Text = message;
			_context.Messages.Update(messages);
			_context.SaveChanges();
			return NoContent();
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteMessage(int id)
		{
			var messages = _context.Messages.Find(id);
			_context.Messages.Remove(messages);
			_context.SaveChanges();
			return NoContent();
		}
	}
}
