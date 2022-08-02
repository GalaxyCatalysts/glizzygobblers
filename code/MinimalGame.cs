using MinimalExample;


//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace Sandbox
{

	/// <summary>
	/// This is your game class. This is an entity that is created serverside when
	/// the game starts, and is replicated to the client. 
	/// 
	/// You can use this to create things like HUDs and declare which player class
	/// to use for spawned players.
	/// 
	/// Your game needs to be registered (using [Library] here) with the same name 
	/// as your game addon. If it isn't then we won't be able to find it.
	/// </summary>
	[Library( "glizzygobblers" )]
	partial class MinimalGame : Game
	{
        [Net]
        MinimalHud Hud { get; set; }
		public MinimalGame()
		{
			if ( IsServer )
			{
				// Log.Info( "My Gamemode Has Created Serverside!" );

				// Create a HUD entity. This entity is globally networked
				// and when it is created clientside it creates the actual
				// UI panels. You don't have to create your HUD via an entity,
				// this just feels like a nice neat way to do it.
                Hud = new MinimalHud();
                _ = GameLoopAsync();
			}

			if ( IsClient )
			{
				// Log.Info( "My Gamemode Has Created Clientside!" );
				//important debug message do not delete don't think about it // hehehehaw

			}
		}

		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			Log.Info("who the fuck loaded in");
			PlaySound( "glizzy" );
			// past here please don't change stuff (from isaiah) ===== FUCK YOU ISAIAH!!!!! I DO WHAT I WANNA!!! - Lokiv
			base.ClientJoined( client );

			var player = new MinimalPlayer(client);
			player.Respawn();

			client.Pawn = player;

		}

		public override void OnKilled(Client client, Entity pawn)
        {
            base.OnKilled(client, pawn);

           
        }

		//[Net]
		public int RoundTime = 120;
	}
}
