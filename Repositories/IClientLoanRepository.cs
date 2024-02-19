﻿using HomeBankingMinHub.Models;

namespace HomeBankingMinHub.Intefaces
{
    public interface IClientLoanRepository
    {
        void Save(ClientLoans clientLoan);
    }
}