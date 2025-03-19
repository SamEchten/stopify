using System.ComponentModel.DataAnnotations;

namespace Stopify.Requests.Music;

public class UploadSongRequest: IValidatableObject
{
    public string SongName { get; set; }

    public IFormFile file { get; set; }

    public ICollection<int> ArtistIds { get; set;  }

    private readonly ICollection<string> _allowedExtensions = [".mp3"];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(SongName)) yield return new ValidationResult("Song Name is required");

        if (ArtistIds.Count < 1) yield return new ValidationResult("Artist Ids is required");

        var fileExtension = Path.GetExtension(file.FileName).ToLower();

        if (!_allowedExtensions.Contains(fileExtension)) {
            yield return new ValidationResult($"Invalid file extension, please provide a file with one of the following extensions: {string.Join(", ", _allowedExtensions)}");
        }
    }
}