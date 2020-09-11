using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListApp.Model;

namespace ToDoListApp.DTO
{
    public class TodoItemDTO
    {
        public TodoItemDTO()
        {

        }

        public TodoItemDTO(TodoItem todoItem)
        {
            Id = todoItem.Id;
            Name = todoItem.Name;
            IsCompleted = todoItem.IsCompleted;
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TodoItemDTO dTO &&
                   Id == dTO.Id &&
                   Name == dTO.Name &&
                   IsCompleted == dTO.IsCompleted;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, IsCompleted);
        }
    }
}
