﻿using cross_zero_game.Models;
using cross_zero_game.Repositories.Interfaces;
using cross_zero_game.Services.Interfaces;
using cross_zero_game.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using PagedList;
using Newtonsoft.Json;
using System.Net.Mime;
using cross_zero_game.StateMachines;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace cross_zero_game.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameSessionController : ControllerBase
    {
        private readonly ILogger<GameSessionController> _logger;
        private IGameSessionService _gameSessionService;

        public GameSessionController(ILogger<GameSessionController> logger, IGameSessionService gameSessionService)
        {
            _logger = logger;
            _gameSessionService = gameSessionService;
        }

        [HttpPost("create")]
        public IActionResult CreateSession(GameSessionViewModel gameSessionViewModel)
        {
            if (ModelState.IsValid)
            {
                var creator = new Player()
                {
                    Id = Guid.NewGuid(),
                    Name = gameSessionViewModel.CreatorName
                };

                var gameSession = _gameSessionService.Add(gameSessionViewModel.Name, GameSessionStates.New, creator);

                return Ok(gameSession);
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpGet("{id}")]
        public IActionResult GetSession(Guid id)
        {
            return Ok(_gameSessionService.Get(id));
        }

        [HttpGet("list")]
        public IActionResult GetList()
        {
            return Ok(_gameSessionService.GetAll());
        }

        [HttpGet("pagedlist")]
        public IActionResult GetPagedList([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var pagedList = _gameSessionService.GetPagedList(pageNumber, pageSize);

            return Ok(pagedList);
        }

        [HttpGet("delete/{id}")]
        public IActionResult DeleteSession(Guid id)
        {
            _gameSessionService.Remove(id);

            return Ok();
        }

        [HttpGet("gamefield")]
        public IActionResult GetGameField(Guid id)
        {
            var gameSession = _gameSessionService.Get(id);

            var gameField = gameSession.GameField;

            return Ok(gameField);
        }
    }
}
