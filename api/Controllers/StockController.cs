using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> GetAll()//წარმოადგენს ასინქრონულ ოპერაციას, რომელსაც შეუძლია დააბრუნოს მნიშვნელობა. შედეგი არის IActionResult
        {
            var stocks = await _context.Stock.ToListAsync(); //ToList - Deffered execution(აღსრულება ლისტად გამოტანა) Select არის იგივე რაც javascript-ში map
            var stockDto = stocks.Select(s => s.ToStockDto());
            return Ok(stocks);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id){ //[FromRoute]-გამოიყენება ისეთი პარამეტრისთვის როგორიცაა Id, რომელიც პირდაპირ URL-შია. მაგ: api/stock/5
            var stock = await _context.Stock.FindAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto) //[FromBody] ის ხშირად გამოიყენება POST, PUT და PATCH მოთხოვნებში, როცა მომხმარებელი აგზავნის მონაცემებს JSON ფორმატში და გინდა, რომ ეს მონაცემები ავტომატურად გადაიყვანო ობიექტად.
        {
            var stockModel = stockDto.ToStockFromCreateDTO();
            await _context.Stock.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());//nameof() ფუნქცია აბრუნებს მითითებული მეთოდის სახელს როგორც string (ამ შემთხვევაში, GetById).
            //new { id = stockModel.Id }:აქ დინამიურად იქმნება ობიექტი
            //stockModel.ToStockDto():ეს არის ახალი Stock მოდელის DTO ფორმატში გარდაქმნა, რომელსაც შემდეგ დააბრუნებს პასუხად.
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto  updateDto)
        {
            var stockModel = await _context.Stock.FirstOrDefaultAsync(s => s.Id == id);
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

            await _context.SaveChangesAsync();
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _context.Stock.FirstOrDefaultAsync(s => s.Id == id);
            if(stockModel == null)
            {
                return NotFound();
            }
            _context.Stock.Remove(stockModel);
           await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}