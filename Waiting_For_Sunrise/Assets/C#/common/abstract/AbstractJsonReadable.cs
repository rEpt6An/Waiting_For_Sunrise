using Newtonsoft.Json;
using System;

namespace Assets.C_.common
{
    public abstract class AbstractJsonReadable<T,R>: IReadable<R>
    {
        public R Read(FileResource fileResource)
        {
            string jsonFileContent = fileResource.FileContent;
            if (jsonFileContent == null)
            {
                throw new ArgumentException("fileResourceÖÐcontentÎª¿Õ£º" + fileResource.Path);
            }
            T t = JsonConverter.FromJson<T>(jsonFileContent);
            R r = Convert(t);
            return r;
        }

        protected abstract R Convert(T jsonDo);
    }
}