using ContosoUniversity.Models.SchoolViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContosoUniversity.Data
{
    public class TaskService
    {
        private readonly HttpClient _client;

        public TaskService(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri("https://localhost:44315/");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = httpClient;
        }

        public async Task<IEnumerable<TaskView>> GetTaskViewsAsync()
        {
            var response = await _client.GetAsync("/api/TodoItems/");
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                IEnumerable<TaskView> converted = await JsonSerializer.DeserializeAsync <IEnumerable<TaskView>>(responseStream);
                return converted;
            } else
            {
                return new List<TaskView>();
            }
        }

        public async Task<TaskView> GetTaskViewByIdAsync(int id)
        {
            var response = await _client.GetAsync($"/api/TodoItems/{id}");
            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                TaskView converted = await JsonSerializer.DeserializeAsync <TaskView>(responseStream);
                return converted;
            } else
            {
                return new TaskView();
            }
        }

        public async Task<TaskView> PostTaskViewAsync(TaskView taskView)
        {
            var taskJson = new StringContent(JsonSerializer.Serialize(taskView), Encoding.UTF8, "application/json");

            using var httpResponse = await _client.PostAsync("/api/TodoItems/", taskJson);

            if (httpResponse.IsSuccessStatusCode)
            {
                using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                TaskView task = await JsonSerializer.DeserializeAsync <TaskView>(responseStream);
                return task;
            } else
            {
                return taskView;
            }
        }

        public async Task<bool> PutTaskViewAsync(TaskView taskView)
        {
            var taskJson = new StringContent(JsonSerializer.Serialize(taskView), Encoding.UTF8, "application/json");

            using var httpResponse = await _client.PutAsync($"/api/TodoItems/{taskView.Id}", taskJson);
            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTaskViewAsync(int id)
        {
            var httpResponse = await _client.DeleteAsync($"/api/TodoItems/{id}");

            return httpResponse.IsSuccessStatusCode;
        }
    }
}
