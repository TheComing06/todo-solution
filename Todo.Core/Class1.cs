using System.Net.Http.Headers;

namespace Todo.Core
{
    public class TodoItem
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Title { get; private set; }
        public Boolean IsDone { get; private set; }

        public TodoItem(string title) 
        {
            Title = title?.Trim() ?? throw new ArgumentNullException(nameof(title));
        }
        public void MarkDown() => IsDone = true;
        public void MarkUndone() => IsDone = false;
        public void Rename(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle)) throw new ArgumentException("Title required", nameof(newTitle));
            Title = newTitle.Trim();
        }
    }

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
    }
}
