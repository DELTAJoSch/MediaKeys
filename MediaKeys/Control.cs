using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MediaKeys
{
    public class Control
    {
        // Import of IDeviceEnumerator Interface
        [ComImport]
        [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IMMDeviceEnumerator
        {
            void _VtblGap1_1();
            int GetDefaultAudioEndpoint(int dataFlow, int role, out IMMDevice ppDevice);
        }
        private static class MMDeviceEnumeratorFactory
        {
            public static IMMDeviceEnumerator CreateInstance()
            {
                return (IMMDeviceEnumerator)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("BCDE0395-E52F-467C-8E3D-C4579291692E"))); // a MMDeviceEnumerator
            }
        }

        // Import of IMMDevice Interface
        [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IMMDevice
        {
            int Activate([MarshalAs(UnmanagedType.LPStruct)] Guid iid, int dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);
        }

        // Import of IAudioEndpointVolume Interface
        [Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IAudioEndpointVolume
        {
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE RegisterControlChangeNotify(/* [in] */__in IAudioEndpointVolumeCallback *pNotify) = 0;
            int RegisterControlChangeNotify(IntPtr pNotify);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE UnregisterControlChangeNotify(/* [in] */ __in IAudioEndpointVolumeCallback *pNotify) = 0;
            int UnregisterControlChangeNotify(IntPtr pNotify);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetChannelCount(/* [out] */ __out UINT *pnChannelCount) = 0;
            int GetChannelCount(ref uint pnChannelCount);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetMasterVolumeLevel( /* [in] */ __in float fLevelDB,/* [unique][in] */ LPCGUID pguidEventContext) = 0;
            int SetMasterVolumeLevel(float fLevelDB, Guid pguidEventContext);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE SetMasterVolumeLevelScalar( /* [in] */ __in float fLevel,/* [unique][in] */ LPCGUID pguidEventContext) = 0;
            int SetMasterVolumeLevelScalar(float fLevel, Guid pguidEventContext);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetMasterVolumeLevel(/* [out] */ __out float *pfLevelDB) = 0;
            int GetMasterVolumeLevel(ref float pfLevelDB);
            //virtual /* [helpstring] */ HRESULT STDMETHODCALLTYPE GetMasterVolumeLevelScalar( /* [out] */ __out float *pfLevel) = 0;
            int GetMasterVolumeLevelScalar(ref float pfLevel);
        }

        /// <summary>
        /// Tries to send a Play/Pause command
        /// </summary>
        /// <returns>returns true if the keypress was sent, false if there was an error</returns>
        public bool PlayPause()
        {
            try
            {
                KeyboardInteraction.SendKey(KeyboardInteraction.KeyCode.MEDIA_PLAY_PAUSE);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to send a Skip command
        /// </summary>
        /// <returns>returns true if the keypress was sent, false if there was an error</returns>
        public bool Skip()
        {
            try
            {
                KeyboardInteraction.SendKey(KeyboardInteraction.KeyCode.MEDIA_NEXT_TRACK);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to send a Skip Back command
        /// </summary>
        /// <returns>returns true if the keypress was sent, false if there was an error</returns>
        public bool SkipBack()
        {
            try
            {
                KeyboardInteraction.SendKey(KeyboardInteraction.KeyCode.MEDIA_PREV_TRACK);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to send a stop command
        /// </summary>
        /// <returns>returns true if the keypress was sent, false if there was an error</returns>
        public bool Stop()
        {
            try
            {
                KeyboardInteraction.SendKey(KeyboardInteraction.KeyCode.MEDIA_STOP);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to increase the volume
        /// </summary>
        /// <returns>returns true if the keypress was sent, false if there was an error</returns>
        public bool IncreaseVolume()
        {
            try
            {
                KeyboardInteraction.SendKey(KeyboardInteraction.KeyCode.VOLUME_UP);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to decrease the volume
        /// </summary>
        /// <returns>returns true if the keypress was sent, false if there was an error</returns>
        public bool DecreaseVolume()
        {
            try
            {
                KeyboardInteraction.SendKey(KeyboardInteraction.KeyCode.VOLUME_DOWN);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to send a mute command
        /// </summary>
        /// <returns>returns true if the keypress was sent, false if there was an error</returns>
        public bool Mute()
        {
            try
            {
                KeyboardInteraction.SendKey(KeyboardInteraction.KeyCode.VOLUME_MUTE);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to set the volume to a specified level
        /// </summary>
        /// <returns>returns true if the keypress was sent, false if there was an error</returns>
        public bool SetVolume(int Volume)
        {
            try
            {
                IMMDeviceEnumerator enumerator = MMDeviceEnumeratorFactory.CreateInstance();
                IMMDevice device;

                int eRender = 0;
                int eMultimedia = 1;

                enumerator.GetDefaultAudioEndpoint(eRender, eMultimedia, out device);

                object endpoint = null;
                device.Activate(typeof(IAudioEndpointVolume).GUID, 0, IntPtr.Zero, out endpoint);

                IAudioEndpointVolume audio = (IAudioEndpointVolume)endpoint;

                if (audio.SetMasterVolumeLevelScalar(Volume / 100f, new Guid()) != 0)
                    throw new Exception();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to get the currently set volume
        /// </summary>
        /// <returns>either the current audio volume or null if the level could not be read</returns>
        public Nullable<int> GetCurrentVolume()
        {
            try
            {
                float level = 0;

                IMMDeviceEnumerator enumerator =  MMDeviceEnumeratorFactory.CreateInstance();
                IMMDevice device;

                int eRender = 0;
                int eMultimedia = 1;

                enumerator.GetDefaultAudioEndpoint(eRender, eMultimedia, out device);

                object endpoint = null;
                device.Activate(typeof(IAudioEndpointVolume).GUID, 0, IntPtr.Zero, out endpoint);
                
                IAudioEndpointVolume audio = (IAudioEndpointVolume)endpoint;

                audio.GetMasterVolumeLevelScalar(ref level);

                return (int)(level * 100);
            }
            catch
            {
                return null;
            }
        }
    }
}
