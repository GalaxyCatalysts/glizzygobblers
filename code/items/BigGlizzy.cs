using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinimalExample;

namespace Sandbox.items
{
    partial class BigGlizzy : Glizzy
    {
        public override void Spawn()
        {
            base.Spawn();

            SetModel("models/glizzy.vmdl");
            Tags.Add("glizzy");
            Root.Scale = 15;
            
            PhysicsEnabled = true;
            UsePhysicsCollision = true;
            

            _ = DeleteGlizzy(25f);
        }

        public override void Touch(Entity other)
        {

            if (other is not MinimalPlayer player)
                return;

            if (player.IsAlive)
            {
                player.food += 15.0;
                PlaySound("bigglizzy").SetVolume(15);
                Delete();
            }


        }

    }
}
