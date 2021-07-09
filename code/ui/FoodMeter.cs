using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
//using MinimalPlayer;

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
            var player = Local.Pawn;
            if ( player == null ) return;

            //Label.Text = $"{player.food.CeilToInt()}";
            Label.Text = "sex";
	    }
    }
}