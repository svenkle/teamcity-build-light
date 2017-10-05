using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Svenkle.TeamCityBuildLight.Infrastructure.Light
{
    public class Light
    {
        private readonly uint _handle;
        private readonly int _updateInterval;
        private readonly Dictionary<Color, Globe> _globes;
        private CancellationToken _cancellationToken = new CancellationToken(false);

        public Light(uint device = 0, int updateInterval = 10, Dictionary<Color, Globe> globes = null)
        {
            var deviceName = new StringBuilder(512);
            if (GetDevice(2, device, deviceName) == 0)
                throw new Exception($"No device found at {device}");

            _globes = globes ?? new Dictionary<Color, Globe> { { Color.Green, new Globe() }, { Color.Blue, new Globe() }, { Color.Red, new Globe() } };
            _handle = OpenDevice(deviceName.ToString(), 0);
            _updateInterval = updateInterval;

            Reset(State.Off);

            Task.Factory.StartNew(Update, _cancellationToken);
        }

        public void Off(Color color)
        {
            if (_globes.TryGetValue(color, out var globe))
                globe.Off();
        }

        public void Off()
        {
            SetState(_handle, (int)Color.Red, (int)State.Off);
            SetState(_handle, (int)Color.Green, (int)State.Off);
            SetState(_handle, (int)Color.Blue, (int)State.Off);
        }

        public void On(Color color)
        {
            if (_globes.TryGetValue(color, out var globe))
                globe.On();
        }

        public void Solid(Color color)
        {
            if (_globes.TryGetValue(color, out var globe))
                globe.Solid();
        }

        public void Pulse(Color color)
        {
            if (_globes.TryGetValue(color, out var globe))
                globe.Pulse();
        }

        public void Flash(Color color)
        {
            if (_globes.TryGetValue(color, out var globe))
                globe.Flash();
        }

        private void Update()
        {
            while (true)
            {
                for (var i = 0; i < _globes.Count; i++)
                {
                    var globe = _globes.ElementAt(i);
                    globe.Value.Update();
                    SetPower(_handle, (int)globe.Key, globe.Value.Brightness);
                    SetState(_handle, (int)globe.Key, (int)globe.Value.State);
                }

                Thread.Sleep(_updateInterval);
            }
        }

        private void Reset(State state)
        {
            SetState(_handle, (int)Color.Red, (int)state);
            SetState(_handle, (int)Color.Green, (int)state);
            SetState(_handle, (int)Color.Blue, (int)state);
            SetPower(_handle, (int)Color.Red, 80);
            SetPower(_handle, (int)Color.Blue, 80);
            SetPower(_handle, (int)Color.Green, 80);
        }

        ~Light()
        {
            _cancellationToken = new CancellationToken(true);
            Reset(State.Off);
            CloseDevice(_handle);
        }

        [DllImport("delcomdll.dll", EntryPoint = "DelcomGetNthDevice")]
        private static extern int GetDevice(uint type, uint nthDevice, StringBuilder name);

        [DllImport("delcomdll.dll", EntryPoint = "DelcomOpenDevice")]
        private static extern uint OpenDevice(string name, int mode);

        [DllImport("delcomdll.dll", EntryPoint = "DelcomCloseDevice")]
        private static extern int CloseDevice(uint handle);

        [DllImport("delcomdll.dll", EntryPoint = "DelcomLEDControl")]
        private static extern int SetState(uint handle, int color, int state);

        [DllImport("delcomdll.dll", EntryPoint = "DelcomLEDPower")]
        public static extern int SetPower(uint handle, int color, int power);
    }
}