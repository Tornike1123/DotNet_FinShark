using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Stock
    {
        public int Id { get; set; } // PrimaryKey
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")] //ატრიბუტი [Column(TypeName = "decimal(18,2)")] გამოიყენება, რათა მონაცემთა ბაზაში ეს ველი decimal ტიპით შეინახოს, მაქსიმუმ 18 ციფრი მთლიან ნაწილში და 2 ციფრი წერტილის შემდეგ.
        public decimal Purchase { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();//ინახება კლასის Comment ტიპის ობიექტები.


    }
}