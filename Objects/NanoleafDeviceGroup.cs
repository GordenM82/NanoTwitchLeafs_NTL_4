using System.Collections.Generic;

namespace NanoTwitchLeafs.Objects
{
    public class NanoleafDeviceGroup
    {
        public string Name { get; set; }
        public List<string> DeviceNames { get; set; } = new List<string>();

        public override string ToString()
        {
            return Name;
        }
    }
}
