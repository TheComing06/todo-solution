using System.Net.Http.Headers;
using System.Text.Json;

namespace Todo.Core
{
    public class TodoList
    {
        private readonly List<TodoItem> _items = new();
        public IReadOnlyList<TodoItem> Items => _items.AsReadOnly();
        public TodoItem Add(string title)
        {
            var item = new TodoItem(title);
            _items.Add(item);
            return item;
        }

        public bool Remove(Guid id) => _items.RemoveAll(i => i.Id == id) > 0;

        public IEnumerable<TodoItem> Find(string substring) =>
            _items.Where(i => i.Title.Contains(substring ?? string.Empty, StringComparison.OrdinalIgnoreCase));

        public int Count => _items.Count;

        public void Save(string path, string title)
        {
            var todoItem = new TodoItem(title);

            string json = JsonSerializer.Serialize(todoItem);
            File.WriteAllText(path+title+".json", json);

            Console.WriteLine($"File has saved. \n{json}");
        }

        public void Load(string path)
        {
            string item = File.ReadAllText(path);
            string jsonString = JsonSerializer.Deserialize<string>(item);

            Console.WriteLine($"File has loaded. \n{jsonString}");
        }
    }
}
