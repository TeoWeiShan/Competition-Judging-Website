using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2021Apr_P04_T4.Models
{
    public class CompetitionJudgeViewModel
    {
        public List<Competition> competitionList { get; set; }
        public List<Judge> judgeList { get; set; }

        public CompetitionJudgeViewModel()
        {
            competitionList = new List<Competition>();
            judgeList = new List<Judge>();
        }
    }
}
