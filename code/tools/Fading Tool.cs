using System;

namespace Sandbox.Tools
{
	[Library( "fading_tool", Title = "Fading Tool (not working)", Description = "tools fades (not functional as of now)", Group = "construction" )]
	public partial class FadingTool : BaseTool
	{
		public override void Simulate()
		{
			if ( !Game.IsServer )
				return;

			using ( Prediction.Off() )
			{
				var startPos = Owner.EyePosition;
				var dir = Owner.EyeRotation.Forward;

				if ( Input.Pressed( "attack1" ) )
				{
					var tr = DoTrace();

					if ( !tr.Hit || !tr.Entity.IsValid() )
						return;

					if ( tr.Entity is Player )
						return;

					if ( tr.Entity is not ModelEntity modelEnt )
						return;

					modelEnt.Tags.Add( "nocollide" );
					modelEnt.Tags.Remove( "solid" );

					CreateHitEffects( tr.EndPosition, tr.Normal );
				}
				else if ( Input.Pressed( "attack2" ) )
				{
					var tr = DoTrace();

					if ( !tr.Hit || !tr.Entity.IsValid() )
						return;

					if ( tr.Entity is Player )
						return;

					if ( tr.Entity is not ModelEntity modelEnt )
						return;

					modelEnt.Tags.Add( "solid" );
					modelEnt.Tags.Remove( "nocollide" );

					CreateHitEffects( tr.EndPosition, tr.Normal );
				}
			}
		}
	}
}
