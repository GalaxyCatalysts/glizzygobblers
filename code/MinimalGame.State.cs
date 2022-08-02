using System;
using System.Linq;
using System.Threading.Tasks;
using MinimalExample;
using Sandbox.items;

namespace Sandbox;

partial class MinimalGame : Game
{
    public static GameStates CurrentState => (Current as MinimalGame)?.GameState ?? GameStates.Live;

    [Net]
    public RealTimeUntil StateTimer { get; set; } = 0f;

    [Net] 
    public GameStates GameState { get; set; } = GameStates.Live;
    private int BigGlizzyTotal = 10;

    private async Task WaitStateTimer()
    {
        while (StateTimer > 0)
        {
            await Task.DelayRealtimeSeconds(1.0f);
        }

        // extra second for fun
        await Task.DelayRealtimeSeconds(1.0f);
    }
    private async Task GameLoopAsync()
    {
        Log.Info("Game Live");
        GameState = GameStates.Live;
        StateTimer = 1*60;
       
        FreshStart();
        await WaitStateTimer();

        Log.Info("Game End");
        GameState = GameStates.GameEnd;
        EndGame();
        StateTimer = 1 *10;
        await WaitStateTimer();

        await GameLoopAsync();

    }


    private void EndGame()
    {
        
        All.OfType<Glizzy>().ToList().ForEach(x =>
        {
            _ = x.DeleteGlizzy(1);
        });
        All.OfType<MinimalPlayer>().ToList().ForEach(x =>
        {
            x.CameraMode = new SpectateRagdollCamera();
            x.PlaySound("gameover").SetVolume(10);
        });
    }
    private void FreshStart()
    {
        
        //SpawnBigGlizzys();
        foreach (var cl in Client.All)
        {
            cl.SetInt("kills", 0);
            cl.SetInt("deaths", 0);
        }
      
        All.OfType<MinimalPlayer>().ToList().ForEach(x =>
        {
            x.Respawn();
        });
     
    }

   
    private void SpawnBigGlizzys()
    {

        for (int i = 0; i < BigGlizzyTotal; i++)
        
        {
            BigGlizzy bg = new BigGlizzy();
            bg.Position = new Vector3(new Random().Next(1, 10), 1, new Random().Next(1, 10));
            bg.Rotation = Rotation.LookAt(Vector3.Random.Normal);
        }
    }
    public enum GameStates
    {
        Live,
        GameEnd,
    }

}