using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListApp.Controllers;
using ToDoListApp.DTO;
using ToDoListApp.Model;

namespace ToDoListApp.Controllers
{
    [TestFixture]
    public class TodoItemsControllerTest
    {
        private static Random random = new Random();

        private DbContextOptions<TodoContext> _contextOptions;

        [SetUp]
        public void Setup()
        {
            _contextOptions = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase("ToDoList")
                .Options;
            using (var context = new TodoContext(_contextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add(new TodoItem() { Name = "Doing Something", IsCompleted = true });
                context.Add(new TodoItem() { Name = "Learn Something", IsCompleted = true });
                context.Add(new TodoItem() { Name = "Delete Something", IsCompleted = false });

                context.SaveChanges();
            }
        }

        public static int GetRandomId(TodoContext context)
        {
            return random.Next(1, context.TodoItems.Count());
        }

        [Test]
        public async Task GivenTodoItemsExists_WhenGetWithoutId_ThenReturnAllTodoItems()
        {
            using (var context = new TodoContext(_contextOptions))
            {
                TodoItemsController controller = new TodoItemsController(context);
                var actionResult = await controller.GetTodoItems();
                var items = actionResult.Value.ToList();
                Assert.AreEqual(3, items.Count);
                var toFind = new TodoItemDTO(context.TodoItems.First());
                Assert.IsTrue(items.Contains(toFind));
            }
        }

        [Test]
        public async Task GivenTodoItemExists_WhenPostNewTodoItem_ThenItemSaved()
        {
            using (var context = new TodoContext(_contextOptions))
            {
                TodoItemsController controller = new TodoItemsController(context);
                TodoItemDTO toPost = new TodoItemDTO() { Name = "New One", IsCompleted = false };
                var postResult = await controller.PostTodoItem(toPost);
                TodoItemDTO postedItem = (TodoItemDTO)(postResult.Result as Microsoft.AspNetCore.Mvc.CreatedAtActionResult).Value;
                Assert.IsNotNull(postedItem.Id);
                Assert.AreEqual(toPost.Name, postedItem.Name);
                Assert.AreEqual(toPost.IsCompleted, postedItem.IsCompleted);

                Assert.AreEqual(4, await context.TodoItems.CountAsync());
            }
        }

        [Test]
        public async Task GivenTodoItemsExists_WhenGetWithId_ThenReturnsExactResult()
        {
            using (var context = new TodoContext(_contextOptions))
            {
                TodoItemsController controller = new TodoItemsController(context);
                int id = GetRandomId(context);
                var expected = await context.TodoItems.FirstOrDefaultAsync(t => t.Id == id);
                var findResult = await controller.GetTodoItem(id);
                Assert.AreEqual(new TodoItemDTO(expected), findResult.Value);
            }
        }

        [Test]
        public async Task GivenIdNotExist_WhenGetWithId_ThenReturnsNotFound()
        {
            using (var context = new TodoContext(_contextOptions))
            {
                TodoItemsController controller = new TodoItemsController(context);
                var invalidId = context.TodoItems.Count() + 1;

                var findResult = await controller.GetTodoItem(invalidId);
                Assert.AreEqual(typeof(NotFoundResult), findResult.Result.GetType());
            }
        }

        [Test]
        public async Task GivenTodoItemExist_WhenPut_ThenTodoItemUpdated()
        {
            using (var context = new TodoContext(_contextOptions))
            {
                TodoItemsController controller = new TodoItemsController(context);
                int id = GetRandomId(context);
                TodoItem toChange = await context.TodoItems.FirstOrDefaultAsync(t => t.Id == id);
                String newName = "Don't do this at home";
                toChange.Name = newName;
                var changeResult = await controller.PutTodoItem(id, new TodoItemDTO(toChange));
                Assert.AreEqual(typeof(NoContentResult), changeResult.GetType());

                TodoItem freshFetched = await context.TodoItems.FirstOrDefaultAsync(t => t.Id == id);
                Assert.AreEqual(newName, freshFetched.Name);
            }
        }

        public async Task GivenTodoItemWithDifferentId_WhenPut_ThenReturnBadRequest()
        {
            using (var context = new TodoContext(_contextOptions))
            {
                TodoItemsController controller = new TodoItemsController(context);
                int id = GetRandomId(context);
                TodoItem toChange = await context.TodoItems.FirstOrDefaultAsync(t => t.Id == id);
                String oldName = toChange.Name;
                String newName = "Don't do this at home";
                toChange.Name = newName;
                toChange.Id = toChange.Id == 1 ? 2 : 1;
                var changeResult = await controller.PutTodoItem(id, new TodoItemDTO(toChange));
                Assert.AreEqual(typeof(BadRequestResult), changeResult.GetType());

                TodoItem freshFetched = await context.TodoItems.FirstOrDefaultAsync(t => t.Id == id);
                Assert.AreEqual(oldName, freshFetched.Name);
            }
        }

        public async Task GivenTodoItemExists_WhenDelete_ThenReturnDeletedItem()
        {
            using (var context = new TodoContext(_contextOptions))
            {
                TodoItemsController controller = new TodoItemsController(context);
                int id = GetRandomId(context);
                var deleteResult = await controller.DeleteTodoItem(id);
                Assert.AreEqual(id, deleteResult.Value.Id);

                TodoItem freshFetched = await context.TodoItems.FirstOrDefaultAsync(t => t.Id == id);
                Assert.IsNull(freshFetched);
                Assert.AreEqual(2, await context.TodoItems.CountAsync());
            }
        }

        public async Task GivenTodoItemNotExists_WhenDelete_ThenReturnNotFound()
        {
            using (var context = new TodoContext(_contextOptions))
            {
                TodoItemsController controller = new TodoItemsController(context);
                var invalidId = context.TodoItems.Count() + 1;

                var deleteResult = await controller.DeleteTodoItem(invalidId);
                Assert.AreEqual(typeof(NotFoundResult), deleteResult.Result.GetType());
                Assert.AreEqual(3, await context.TodoItems.CountAsync());
            }

        }
    }
}
