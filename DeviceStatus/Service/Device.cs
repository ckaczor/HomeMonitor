namespace Service;

public class Device
{
    public string Name { get; }
    public bool Status { get; set; }

    public Device(string name, string statusString)
    {
        Name = name;
        Update(statusString);
    }

    public void Update(string statusString)
    {
        Status = statusString == "1";
    }
}