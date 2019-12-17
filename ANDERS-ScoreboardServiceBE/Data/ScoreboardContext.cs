using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ANDERS_ScoreboardServiceBE;

namespace ANDERS_ScoreboardServiceBE.Models
{
    public class ScoreboardContext : DbContext
    {
        public ScoreboardContext (DbContextOptions<ScoreboardContext> options)
            : base(options)
        {
        }

        public DbSet<ANDERS_ScoreboardServiceBE.ScoreListing> ScoreListing { get; set; }
    }
}
