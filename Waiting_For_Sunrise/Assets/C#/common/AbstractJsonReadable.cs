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
                throw new ArgumentException("读取Json资源内容为空：" + fileResource.Path);
            }
            T t = JsonConvert.DeserializeObject<T>(jsonFileContent);
            InitT(t);
            R r = Convert(t);
            return r;
        }

        protected abstract R Convert(T jsonDo);

        protected virtual void InitT(T t) 
        { 
            // default do nothing
        }
    }
}