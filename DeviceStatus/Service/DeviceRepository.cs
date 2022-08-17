namespace Service;

public class DeviceRepository : Dictionary<string, Device>
{
    public void HandleDeviceMessage(string name, string value)
    {
        if (ContainsKey(name))
        {
            this[name].Update(value);
        }
        else
        {
            var device = new Device(name, value);

            this[name] = device;
        }
    }
}