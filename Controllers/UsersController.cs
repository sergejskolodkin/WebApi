﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        UsersContext db;
        public UsersController(UsersContext context)
        {
            db = context;
            if (!db.Users.Any())
            {
                db.Users.Add(new UsersDto { Name = "Иван", Patronymic = "Иванович", Surname = "Иванов", BirthDate = new DateTime(2000, 3, 6) });
                db.Users.Add(new UsersDto { Name = "Сергей", Patronymic = "Сергеевич", Surname = "Сергеев", BirthDate = new DateTime(2001, 4, 7) });
                db.Users.Add(new UsersDto { Name = "Вадим", Patronymic = "Вадимович", Surname = "Вадимов", BirthDate = new DateTime(2002, 5, 8) });
                db.SaveChanges();
            }
        }
        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersDto>>> Get() 
        {
            var users = await db.Users.ToListAsync();
            if (users.Count== 0)
            {
                return NotFound();
            }
            return users;
          
        }


        // GET api/<UsersController>/5
        [HttpGet("{Name}, {Patronymic}, {Surname}")]
        public async Task<ActionResult<UsersDto>> Get(string Name, string Patronymic, string Surname)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Name == Name);
            if (user==null)
            {
                return NotFound();
            }
            return user;
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult<UsersDto>> Post(UsersDto user )
        {
            var users = db.Users;
            if (user == null)
            {
                return BadRequest();
            }

            users.Add(user);
            await db.SaveChangesAsync();
            return Ok(users);
        }

      

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UsersDto>> Delete(int id)
        {
            var user = db.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }
    }
}
