using MinimalExample;
using Sandbox;
using MinimalPlayer = Sandbox.MinimalPlayer;

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

			var ply = carrier as MinimalPlayer;
			PlaySound( "fard" );
		}
	}

    public override void Touch(Entity other)
    {

        if (other is not MinimalPlayer player)
            return;


		PlaySound("kritzglizzy");

	}
}
