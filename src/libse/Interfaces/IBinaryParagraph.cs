using Gdk;

namespace Nikse.SubtitleEdit.Core.Interfaces
{
    public interface IBinaryParagraph
    {
        bool IsForced { get; }
        Pixbuf GetBitmap();
    }
}
