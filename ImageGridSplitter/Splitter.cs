using System.Drawing;
using System.Text.Json.Nodes;

namespace ImageGridSplitter;

public class SplitResult
{
    public string FileName { get; set; }
    public Bitmap Image { get; set; }
}

public class Splitter
{
    private int _rows;
    private int _columns;
    
    public Splitter(JsonNode configuration)
    {
        _rows = configuration["rows"]?.GetValue<int>() ?? 1;
        _columns = configuration["columns"]?.GetValue<int>() ?? 1;            
    }
    
    public SplitResult[] Split(string path)
    {
        var result = new List<SplitResult>(_rows * _columns);
        var image = LoadImageFile(path);
        var width = image.Width / _columns;
        var height = image.Height / _rows;
        
        // get filename from path
        var fileName = Path.GetFileNameWithoutExtension(path);
        fileName = fileName.Split('/').Last().Split('\\').Last();
        
        for (var i = 0; i < _rows; i++)
        {
            for (var j = 0; j < _columns; j++)
            {
                var bitmap = new Bitmap(width, height);
                using (var g = Graphics.FromImage(bitmap))
                {
                    var srcRect = new Rectangle(j * width, i * height, width, height);
                    var destRect = new Rectangle(0, 0, width, height);
                    g.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
                }
                
                result.Add(new SplitResult {
                    FileName = fileName, 
                    Image = bitmap
                });
            }
        }
        
        return result.ToArray();
    }

    private static Bitmap LoadImageFile(string path)
    {
        return new Bitmap(path);
    }
}