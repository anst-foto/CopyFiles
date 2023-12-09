namespace CopyFiles.Lib;

public static class CopyFile
{
    public static int GetCount(string source) => Directory.GetFiles(source).Length;
    
    public static async Task Copy(string source, string destination, IProgress<int> progress)
    {
        var files = Directory.GetFiles(source).Select(f => new FileInfo(f));
        var count = 0;

        foreach (var file in files)
        {
            var dist = $@"{destination}\{file.Name}";
            File.Copy(file.FullName, dist);
            progress.Report(++count);
            await Task.Delay(2000);
        }
    }
}