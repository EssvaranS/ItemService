namespace ItemService.Api
{
    /// <summary>
    /// Provides the UTC timestamp when the application started.
    /// </summary>
    public sealed class StartupTime
    {
        public DateTime UtcStarted { get; private set; } = DateTime.MinValue;

        public void MarkStarted() => UtcStarted = DateTime.UtcNow;
    }
}