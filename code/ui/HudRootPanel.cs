using MinimalExample;
using Sandbox.UI;

namespace Sandbox.ui;

public class HudRootPanel : RootPanel
{
    public static HudRootPanel Current;

    public Scoreboard Scoreboard { get; set; }

    public HudRootPanel()
    {
        Current = this;

        AddChild<VoiceList>();
        SetTemplate("/ui/minimalhud.html");
        StyleSheet.Load("/ui/minimalhud.scss");
        AddChild<FoodMeter>();
        AddChild<RoundTime>();
      
        AddChild<HitIndicator>();
       
        Scoreboard =AddChild<Scoreboard>();
    }

    public override void Tick()
    {
        base.Tick();

       
    }

    protected override void UpdateScale(Rect screenSize)
    {
        base.UpdateScale(screenSize);
    }
}