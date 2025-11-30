using Xunit;
using Todo.Core;
using System.Linq;
using System.IO;

namespace Todo.Core.Tests
{
    public class TodoListTests
    {
        [Fact]
        public void Add_IncreasesCount()
        {
            var list = new TodoList();
            list.Add(" task ");
            Assert.Equal(1, list.Count);
            Assert.Equal("task", list.Items.First().Title);
        }

        [Fact]
        public void Remove_ById_Works()
        {
            var list = new TodoList();
            var item = list.Add("a");
            Assert.True(list.Remove(item.Id));
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public void Find_ReturnsMatches()
        {
            var list = new TodoList();
            list.Add("Buy milk");
            list.Add("Read book");
            var found = list.Find("buy").ToList();
            Assert.Single(found);
            Assert.Equal("Buy milk", found[0].Title);
        }

        [Fact]
        public void SaveAndLoad_RestoresItems()
        {
            var list = new TodoList();
            list.Add("Task 1");
            list.Add("Task 2");

            string path = Path.Combine(Path.GetTempPath(), "todolist.json");

            list.Save(path);
            Assert.True(File.Exists(path));
            var loadedList = new TodoList();
            loadedList.Load(path);

            Assert.Equal(2, loadedList.Count);
            Assert.Contains(loadedList.Items, i => i.Title == "Task 1");
            Assert.Contains(loadedList.Items, i => i.Title == "Task 2");
            File.Delete(path);
        }

        [Fact]
        public void Load_NonExistingFile_Throws()
        {
            var list = new TodoList();
            string path = Path.Combine(Path.GetTempPath(), "nonexistent.json");

            Assert.Throws<FileNotFoundException>(() => list.Load(path));
        }
    }
}
