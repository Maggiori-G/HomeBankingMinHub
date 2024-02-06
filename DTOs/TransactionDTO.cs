﻿using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.DTOs
{
	public class TransactionDTO
	{
		public long Id { get; set; }
        public string Type { get; set; }
        public double Ammount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public Account Account { get; set; }
	}
}