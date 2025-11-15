namespace IPCameraViewer.Services
{
    public class NoOpAudioService : IAudioService
    {
        public void PlaySound(string? filePath)
        {
            // No-op implementation for non-Windows platforms
        }
    }
}

