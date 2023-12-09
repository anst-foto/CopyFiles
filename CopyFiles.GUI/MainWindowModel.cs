using CopyFiles.Lib;

namespace CopyFiles.GUI;

public class MainWindowModel : BaseWindowModel
{
    private string? _from;
    public string? From
    {
        get => _from;
        set => SetField(ref _from, value);
    }
    
    private string? _to;
    public string? To
    {
        get => _to;
        set => SetField(ref _to, value);
    }

    private int _maximum;
    public int Maximum
    {
        get => _maximum;
        set => SetField(ref _maximum, value);
    }
    
    private int _value;
    public int Value
    {
        get => _value;
        set => SetField(ref _value, value);
    }

    public LambdaCommand CommandSelect { get; set; }
    public LambdaCommand CommandStart { get; set; }

    public MainWindowModel()
    {
        CommandSelect = new LambdaCommand(
            execute: o =>
            {
                var dialog = new Microsoft.Win32.OpenFolderDialog
                {
                    Multiselect = false,
                    Title = "Select a folder"
                };
                var result = dialog.ShowDialog();
                
                if (result != true) return;
                
                var path = dialog.FolderName;
                switch (o?.ToString())
                {
                    case "From":
                        From = path;
                        break;
                    case "To":
                        To = path;
                        break;
                }
            });
        CommandStart = new LambdaCommand(
            execute: async _ =>
            {
                Maximum = CopyFile.GetCount(From);
                var progress = new Progress<int>();
                progress.ProgressChanged += (_, i) => Value = i; 
                await CopyFile.Copy(From, To, progress);
            },
            canExecute: _ => { return true;}
        );
    }
}