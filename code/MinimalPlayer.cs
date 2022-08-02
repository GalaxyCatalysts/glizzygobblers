using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using MinimalExample;

namespace Sandbox
{
	public partial class MinimalPlayer : Player
	{
		[Net, Local]
		public double food { get; set; } = 50.0;
		[Net, Local]
		public double consumptionRate {get; set; } = 0.05;
        //double food = 50.0;
        static EntityLimit RagdollLimit = new EntityLimit { MaxTotal = 2};
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
			if (IsAlive) 
			{
				if (Input.Pressed( InputButton.PrimaryAttack))
				{
                    var glizzy = new Glizzy
                    {
                        Position = EyePosition + EyeRotation.Forward * 40,
                        Rotation = Rotation.LookAt(Vector3.Random.Normal)
                    };
                    
                    glizzy.SetupPhysicsFromModel(PhysicsMotionType.Dynamic, false);
                    glizzy.PhysicsGroup.Velocity = EyeRotation.Forward * 4000;
                    glizzy.Spawn();
					
					foreach (var tr in TraceBullet(glizzy.Position, glizzy.Position + EyeRotation.Forward * 5000, 1))
                    {
                        tr.Surface.DoBulletImpact(tr);

                        if (!IsServer) continue;
                        if (!tr.Entity.IsValid()) continue;

                        //
                        // We turn predictiuon off for this, so any exploding effects don't get culled etc
                        //
                        using (Prediction.Off())
                        {
							
                            var damageInfo = DamageInfo.FromBullet(tr.EndPosition, EyeRotation.Forward * 100 * 10, 0)
                                .UsingTraceResult(tr)
                                .WithAttacker(Owner)
                                .WithWeapon(this);
                            tr.Entity.TakeDamage(damageInfo);

                        }
                    }


                    _ = glizzy.DeleteGlizzy(25);
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
					
					
						OnKilled();

						EnableDrawing = false;
                        PhysicsEnabled = false;
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

       
		public  IEnumerable<TraceResult> TraceBullet(Vector3 start, Vector3 end, float radius = 2.0f)
        {
            bool underWater = Trace.TestPoint(start, "water");

            var trace = Trace.Ray(start, end)
                .UseHitboxes()
                .WithAnyTags("solid", "player", "npc", "glass")
                .Ignore(this)
                .Size(radius);

            //
            // If we're not underwater then we can hit water
            //
            if (!underWater)
                trace = trace.WithAnyTags("water");

            var tr = trace.Run();

            if (tr.Hit)
                yield return tr;

            //
            // Another trace, bullet going through thin material, penetrating water surface?
            //
        }
		public override void OnKilled()
		{
			base.OnKilled();
            EnableDrawing = false;
           
            IsAlive = false;
			food = 50;
            
            if (Root.Client.Pawn.LastAttacker != null)
            {
                var attacker = All.OfType<MinimalPlayer>().FirstOrDefault(x => x.Client.Name == Client.Pawn.LastAttacker.Name);
                attacker?.Client.AddInt("kills");


            }
            BecomeRagdollOnClient(10, 1);
            CameraMode = new SpectateRagdollCamera();

        }

        //ragdoller
        [ClientRpc]
        void BecomeRagdollOnClient(Vector3 force, int forceBone)
        {
            // TODO - lets not make everyone write this shit out all the time
            // maybe a CreateRagdoll<T>() on ModelEntity?
            var ent = new ModelEntity();
            ent.Position = Position;
            ent.Rotation = Rotation;
            ent.PhysicsEnabled = true;
            ent.UsePhysicsCollision = true;

            // I like being able to kick around the ragdolls clientside :)
            //Tags.Add( "gib" );

            ent.CopyFrom(this);
            ent.CopyBonesFrom(this);
            ent.SetRagdollVelocityFrom(this);
            ent.DeleteAsync(10.0f);

            // Copy the clothes over
            foreach (var child in Children)
            {
                if (!child.Tags.Has("clothes"))
                    continue;

                if (child is ModelEntity e)
                {
                    var clothing = new ModelEntity();
                    clothing.CopyFrom(e);
                    clothing.SetParent(ent, true);
                }
            }

            ent.PhysicsGroup.AddVelocity(force);

            if (forceBone >= 0)
            {
                var body = ent.GetBonePhysicsBody(forceBone);
                if (body != null)
                {
                    body.ApplyForce(force * 1000);
                }
                else
                {
                    ent.PhysicsGroup.AddVelocity(force);
                }
            }


            Corpse = ent;
            RagdollLimit.Watch(ent);
            
        }

        //hitmarkers
        public override void TakeDamage(DamageInfo info)
        {


            LastAttacker = info.Attacker;
            LastAttackerWeapon = info.Weapon;
            if (info.Attacker is MinimalPlayer attacker)
            {
                info.Damage *= .1f;
                if (attacker != this)
                {
                    attacker.DidDamage(To.Single(attacker), info.Position, info.Damage, Health.LerpInverse(100, 0));
                }

            }
        }


        [ClientRpc]
        public void DidDamage(Vector3 pos, float amount, float healthinv)
        {


            HitIndicator.Current?.OnHit(pos, amount);
        }


    }
}
