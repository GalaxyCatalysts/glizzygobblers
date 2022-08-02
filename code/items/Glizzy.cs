using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinimalExample;
using Sandbox;
using MinimalPlayer = Sandbox.MinimalPlayer;

partial class Glizzy : ModelEntity
{
   

    public override void Spawn()
    {
        base.Spawn();
        PhysicsEnabled = true;
        UsePhysicsCollision = true;
        SetModel("models/glizzy.vmdl");
        Tags.Add("glizzy");

    }

    public override void Touch(Entity other)
    {

        if (other is not MinimalPlayer player)
            return;

        if (player.IsAlive)
        {
            player.food += 5.0;
            Delete();
        }
       
            
        PlaySound("slorp_2");

    }

    public async Task DeleteGlizzy(float seconds)
    {
        await Task.DelaySeconds(seconds);

        Delete();
    }


}

	


