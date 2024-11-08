using Microsoft.EntityFrameworkCore;
using ToDo.Models;

namespace ToDo.Context
{
    public class ToDoDB(DbContextOptions<ToDoDB> options) : DbContext(options)
    {
        public DbSet<ToDoItem> Todos { get; set; }
    }
}
