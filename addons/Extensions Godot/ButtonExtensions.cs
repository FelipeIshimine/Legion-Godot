using System.Threading;
using Godot;
using System.Threading.Tasks;
using Fractural.Tasks;

namespace Extensions.Godot;

public static class ButtonExtensions
{
    public static async GDTask<int> WaitForButtonUp(this Button[] buttons, CancellationToken token)
    {
        GDTask[] tasks = buttons.Collect(x => x.WaitForButtonUp(token));
        int result = await GDTask.WhenAny(tasks);
        if (token.IsCancellationRequested)
            return -1;
        return result;
    }
    
    public static async GDTask<int> WaitForButtonDown(this Button[] buttons, CancellationToken token)
    {
        GDTask[] tasks = buttons.Collect(x => x.WaitForButtonDown(token));
        int result = await GDTask.WhenAny(tasks);
        if (token.IsCancellationRequested)
            return -1;
        return result;
    }
    
    //public static GDTask WaitForButtonUp(this Button button) => WaitForButtonUp(button, CancellationToken.None);
    public static async GDTask WaitForButtonUp(this Button button, CancellationToken token)
    {
        TaskCompletionSource taskCompletionSource = new TaskCompletionSource();
        button.ButtonUp += taskCompletionSource.SetResult;
        await GDTask.WaitUntil(() => taskCompletionSource.Task.IsCompleted, cancellationToken: token);
        button.ButtonUp -= taskCompletionSource.SetResult;
    }
    
    //public static GDTask WaitForButtonDown(this Button button) => WaitForButtonUp(button, CancellationToken.None);
    public static async GDTask WaitForButtonDown(this Button button, CancellationToken token)
    {
        TaskCompletionSource taskCompletionSource = new TaskCompletionSource();
        button.ButtonDown += taskCompletionSource.SetResult;
        await GDTask.WaitUntil(() => taskCompletionSource.Task.IsCompleted, cancellationToken: token);
        button.ButtonDown -= taskCompletionSource.SetResult;
    }
    
    
}
