using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Journal> Journals { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
    }
}
