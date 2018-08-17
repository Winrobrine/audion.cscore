﻿using CSCore.CoreAudioAPI;
using System.Collections.Generic;

namespace Audion
{
    public class Device
    {
        public string DeviceId { get; set; }
        public string Name { get; set; }
        public DeviceType Type
        {
            get
            {
                if (ActualDevice != null)
                {
                    if (ActualDevice.DataFlow == DataFlow.Render)
                        return DeviceType.LoopbackCapture;
                }

                return DeviceType.Capture;
            }
        }
        internal MMDevice ActualDevice { get; set; }

        public static Device GetDefaultDevice()
        {
            using (var mmdeviceEnumerator = new MMDeviceEnumerator())
            {
                try
                {
                    var device = mmdeviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                    return new Device
                    {
                        DeviceId = device.DeviceID,
                        Name = device.FriendlyName,
                        ActualDevice = device
                    };
                }
                catch (CoreAudioAPIException)
                {
                    return null;
                }
            }
        }

        public static Device GetDefaultRecordingDevice()
        {
            using (var mmdeviceEnumerator = new MMDeviceEnumerator())
            {
                using (var mmdeviceCollection = mmdeviceEnumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active))
                {
                    foreach (var device in mmdeviceCollection)
                    {
                        return new Device
                        {
                            DeviceId = device.DeviceID,
                            Name = device.FriendlyName,
                            ActualDevice = device
                        };
                    }
                }

                return null;
            }
        }

        public static IList<Device> GetDevices()
        {
            var devices = new List<Device>();

            using (var mmdeviceEnumerator = new MMDeviceEnumerator())
            {
                using (var mmdeviceCollection = mmdeviceEnumerator.EnumAudioEndpoints(DataFlow.All, DeviceState.Active))
                {
                    foreach (var device in mmdeviceCollection)
                    {
                        devices.Add(new Device
                        {
                            DeviceId = device.DeviceID,
                            Name = device.FriendlyName,
                            ActualDevice = device
                        });
                    }
                }
            }

            return devices;
        }

        public static IList<Device> GetInputDevices()
        {
            var devices = new List<Device>();

            using (var mmdeviceEnumerator = new MMDeviceEnumerator())
            {
                using (var mmdeviceCollection = mmdeviceEnumerator.EnumAudioEndpoints(DataFlow.Capture, DeviceState.Active))
                {
                    foreach (var device in mmdeviceCollection)
                    {
                        devices.Add(new Device
                        {
                            DeviceId = device.DeviceID,
                            Name = device.FriendlyName,
                            ActualDevice = device
                        });
                    }
                }
            }

            return devices;
        }

        public static IList<Device> GetOutputDevices()
        {
            var devices = new List<Device>();

            using (var mmdeviceEnumerator = new MMDeviceEnumerator())
            {
                using (var mmdeviceCollection = mmdeviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
                {
                    foreach (var device in mmdeviceCollection)
                    {
                        devices.Add(new Device
                        {
                            DeviceId = device.DeviceID,
                            Name = device.FriendlyName,
                            ActualDevice = device
                        });
                    }
                }
            }

            return devices;
        }
    }
}
