using System;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using MinimalExample;

namespace MinimalExample {
    internal class RoundTime : Panel {

        public Label TimerLabel;
        public RoundTime()	
        {
            StyleSheet.Load("/ui/RoundTime.scss");
		    TimerLabel = Add.Label(string.Empty, "game-timer");
        }
        public override void Tick()
	    {
            base.Tick();
            var game = Game.Current as MinimalGame;
           

            if (!game.IsValid()) return;

            var span = TimeSpan.FromSeconds((game.StateTimer* 60).Clamp(0, float.MaxValue));

            TimerLabel.Text = span.ToString(@"hh\:mm");
        }
    }
}
