using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using MinimalExample;

namespace MinimalExample {
    public class RoundTime : Panel {

        public Label TimerLabel;
        public RoundTime()	
        {
            StyleSheet.Load("/ui/RoundTime.scss");
		    TimerLabel = Add.Label( "100", "timer" );
        }
        public override void Tick()
	    {
            var game = Game.Current as MinimalGame;
            TimerLabel.Text = $"{game.RoundTime}";
	    }
    }
}
