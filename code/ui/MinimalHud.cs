using MinimalExample;
using Sandbox;
using Sandbox.ui;
using Sandbox.UI;
using MinimalPlayer = Sandbox.MinimalPlayer;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
/// <summary>
	/// This is the HUD entity. It creates a RootPanel clientside, which can be accessed
	/// via RootPanel on this entity, or Local.Hud.
	/// </summary>
	public partial class MinimalHud : HudEntity<HudRootPanel>
    {
        [ClientRpc]
        public void OnPlayerDied(MinimalPlayer player)
        {
            Host.AssertClient();
        }

        [ClientRpc]
        public void ShowDeathScreen(string attackerName)
        {
            Host.AssertClient();
        }
    }


