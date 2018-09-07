using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace TagTool.HyperSerialization
{
	public class TaskBag : Task
	{
		public TaskBag(Action action) : base(action) { }

		public ConcurrentBag<TaskBag> ChildTasks = new ConcurrentBag<TaskBag>();
		public void WaitForChildren()
		{
			Task.WaitAll(ChildTasks.ToArray());
			foreach (var task in ChildTasks)
				task.WaitForChildren();
		}
	}
}
