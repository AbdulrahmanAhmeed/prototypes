#if WINDOWS
using IPCameraViewer.Services;
using System.Runtime.InteropServices;

namespace IPCameraViewer.Platforms.Windows
{
    public class AudioService : IAudioService
    {
        private const string WinmmDll = "winmm.dll";
        private const string PlaySoundWEntryPoint = "PlaySoundW";
        private const string DebugFilePathNull = "AudioService: filePath is null or empty";
        private const string DebugFileNotExists = "AudioService: File does not exist: {0}";
        private const string DebugCouldNotGetFullPath = "AudioService: Could not get full path";
        private const string DebugAttemptingToPlay = "AudioService: Attempting to play sound: {0}";
        private const string DebugPlaySoundReturned = "AudioService: PlaySoundWin32 returned: {0} for path: {1}";
        private const string DebugPlaySoundFailed = "AudioService: PlaySoundWin32 failed with error code: {0}";
        private const string DebugDllNotFoundException = "AudioService: DllNotFoundException - {0}";
        private const string DebugEntryPointNotFoundException = "AudioService: EntryPointNotFoundException - {0}";
        private const string DebugBadImageFormatException = "AudioService: BadImageFormatException - {0}";
        private const string DebugAccessViolationException = "AudioService: AccessViolationException - {0}";
        private const string DebugExceptionFormat = "AudioService: Exception - {0}: {1}";

        [DllImport(AudioService.WinmmDll, EntryPoint = AudioService.PlaySoundWEntryPoint, CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PlaySoundWin32([MarshalAs(UnmanagedType.LPWStr)] string? pszSound, IntPtr hmod, uint fdwSound);

        private const uint SND_FILENAME = 0x00020000;
        private const uint SND_ASYNC = 0x0001;
        private const uint SND_NODEFAULT = 0x0002;

        public void PlaySound(string? filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                try
                {
                    if (File.Exists(filePath))
                    {
                        // Convert to full path to avoid issues
                        string fullPath = Path.GetFullPath(filePath);
                        if (!string.IsNullOrEmpty(fullPath))
                        {
                            System.Diagnostics.Debug.WriteLine(string.Format(AudioService.DebugAttemptingToPlay, fullPath));

                            // Ensure path uses backslashes and is properly formatted for Windows API
                            string normalizedPath = fullPath.Replace('/', '\\');
                            
                            // Call PlaySound with SND_FILENAME | SND_ASYNC
                            // SND_ASYNC plays asynchronously without blocking
                            uint flags = AudioService.SND_FILENAME | AudioService.SND_ASYNC;
                            bool result = AudioService.PlaySoundWin32(normalizedPath, IntPtr.Zero, flags);
                            System.Diagnostics.Debug.WriteLine(string.Format(AudioService.DebugPlaySoundReturned, result, normalizedPath));
                            
                            if (!result)
                            {
                                // Get last error if available
                                int error = Marshal.GetLastWin32Error();
                                System.Diagnostics.Debug.WriteLine(string.Format(AudioService.DebugPlaySoundFailed, error));
                            }
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine(AudioService.DebugCouldNotGetFullPath);
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format(AudioService.DebugFileNotExists, filePath));
                    }
                }
                catch (DllNotFoundException ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format(AudioService.DebugDllNotFoundException, ex.Message));
                }
                catch (EntryPointNotFoundException ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format(AudioService.DebugEntryPointNotFoundException, ex.Message));
                }
                catch (BadImageFormatException ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format(AudioService.DebugBadImageFormatException, ex.Message));
                }
                catch (AccessViolationException ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format(AudioService.DebugAccessViolationException, ex.Message));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format(AudioService.DebugExceptionFormat, ex.GetType().Name, ex.Message));
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(AudioService.DebugFilePathNull);
            }
        }
    }
}
#endif

