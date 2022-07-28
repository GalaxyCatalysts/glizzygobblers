using Sandbox;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MinimalExample
{
	partial class MinimalPlayer : Player
	{
		[Net, Local]
		public double food { get; set; } = 50.0;
		[Net, Local]
		public double consumptionRate {get; set; } = 0.05;
		//double food = 50.0;

		private TimeSince TimeSinceGlizzSpawn;

		public bool IsAlive = true;

		public ClothingContainer Clothing = new();

		public MinimalPlayer() 
		{

		}
		
		private static Random s_Random = new Random();

		public MinimalPlayer(Client client) : this() 
		{
			Clothing.LoadFromClient(client);
		}

		public override void Respawn()
		{
			IsAlive = true;
			food = 50;
			
			SetModel( "models/citizen/citizen.vmdl" );

			//
			// Use WalkController for movement (you can make your own PlayerController for 100% control)
			//
			Controller = new WalkController();

			//
			// Use StandardPlayerAnimator  (you can make your own PlayerAnimator for 100% control)
			//
			Animator = new StandardPlayerAnimator();

			//
			// Use ThirdPersonCamera (you can make your own Camera for 100% control)
			//
			CameraMode = new FirstPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			Clothing.DressEntity(this);

			base.Respawn();
		}

		/// <summary>
		/// Called every tick, clientside and serverside.
		/// </summary>

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			//
			// If you have active children (like a weapon etc) you should call this to 
			// simulate those too.
			//
			SimulateActiveChild( cl, ActiveChild );

			//
			// If we're running serverside and Attack1 was just pressed, spawn a ragdoll
			//
			if (IsAlive == true) 
			{
				if ( IsServer && Input.Pressed( InputButton.PrimaryAttack ) )
				{
					//TODO: Add critical glizzy
					var ragdoll = new ModelEntity();
					ragdoll.Tags.Add( "glizzy" );
					TimeSinceGlizzSpawn = 0;
					ragdoll.SetModel( "models/glizzy.vmdl" );  
					ragdoll.Position = EyePosition + EyeRotation	.Forward * 40;
					ragdoll.Rotation = Rotation.LookAt( Vector3.Random.Normal );
					ragdoll.SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
					ragdoll.PhysicsGroup.Velocity = EyeRotation.Forward * 4000;
					Tags.Add("glizzy");

					if (TimeSinceGlizzSpawn >= 25.0f) 
					{
						ragdoll.Delete();
					}
					PlaySound( "throw" );
				}
				// else if (Input.Pressed( InputButton.PrimaryAttack ) && IsAlive == true){
				// 	PlaySound( "throw" ); // Why was this?? - Lokiv
				// }
			}
			

			if ( food <= 0 || food >= 100 )
			{
				if ( ConsoleCommands.GlizzyGod == 0 )
				{
					if (IsServer == false && IsAlive){ PlaySound( "death" ); }
					else if ( IsServer && IsAlive )
					{
						if (food >= 100){food = 100;}
					
					
						base.OnKilled();

						EnableDrawing = false;

						//food = 50;

						IsAlive = false;
						
						int perCent = s_Random.Next(0, 100);

						if ( perCent < 2 )
						{
							PlaySound( "death" );
						}

						if ( perCent == 1 )
						{
							PlaySound( "amongus" );
						}
					}
				}
				//PlaySound( "death" );
				

			}
			else
			{
				food = food - consumptionRate;
			}
		}


		public override void OnKilled()
		{
			base.OnKilled();

			EnableDrawing = false;
			
			IsAlive = false;
			
			food = 50;

		}

		public override void StartTouch( Entity other )
		{
			//Detects if entity is a glizzy
			if ( other.Tags.Has("glizzy")) 
			{
				//this is what happens when hit by a glizzy
				Log.Info( other.Position );
				if ( IsAlive ) { food = food + 5.0; }
				//base.Respawn();
				other.Delete();
				PlaySound( "slorp_2" );
			}
			if ( other.Tags.Has( "kritglizzy" ) )
			{
				PlaySound( "kritzglizzy" );
			}
		}

	}
}
