using System.ComponentModel;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;

public static partial class MoneySystem
{

    public static int DefaultMoney = 2500; // Edit this to change players starting money upon joining the server
    public static int MoneyAmount = 0 + DefaultMoney;
    public static int Salary;
    public static void Money()
    {
        
    }

    public async static void Startup()
    {
        await Task.Delay(200);
        MoneyAmount = MoneyAmount + Salary;
        Log.Error(Salary);
        await Task.Delay(100);
        Startup();
    }
    
}