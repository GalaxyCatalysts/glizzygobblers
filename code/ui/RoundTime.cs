using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using MinimalExample;

namespace MinimalExample {
    public class RoundTime : Panel {

        public Label Laabel;
        public RoundTime()	
        {
            StyleSheet.Load("/ui/RoundTime.scss");
		    Laabel = Add.Label( "100", "value" );
        }

        public override void Tick()
	    {
            Laabel.Text = ( "ass" );
	    }
    }
}
