using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskService _taskService;

        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: TaskController
        public async Task<IActionResult> Index()
        {
            return View(await _taskService.GetTaskViewsAsync());
        }

        // GET: TaskController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var task = await _taskService.GetTaskViewByIdAsync(id);
            if (task.Id == 0)
            {
                ModelState.AddModelError(String.Empty, "Error establishing connection to back service, please refresh");
            }
            return View(task);
        }

        // GET: TaskController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaskController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id", "Name", "IsCompleted")] TaskView task)
        {
            try
            {
                var posted = await _taskService.PostTaskViewAsync(task);
                if (posted.Id == 0)
                {
                    ModelState.AddModelError(String.Empty, "Unable to connect to host");
                    return View(task);
                } else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                ModelState.AddModelError(String.Empty, "An error has occured");
                return View(task);
            }
        }

        // GET: TaskController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return await Details(id);
        }

        // POST: TaskController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id", "Name", "IsCompleted")] TaskView task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }
            try
            {
                if (!await _taskService.PutTaskViewAsync(task)) {
                    ModelState.AddModelError(String.Empty, "Error saving information");
                } else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                ModelState.AddModelError(String.Empty, "Error connection to host");
            }
            
            return View(task);
        }

        // GET: TaskController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return await Details(id);
        }

        // POST: TaskController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (await _taskService.DeleteTaskViewAsync(id))
                {
                    return RedirectToAction(nameof(Index));
                } else
                {
                    ModelState.AddModelError(String.Empty, "Delete failed");
                }
            }
            catch
            {
                ModelState.AddModelError(String.Empty, "Error connection to host");
            }

            return await Details(id);
        }
    }
}
