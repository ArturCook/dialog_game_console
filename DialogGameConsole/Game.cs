using DialogGameConsole.Control;
using DialogGameConsole.UI;
using System;

namespace DialogGameConsole;

public class Game
{
    public const int FPS = 5;

    public const int Delay = (int)(1000.0 / FPS);

    public MainUI MainUI;

    public TimerController TimerController;

    public DialogPanelController DialogPanelController;

    public OptionsController OptionsController;

    public CommandsController CommandsController;

    public DebugController DebugController;

    public DialogTraversalController DialogTraversalController;

    public Game()
    {
        // Tested with a 70x50 Windows Console
        // Haven't added support for other resolutions at the moment

        MainUI = new MainUI();
        var timerUI = new TimerUI(MainUI.GetInternalWidth());
        var dialogPanelUI = new DialogPanelUI(MainUI.GetInternalWidth(), 36);
        var optionsUI = new OptionsUI(MainUI.GetInternalWidth());
        var debugUI = new DebugUI(MainUI.GetInternalWidth());

        MainUI.AddElement(timerUI);
        MainUI.AddElement(dialogPanelUI);
        MainUI.AddElement(optionsUI);
        MainUI.AddElement(debugUI);

        // Controllers

        TimerController = new TimerController(timerUI);
        DialogPanelController = new DialogPanelController(dialogPanelUI);
        OptionsController = new OptionsController(optionsUI);
        CommandsController = new CommandsController(OptionsController);
        DebugController = new DebugController(debugUI, IsDebugMode);
        DialogTraversalController = new DialogTraversalController(TimerController, DialogPanelController, 
            OptionsController, DebugController);
    }

    public static bool IsDebugMode = false;

    public bool Finished = false;

    public bool Restarting = false;

    public bool Resize = false;

    public bool UIActive = true;

    private int[] GameSpeeds = new[] { 1, 2, 3, 5, 10, 20 };

    private int GameSpeedIndex = 0;

    public int GameSpeed => GameSpeeds[GameSpeedIndex];

    public readonly Random Random = new(1);

    public static readonly Random UIRandom = new(1);

    public double GlobalTime;

    public void ToggleDebug()
    {
        IsDebugMode = !IsDebugMode;
        DebugController.Set(IsDebugMode);
        Resize = true;
    }

    public void Finish()
    {
        Finished = true;
    }

    public void Restart()
    {
        Restarting = false;
    }

    public void UpdateSpeed()
    {
        GameSpeedIndex += 1;
        GameSpeedIndex %= GameSpeeds.Length;
    }

    public void MaxSpeed()
    {
        var maxIndex = GameSpeeds.Length - 1;
        if (GameSpeedIndex == maxIndex)
            GameSpeedIndex = 0;
        else
            GameSpeedIndex = maxIndex;
    }

    public void Rewind()
    {
        DialogTraversalController.Rewind();
    }

    public void UpdateGlobalTime(double dt)
    {
        GlobalTime += dt;
        var scaledDt = (long)(dt * GameSpeed);
        CommandsController.ExcuteCurrentCommand(this);

        TimerController.UpdateTime(scaledDt);
        OptionsController.UpdateTime(scaledDt);
        DialogTraversalController.UpdateTime(scaledDt);
        DialogPanelController.UpdateTime(scaledDt);

        MainUI.UpdateTime(scaledDt);
    }
}
