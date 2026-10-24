

// command interface 
public interface ICommand
{
    string Name { get; }
    void Execute();
}

public class CommandInvoker
{

   
    public void ExceuteCommand(ICommand command)
    {

        Console.WriteLine($"Executing command...{command.Name}");
        command.Execute();
        Console.WriteLine($"Execution completed for command...{command.Name}");


    }


}


public class PrintCommand: ICommand
{
    private string _message;

    public PrintCommand(string message)
    {
        _message = message;
    }

    public void Execute()
    {
       Console.WriteLine($"{_message}");
    }

}

public class SaveCommand: ICommand
{
    private string _fileName;
    public SaveCommand(string fileName)
    {
        _fileName = fileName;
    }
    public void Execute()
    {
        Console.WriteLine($"Saving file {_fileName}");
    }
}




// a class just to demo the power of this pattern , we queueedd the commands and then performed delayed execution 
public class PerformOperatrionsDemo
{

    public void PerformDemo(Lsit<ICommand> commands)
    {
        foreach (var command in commands)
        {
            command.Execute();
        }
}



public class Program
{
    public static void Main(string[] args)
    {
        CommandInvoker invoker = new CommandInvoker();
        ICommand printBeforeFileSave = new PrintCommand("Before saving file");
        ICommand saveCommand = new SaveCommand("file.txt");
        ICommand printAfterFileSave = new PrintCommand("After saving file");

       new PerformOperatrionsDemo().PerformDemo(new List<ICommand> { printBeforeFileSave, saveCommand, printAfterFileSave });




        }
}

