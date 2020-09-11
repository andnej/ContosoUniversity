using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListApp.Model
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public string Secret { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TodoItem item &&
                   Id == item.Id &&
                   Name == item.Name &&
                   IsCompleted == item.IsCompleted &&
                   Secret == item.Secret;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, IsCompleted, Secret);
        }
    }
}
