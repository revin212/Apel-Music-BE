﻿namespace fs_12_team_1_BE.DTO.TsOrder
{
    public class TsOrderGetInvoiceDetailHeaderRes
    { 
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime? OrderDate { get; set; }
        public double TotalHarga { get; set; }
    }
}
