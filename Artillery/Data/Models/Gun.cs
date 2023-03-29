﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery.Data.Models
{
    public class Gun
    {
        public Gun()
        {
            CountriesGuns = new HashSet<CountryGun>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public int ManufacturerId  { get; set; }
        [Required]
        public string GunWeight { get; set;}
        [Required]
        public double BarrelLength  { get; set; }
        public int NumberBuild { get; set; }
        [Required]
        public int Range { get; set; }
        [Required]
        public GunType GunType { get; set; }
        [Required]
        public int ShellId { get; set; }
        public ICollection<CountryGun> CountriesGuns { get; set; }
    }
}