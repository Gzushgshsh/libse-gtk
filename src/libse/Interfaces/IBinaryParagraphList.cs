using Gdk;

namespace Nikse.SubtitleEdit.Core.Interfaces
{
    public interface IBinaryParagraphList
    {
        Pixbuf GetSubtitleBitmap(int index, bool crop = true);
        bool GetIsForced(int index);
    }
}
