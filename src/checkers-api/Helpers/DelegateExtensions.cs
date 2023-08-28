namespace checkers_api.Helpers;

public static class DelegateExtensions
{
    public static Task InvokeAsync<TArgs>(this Func<object, TArgs, Task> func, object sender, TArgs e)
    {
        return func == null ? Task.CompletedTask : Task.WhenAll(func.GetInvocationList().Cast<Func<object, TArgs, Task>>().Select(f => f(sender, e)));
    }
}