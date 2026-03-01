using Noxypedia.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace NoxyTools.Core.Services
{
    public class FavoriteService
    {
        public event EventHandler FavoriteChanged;
        private HashSet<string> mFavoriteItems = new HashSet<string>();
        private readonly string mBasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"Noxy Tools", @"cache");
        private string mFavoriteFile => Path.Combine(mBasePath, @"favorites.txt");
        private readonly object mSyncRoot = new object();

        public FavoriteService()
        {
            Reload();
        }

        public bool IsFavoriteItem(ItemSet item)
        {
            return mFavoriteItems.Contains(item.Name);
        }

        public bool ToggleFavorite(ItemSet item)
        {
            try
            {
                if (mFavoriteItems.Contains(item.Name) == true)
                {
                    mFavoriteItems.Remove(item.Name);
                    return false;
                }
                else
                {
                    mFavoriteItems.Add(item.Name);
                    return true;
                }
            }
            finally
            {
                Backup();
                RaiseFavoriteChanged();
            }
        }

        public void Reload()
        {
            Restore();
        }

        public IEnumerable<ItemSet> GetFavorites(CacheService cache)
        {
            return cache.NoxypediaData.Items
                .Where(item => mFavoriteItems.Contains(item.Name));
        }

        private void Backup()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            lock (mSyncRoot)
            {
                checkDirectory(mBasePath);
                using (var stream = File.OpenWrite(mFavoriteFile))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.SetLength(0);
                    formatter.Serialize(stream, mFavoriteItems);
                }
            }
        }

        private void Restore()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            lock (mSyncRoot)
            {
                if (File.Exists(mFavoriteFile) == true)
                {
                    using (var stream = File.OpenRead(mFavoriteFile))
                    {
                        mFavoriteItems = (HashSet<string>)formatter.Deserialize(stream);
                    }
                }
            }
        }

        private void RaiseFavoriteChanged()
        {
            FavoriteChanged?.Invoke(this, EventArgs.Empty);
        }

        private static void checkDirectory(string path)
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
