﻿using Project_C.Models.StoreModels;
using Project_C.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Project_C.Models.ProductModels
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public string Place { get; set; }
        public string? PhotoPath { get; set; }
        public string? VideoLink { get; set; }

        //elk product heeft een lijst met meerdere leveranciers, voor nu kan die ook leeg zijn (Many to many)
        // https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration
        public ICollection<Store>? Stores { get; set; }

        //Een senior heeft (mogelijk) meerdere producten in zijn cart
        public Guid? SeniorId { get; set; }
        public Senior? Senior { get; set; }
    }
}
