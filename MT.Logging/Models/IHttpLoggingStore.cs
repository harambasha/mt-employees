using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Logging
{
    /// <summary>
    /// Interface for tracking details about HTTP calls.
    /// </summary>
    public interface IHttpLoggingStore
    {
        /// <summary>
        /// Persist details of an HTTP call into durable storage.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        Task InsertRecordAsync(HttpEntry record);
    }
}
