using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stock")]//route-attribute - ადგენს URL-ით რომელი კონკრეტული კონტროლერი და მისი კონკრეტული მეთოდი გამოიძახოს.
    [ApiController]
    public class StockController : ControllerBase //ControllerBase - გვაძლევს მეთოდებს რომ Api-დან პასუხები გავაგზავნოთ HTTP-ფორმატით:OK(),NotFound(),Created(),BadRequest().
    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var stocks = _context.Stock.ToList().Select(s => s.ToStockDto()); //ToList - Deffered execution(აღსრულება ლისტად გამოტანა) Select არის იგივე რაც javascript-ში map
            return Ok(stocks);
        }
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id){ //[FromRoute]-გამოიყენება ისეთი პარამეტრისთვის როგორიცაა Id, რომელიც პირდაპირ URL-შია. მაგ: api/stock/5
            var stock = _context.Stock.Find(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto) //[FromBody] ის ხშირად გამოიყენება POST, PUT და PATCH მოთხოვნებში, როცა მომხმარებელი აგზავნის მონაცემებს JSON ფორმატში და გინდა, რომ ეს მონაცემები ავტომატურად გადაიყვანო ობიექტად.
        {
            var stockModel = stockDto.ToStockFromCreateDTO();
            _context.Stock.Add(stockModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());//nameof() ფუნქცია აბრუნებს მითითებული მეთოდის სახელს როგორც string (ამ შემთხვევაში, GetById).
            //new { id = stockModel.Id }:აქ დინამიურად იქმნება ობიექტი
            //stockModel.ToStockDto():ეს არის ახალი Stock მოდელის DTO ფორმატში გარდაქმნა, რომელსაც შემდეგ დააბრუნებს პასუხად.
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto  updateDto)
        {
            var stockModel = _context.Stock.FirstOrDefault(s => s.Id == id);
            if (stockModel == null)
            {
                return NotFound();
            }
            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            _context.SaveChanges();
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]

        public IActionResult Delete([FromRoute] int id)
        {
            var stockModel = _context.Stock.FirstOrDefault(s => s.Id == id);
            if(stockModel == null)
            {
                return NotFound();
            }
            _context.Stock.Remove(stockModel);
            _context.SaveChanges();
            return Ok(stockModel.ToStockDto());
        }
    }
}