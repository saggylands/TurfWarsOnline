using System.ComponentModel;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Timers;

public enum jobType {
citizen = 0,
thief = 1,
gundealer = 2,
hobo = 3,
hitman = 4,
ganglead = 5,
police = 6,

}

public partial class JobUI : Panel
{
	public Label Label;

	public static string Job = "Pick Job";
	public JobUI()
	{
		Label = Add.Label( "Job", "value" );

	}
	
	public override void Tick()
	{
		var player = Game.LocalPawn;
		if ( player == null ) return;

		Label.Text = $"Job: {Job} | ${MoneySystem.MoneyAmount}";


	}
	[ConCmd.Client( "myjob" )]
	public static void MyJob()
	{
		Log.Info( $"Hello I am {Job}" );
	}
	[ClientRpc]
	public static void SendJob(jobType recieveJob)
	{
			Log.Info(recieveJob.ToString());

	}
	[ConCmd.Server( "ServerSetJob" )]
	public static void ServerSetJob(jobType serverJob)
	{
		SendJob(To.Single(ConsoleSystem.Caller), serverJob);
		Log.Error($"{ConsoleSystem.Caller} has switched job to " + serverJob.ToString());
		
	}
}

