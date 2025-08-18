using System;

namespace Europa.Resources
{
    public class LocalizedEnumConverter : ResourcesEnumConverter
    {
        public LocalizedEnumConverter(Type type) : base(type, GlobalMessages.ResourceManager)
        {
        }
    }
}
