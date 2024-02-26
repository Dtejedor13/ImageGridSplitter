using System.Text.Json.Nodes;
using ImageGridSplitter;

if (args.Length == 0)
    throw new Exception("No configuration file provided");

// load config file
var json = File.ReadAllText(args[0]);
var config = JsonNode.Parse(json);

if (config == null)
    throw new Exception("Invalid configuration file");

var splitterConfig = config["splitter"];
if (splitterConfig == null)
    throw new Exception("Invalid configuration file");

var handlerConfig = config["handler"];
if (handlerConfig == null)
    throw new Exception("Invalid configuration file");

var rootFolder = handlerConfig["root"]?.ToString();
if (string.IsNullOrEmpty(rootFolder))
    throw new Exception("Invalid configuration file");

// load essentials
var imageHandler = new ImageHandler(handlerConfig);
var splitter = new Splitter(splitterConfig);

// do your job
foreach (var imagePath in imageHandler.GetImagesPathFromRoot())
{
    Console.WriteLine($"Processing image: {imagePath}");
    var splitResults = splitter.Split(imagePath);
    imageHandler.SaveImagesToOutputDirectory(splitResults);
}
