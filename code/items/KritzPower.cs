using Sandbox;

[EditorModel( "models/kritzglizzy.vmdl" )]
partial class KritzPower : ItemBase
{
	public override Type ItemType => Type.Health;
	public virtual string WorldModelPath => "models/kritzglizzy.vmdl";

	public override string PickupSound => "glizzykrit";

	public override void Spawn()
	{
		base.Spawn();

		SetModel( WorldModelPath );

		Tags.Set( "kritglizzy", true );
	}

	public override void OnCarryStart( Entity carrier )
	{
		base.OnCarryStart( carrier );

		if ( PickupTrigger.IsValid() )
		{
			PickupTrigger.EnableTouch = false;

			var ply = carrier as MinimalExample.MinimalPlayer;
			PlaySound( "fard" );
		}
	}
}
