using DialogGameConsole.Dialogs.PetDialog;
using Spectre.Console;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DialogGameConsole;

class Program
{
	static void Main(string[] args)
	{
		RunProgram();
	}

	static void RunProgram()
	{
		var game = new Game();
		
		Task taskUi = null;
		var isUiActive = false;

		void CreateUITask()
        {
			if(isUiActive)
            {
				isUiActive = false;
				taskUi?.Wait();
			}

			var width = game.MainUI.GetWidth();
			var height = game.MainUI.GetHeight();

			Console.OutputEncoding = System.Text.Encoding.Unicode;
			Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

			//Again to remove scroll bar width
			Console.SetWindowSize(width, height);

			isUiActive = true;

			taskUi = AnsiConsole.Live(game.MainUI)
				.AutoClear(false)
				.Overflow(VerticalOverflow.Crop)
				.Cropping(VerticalOverflowCropping.Bottom)
				.StartAsync(async ctx =>
				{
					while (isUiActive)
					{
						try
						{
							if (game.MainUI.NeedRefresh())
								ctx.Refresh();
						}
						catch (Exception e)
						{
							var xx = 1;
						}

						await Task.Delay(Game.Delay);
					}
				});
		}
		
		CreateUITask();

		var keyReaderTask = Task.Factory.StartNew(() =>
		{
			while (!game.Finished)
                {
				var key = Console.ReadKey(true);
				game.CommandsController.HandleKeyPress(key.Key);
			}
		});


		game.DialogTraversalController.AssignDialog(PetDialogBuilder.New());

		var lastEndTime = DateTime.UtcNow;
		while (!game.Finished)
		{
			var startTime = DateTime.UtcNow;
			var dt = (int)((startTime - lastEndTime).TotalMilliseconds);

			if (game.Resize)
            {
				CreateUITask();
				game.Resize = false;
			}
			game.UpdateGlobalTime(dt);

			lastEndTime = DateTime.UtcNow;
			var loopTime = (int)((lastEndTime - startTime).TotalMilliseconds);
			var delay = Math.Max(10, Game.Delay - loopTime);
			Thread.Sleep(delay);
		}

		keyReaderTask.Wait();
		taskUi?.Wait();
	}
}

