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

        public bool Save(string path)
        {
            string title = "Item";
            TodoItem item = _items.First(i => i.Title == title);
            string json = JsonSerializer.Serialize(item);

            try
            {
                File.WriteAllText(path + title + ".json", json);
                Console.WriteLine($"File has saved. \n{json}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }           

            return false;
        }

        public bool Load(string path)
        {
            string title = "Item";
            path = path + title + ".json";
            try
            {
                string jsonItem = File.ReadAllText(path);
                string jsonString = JsonSerializer.Deserialize<string>(jsonItem);

                var item = new TodoItem(jsonItem);

                _items.Add(item);

                Console.WriteLine($"File has loaded. \n{jsonString}");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}
