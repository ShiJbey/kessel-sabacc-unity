using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using KesselSabacc.Utils;
using System;

namespace KesselSabacc.Gameplay
{
	public class PresenterCommandSystem
	{
		private Queue<PresenterCommand> _commandQueue = new();
		private Awaitable _currentTask;

		public bool IsIdle => _currentTask == null && _commandQueue.Count == 0;

		public void Update()
		{
			if (_currentTask != null && _currentTask.IsCompleted)
			{
				_currentTask = null;
			}

			if (_currentTask == null && _commandQueue.Count > 0)
			{
				_currentTask = _commandQueue.Dequeue().Execute();
			}
		}

		public async Awaitable WaitUntilIdle()
		{
			while (!IsIdle)
			{
				await Awaitable.NextFrameAsync();
			}
		}

		public void QueueCommand(PresenterCommand command)
		{
			_commandQueue.Enqueue(command);
		}

		public static PresenterCommand Func(Func<Awaitable> commandFn)
		{
			return new FunctionPresenterCommand(commandFn);
		}

		public static PresenterCommand Parallel(params PresenterCommand[] commands)
		{
			return new ParallelPresenterCommand(commands);
		}

		public class FunctionPresenterCommand : PresenterCommand
		{
			private readonly Func<Awaitable> _func;

			public FunctionPresenterCommand(Func<Awaitable> func)
			{
				_func = func;
			}

			public override async Awaitable Execute()
			{
				await _func();
			}
		}

		public class ParallelPresenterCommand : PresenterCommand
		{
			private readonly PresenterCommand[] _commands;

			public ParallelPresenterCommand(params PresenterCommand[] commands)
			{
				_commands = commands;
			}

			public override async Awaitable Execute()
			{
				Task[] tasks = new Task[_commands.Length];
				for (int i = 0; i < _commands.Length; i++)
				{
					tasks[i] = _commands[i].Execute().AsTask();
				}
				await Task.WhenAll(tasks);
			}
		}
	}
}
