﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKTickets.Interfaces;
using PKTickets.Models.DTO;
using PKTickets.Models;
using PKTickets.Repository;

namespace PKTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowTimesController : ControllerBase
    {
        private readonly IShowTimeRepository showTimeRepository;

        public ShowTimesController(IShowTimeRepository _showTimeRepository)
        {
            showTimeRepository = _showTimeRepository;
        }

        [HttpGet("")]
        public IActionResult List()
        {
            return Ok(showTimeRepository.GetAllShowTimes());
        }

        [HttpGet("{showTimeId}")]

        public ActionResult GetById(int showTimeId)
        {
            var showTime = showTimeRepository.ShowTimeasStringById(showTimeId);
            return (showTime == null) ? NotFound("This Show Time Id is not Registered") : Ok(showTime);
        }


        [HttpPost("")]
        public IActionResult Add(ShowTimeDTO showTime)
        {
            var result = showTimeRepository.CreateShowTime(showTime);
            return (result.Status == Statuses.Created) ? Created($"{TimingConvert.LocalHost("ShowTimes")}{showTime.ShowTimeId}", result.Message) : 
                Conflict(result.Message);
        }


        [HttpPut("")]
        public IActionResult Update(ShowTimeDTO showTime)
        {
            var result = showTimeRepository.UpdateShowTime(showTime);
            return OutPut(result);
        }

        private IActionResult OutPut(Messages result)
        {
            switch (result.Status)
            {
                case Statuses.BadRequest:
                    return BadRequest(result.Message);
                case Statuses.NotFound:
                    return NotFound(result.Message);
                case Statuses.Conflict:
                    return Conflict(result.Message);
            }
            return Ok(result.Message);
        }
    }
}
