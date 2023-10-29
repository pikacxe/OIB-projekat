using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Common
{
    [DataContract]
    internal class MonitoredFile : IFile
    {
        private bool disposedValue;
        private MemoryStream file;
        private string name;
        private string hash;

        [DataMember(IsRequired =true)]
        public MemoryStream File { get => file; set => file = value ; }
        [DataMember(IsRequired =true)]
        public string Name { get => name ; set => name= value; }
        [DataMember(IsRequired =true)]
        public string Hash { get => hash; set => hash = value; }



        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    file.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MonitoredFile()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
