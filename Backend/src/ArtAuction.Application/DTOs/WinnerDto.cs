using System;
using System.Collections.Generic;
using System.Text;


    namespace ArtAuction.Application.DTOs.Payment;

    public class WinnerDto
    {
        public int BuyerId { get; set; }
        public string BuyerFullName { get; set; } = null!;
        public string BuyerEmail { get; set; } = null!;
        public decimal FinalPrice { get; set; }
        public int ArtworkPostId { get; set; }
        public string ArtworkTitle { get; set; } = null!;
        public bool IsPaid { get; set; }
    }
