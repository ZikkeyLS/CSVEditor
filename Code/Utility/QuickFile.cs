using System.IO;
using System.Text;

namespace CSVEditor.Utility
{
    internal static class QuickFile
    {
        public static void RewriteLine(string path, int lineIndex, string value)
        {
            FileStream fileStream = new FileStream(path, FileMode.Open);
            byte[] buffer = new byte[1];
            int byteStart = lineIndex == 1 ? 0 : -1, byteEnd = -1;

            for (int i = 0, line = 1; i < fileStream.Length; i++)
            {
                fileStream.Read(buffer, 0, 1);

                if (buffer[0] == 10) //10 - перенос строки
                {
                    if (line == lineIndex)
                    {
                        byteEnd = i;
                        break;
                    }
                    line++;

                    if (line == lineIndex)
                        byteStart = i + 1;

                }
                if (i == fileStream.Length - 1)
                {
                    byteEnd = i;
                }
            }

            if (byteStart == -1 || byteEnd == -1)
                return;

            byte[] stringBuffer = Encoding.UTF8.GetBytes(value);
            fileStream.Position = byteEnd;
            byte[] tailBuffer = new byte[fileStream.Length - byteEnd];
            fileStream.Read(tailBuffer, 0, (int)(fileStream.Length - byteEnd));
            fileStream.Position = byteStart;
            fileStream.Write(stringBuffer, 0, stringBuffer.Length);
            fileStream.Write(tailBuffer, 0, tailBuffer.Length);
            fileStream.SetLength(byteStart + stringBuffer.Length + tailBuffer.Length);
            fileStream.Close();
        }
    }
}
