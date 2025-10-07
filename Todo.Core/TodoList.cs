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
            
            try
            {
                TodoItem item = _items[0];
                string json = JsonSerializer.Serialize(item);

                File.WriteAllText(path + title + ".json", json);
                Console.WriteLine($"File has saved. \n{json}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }           
        }

        public bool Load(string path)
        {
            string title = "Item";
            string fullPath = path + title + ".json";
            
            try
            {
                if (!File.Exists(fullPath))
                {
                    Console.WriteLine("File not found.");
                    return false;
                }

                string json = File.ReadAllText(fullPath);
                var items = JsonSerializer.Deserialize<TodoItem>(json);

                if (items != null)
                {
                    _items.Clear();
                    _items.Add(items);

                    Console.WriteLine($"File has loaded.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to deserialize item.");
                    return false;
                }    
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
