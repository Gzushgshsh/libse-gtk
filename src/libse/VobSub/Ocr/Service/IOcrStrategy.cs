using System.Collections.Generic;
using Gdk;

namespace Nikse.SubtitleEdit.Core.VobSub.Ocr.Service
{
    public interface IOcrStrategy
    {
        string GetName();
        string GetUrl();
        List<string> PerformOcr(string language, List<Pixbuf> images);
        int GetMaxImageSize();
        int GetMaximumRequestArraySize();
        List<OcrLanguage> GetLanguages();
    }
}
