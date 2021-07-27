using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2021Apr_P04_T4.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public DateTime Date { get; set; }
        public string Intro { get; set; }
        public string Content { get; set; }
    }
}
