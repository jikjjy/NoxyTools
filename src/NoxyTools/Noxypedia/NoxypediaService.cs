using System;
using System.IO;
using Noxypedia.Model;
using Noxypedia.Utils;

namespace Noxypedia
{
    public class NoxypediaService
    {
        public NoxypediaSet Data { get; private set; } = new NoxypediaSet();
        public DateTime UpdateDate { get; private set; } = DateTime.MinValue;
        public string Auther { get; private set; } = string.Empty;
        public int Version { get; private set; } = 0;
        public string Comment { get; private set; } = string.Empty;
        public event EventHandler? DataChanged;
        public event EventHandler? InformationChanged;

        /// <summary>
        /// noxypedia.dat 파일에서 데이터를 로드합니다.
        /// </summary>
        public bool LoadFromFile(string filePath)
        {
            try
            {
                var (data, version) = NoxypediaDataFile.Load(filePath);
                Data = data;
                Version = version;
                UpdateDate = File.GetLastWriteTimeUtc(filePath);
                Auther = "local";
                Comment = string.Empty;
                raiseInformationChangedEvent();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void RasieDataChangedEvent()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
        }

        private void raiseInformationChangedEvent()
        {
            InformationChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
