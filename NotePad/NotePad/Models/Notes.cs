using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NotePad.Models
{
    public class Notes
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; } = "";

        public DateTime Date { get; set; }

        public bool IsChecked { get; set; }

        public string HexColor { get; set; } = "#000000";
        public double Size { get; set; } = 12;
         
    }
}
