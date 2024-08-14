using DialogGameConsole.Text.Enums;
using DialogGameConsole.Texts;
using DialogGameConsole.UI;
using DialogGameConsole.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogGameConsole.Control;

public class DialogPanelController {
    private readonly List<(string, Character)> _history = new();

    private bool _isNPCTypingEnabled;
    private bool _isPCTypingEnabled;

    private long _typingNPCTimer;
    private long _typingPCTimer;

    private bool _isPCTyping;
    private bool _isNPCTyping;

    private RangeDictionary<bool> _typingProfileNPC = new();
    private RangeDictionary<bool> _typingProfilePC = new();

    private readonly DialogPanelUI _ui;


    public DialogPanelController(DialogPanelUI ui) {
        _ui = ui;
    }

    public void AddDialog(string text, Character character) {
        Assert.IsNotEmpty(text, nameof(text));
        Assert.IsNotDefault(character, nameof(character));
        _history.Add((text, character));

        if (character == Character.Npc)
            _ui.AddNpcDialog(text);
        else if (character == Character.Player)
            _ui.AddPlayerDialog(text);
    }

    public void SetTyping(Character character, IEnumerable<(long, bool)> profile) {
        Assert.IsNotDefault(character, nameof(character));
        if (character == Character.Npc) {
            _typingNPCTimer = 0;
            _isNPCTyping = false;
            _typingProfileNPC.AddRange(profile);
            _isNPCTypingEnabled = true;
        } else if (character == Character.Player) {
            _typingPCTimer = 0;
            _isPCTyping = false;
            _typingProfilePC.AddRange(profile);
            _isPCTypingEnabled = true;
        }
    }

    public void UpdateTime(long dt) {
        if(_isNPCTypingEnabled) {
            _typingNPCTimer += dt;
            var wasTyping = _isNPCTyping;
            _isNPCTyping = _typingProfileNPC.Get(_typingNPCTimer);
            if (_isNPCTyping && !wasTyping) {
                _ui.EnableNpcTyping();
            } else if (!_isNPCTyping && wasTyping) {
                _ui.DisableNpcTyping();
            }
        }
        if (_isPCTypingEnabled) {
            _typingPCTimer += dt;
            var wasTyping = _isPCTyping;
            _isPCTyping = _typingProfilePC.Get(_typingPCTimer);
            if (_isPCTyping && !wasTyping) {
                _ui.EnablePlayerTyping();
            }
            else if(!_isPCTyping && wasTyping) {
                _ui.DisablePlayerTyping();
            }
        }
    }


    internal void DisablePlayerTyping() {
        _isPCTypingEnabled = false;
        _isPCTyping = false;
        _typingPCTimer = 0;
        _typingProfilePC.Clear();
        _ui.DisablePlayerTyping();
    }
    internal void DisableNpcTyping() {
        _isNPCTypingEnabled = false;
        _isNPCTyping = false;
        _typingNPCTimer = 0;
        _typingProfileNPC.Clear();
        _ui.DisableNpcTyping();
    }

    public void Rewind() {
        _ui.Clear();
        _history.RemoveAt(_history.Count - 1);
        foreach (var (text, character) in _history.Skip(_history.Count - 30)) {
            if (character == Character.Npc)
                _ui.AddNpcDialog(text);
            else if (character == Character.Player)
                _ui.AddPlayerDialog(text);
        }
    }
}
