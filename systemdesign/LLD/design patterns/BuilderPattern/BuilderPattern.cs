

// concrete class of the builder pattern : for which class the builder pattern we are creating 
public class CustomPC
{
       public string CPU { get; set; }
    public string GPU { get; set; }
    public int RAM { get; set; }
    public int Storage { get; set; }
    public bool IsWaterCooled { get; set; }
    public override string PCDescription()
    {
        return $"CPU: {CPU}, GPU: {GPU}, RAM: {RAM}GB, Storage: {Storage}GB, Water Cooled: {IsWaterCooled}";
    }
}

// builder interface for the concrete class CustomPC
public interface  ICustomPCBuilder
{
    public ICustomPCBuilder SetCPU(string cpu);
    public ICustomPCBuilder SetGPU( string gpu);
    public ICustomPCBuilder SetRAM(int ram);
    public ICustomPCBuilder SetStorage(int storage);
    public ICustomPCBuilder SetIsWaterColled(bool isWaterCooled);
    public CustomPC Build();

}


// concrete builder class that implements the builder interface
public class CustomPCBuilder : ICustomPCBuilder
{
    private CustomPC _customPC = new CustomPC();
    public ICustomPCBuilder SetCPU(string cpu)
    {
        _customPC.CPU = cpu;
        return this;
    }
    public ICustomPCBuilder SetGPU(string gpu)
    {
        _customPC.GPU = gpu;
        return this;
    }
    public ICustomPCBuilder SetRAM(int ram)
    {
        _customPC.RAM = ram;
        return this;
    }
    public ICustomPCBuilder SetStorage(int storage)
    {
        _customPC.Storage = storage;
        return this;
    }
    public ICustomPCBuilder SetIsWaterColled(bool isWaterCooled)
    {
        _customPC.IsWaterCooled = isWaterCooled;
        return this;
    }
    public CustomPC Build()
    {
        return _customPC;
    }
}


// usage of the builder pattern
public class Program
{
    public static void main(string[] args)
    {
        var CustomPC= new CustomPCBuilder()
            .SetCPU("Intel i9")
            .SetGPU("NVIDIA RTX 3090")
            .SetRAM(32)
            .SetStorage(2000)
            .SetIsWaterColled(true)
            .Build();

        Console.WriteLine(CustomPC.PCDescription());
    }
}