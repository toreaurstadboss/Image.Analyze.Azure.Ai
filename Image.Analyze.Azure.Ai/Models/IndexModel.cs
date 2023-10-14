namespace Image.Analyze.Azure.Ai.Models;

public class IndexModel
{
    public string FilePath { get; set; }
    public long FileSize { get; set; }
    public string ImageAnalysisOutputText { get; set; }
    public string SavedFilePath { get; set; }
    public string PreviewImageUrl { get; set; }
    public string Caption { get; set; }
    public List<string> Tags { get; set; } = new();


    public IndexModel()
    {
    }

    public IndexModel(ImageSaveModel imageSaveModel)
    {
        FilePath = imageSaveModel.FilePath;
        FileSize = imageSaveModel.FileSize;
        SavedFilePath = imageSaveModel.SavedFilePath;
        PreviewImageUrl = imageSaveModel.PreviewImageUrl;
    }
}