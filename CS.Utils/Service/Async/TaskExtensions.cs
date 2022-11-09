using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ArsuLeo.CS.Utils.Service.Async
{
    public static class TaskExtensions
    {
        public static ConfiguredTaskAwaitable CAF(this Task task) => task.ConfigureAwait(false);
        public static ConfiguredTaskAwaitable<T> CAF<T>(this Task<T> task) => task.ConfigureAwait(false);
    }
}
