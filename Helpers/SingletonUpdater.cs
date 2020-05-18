using NK.yaSingleton.Utility;

namespace NK.yaSingleton.Helpers
{
    /// <summary>
    ///     Singleton updater class. Instantiates a single MonoBehaviour and uses it to send Unity's events to all singletons.
    ///     Has a sexy editor.
    /// </summary>
    public class SingletonUpdater : ExecutorBehavior
	{
		private static SingletonUpdater updaterInternal;

		internal static SingletonUpdater updater
		{
			get {
				if (updaterInternal == null) {
					updaterInternal = Create<SingletonUpdater>(nameof(SingletonUpdater), true);
				}

				return updaterInternal;
			}
		}
	}
}