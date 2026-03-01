using Noxypedia;
using NoxypediaEditor.Model;
using System;

namespace NoxypediaEditor
{
    public static class Services
    {
        public static readonly Lazy<ConfigService> Config = new Lazy<ConfigService>();
        public static readonly Lazy<NoxypediaService> Noxypedia = new Lazy<NoxypediaService>();
    }
}
