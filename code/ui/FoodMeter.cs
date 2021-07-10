using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using MinimalExample;

namespace MinimalExample {
    public class FoodMeter : Panel {

        public Label Label;
        public FoodMeter()	
        {
            StyleSheet.Load("/ui/FoodMeter.scss");
            Add.Label( "🌭", "icon" );
		    Label = Add.Label( "100", "value" );
        }

        public override void Tick()
	    {
            var player = Local.Pawn as MinimalPlayer;
            if ( player == null ) return;
            Label.Text = $"{player.food = (int) player.food}";
	    }
    }
}
