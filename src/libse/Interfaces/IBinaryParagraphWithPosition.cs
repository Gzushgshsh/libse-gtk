using Nikse.SubtitleEdit.Core.Common;
using Gdk;

namespace Nikse.SubtitleEdit.Core.Interfaces
{
    public interface IBinaryParagraphWithPosition : IBinaryParagraph
    {
        Size GetScreenSize();
        Position GetPosition();
        TimeCode StartTimeCode { get; }
        TimeCode EndTimeCode { get; }
    }
}
