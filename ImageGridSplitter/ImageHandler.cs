using System.Drawing.Imaging;
using System.Text.Json.Nodes;

namespace ImageGridSplitter;

public class ImageHandler
{
    private readonly string? _root;
    private readonly string? _output;
    private readonly string? _pattern;
    
    public ImageHandler(JsonNode config)
    {
        _pattern = config["pattern"]?.ToString();
        _root = config["root"]?.ToString();
        _output = config["output"]?.ToString();
    }

    public IEnumerable<string> GetImagesPathFromRoot()
    {
        if (string.IsNullOrEmpty(_root))
            throw new NullReferenceException("Root folder is not set");

        var folder = new DirectoryInfo(_root);
        var images = folder.GetFiles();

        return images.Select(image => $"{_root}/{image.Name}").ToArray();
    }
    
    public void SaveImagesToOutputDirectory(SplitResult[] images)
    {
        if (string.IsNullOrEmpty(_output))
            throw new NullReferenceException("Output folder is not set");
    
        if (string.IsNullOrEmpty(_pattern))
            throw new NullReferenceException("Pattern is not set");
    
        if (!Directory.Exists(_output))
            Directory.CreateDirectory(_output);
        
        for (var i = 0; i < images.Count(); i++) {
            var rawName = _pattern; // safe copy
            var imageName = rawName
                .Replace("%n", images[i].FileName)
                .Replace("%i", i.ToString())
                .Replace("\"", string.Empty);
            
            var path = $"{_output}/{imageName}";

            Console.WriteLine("Saving image to: " + path);
            images[i].Image.Save(path, GetFormat(path));
        }
    }
    
    private ImageFormat GetFormat(string path)
    {
        var extension = path.Split('.').Last().ToLower();
        return extension switch
        {
            "png" => ImageFormat.Png,
            "jpg" => ImageFormat.Jpeg,
            "jpeg" => ImageFormat.Jpeg,
            "bmp" => ImageFormat.Bmp,
            "gif" => ImageFormat.Gif,
            _ => throw new NotSupportedException("Unsupported image format")
        };
    }
}