﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Owin;
using MT.Logging.Models;

namespace MT.Logging
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    /// <summary>
    /// A simple Owin Middleware to capture HTTP requests and responses
    /// and store details of the call into a durable store.
    /// </summary>
    public sealed class HttpLogging : OwinMiddleware
    {
        /// <summary>
        /// Default value for the TrackingId response header.
        /// This value can be changed by specifying the TrackingIdPropertyName
        /// in the <see cref="HttpLoggingOptions"/> class passed to the ctor.
        /// </summary>
        private readonly string _trackingIdPropertyName = "http-tracking-id";

        private const string ContentType = "Content-Type";


        //private readonly IHttpTrackingStore storage_;
        internal readonly IHttpLoggingStore HttpLoggingStore;
        private readonly long _maxRequestLength = Int64.MaxValue;
        private readonly long _maxResponseLength = Int64.MaxValue;

        /// <summary>
        /// Initialize a new instance of the <see cref="HttpLogging"/> class.
        /// </summary>
        /// <param name="next">A reference to the next OwinMiddleware object in the chain.</param>
        /// <param name="options">A reference to an <see cref="HttpLoggingOptions"/> class.</param>
        public HttpLogging(OwinMiddleware next, HttpLoggingOptions options)
            : base(next)
        {
            HttpLoggingStore = options.TrackingStore;

            if (!string.IsNullOrEmpty(options.TrackingIdPropertyName))
                _trackingIdPropertyName = options.TrackingIdPropertyName;

            _maxRequestLength = options.MaximumRecordedRequestLength ?? _maxRequestLength;
            _maxResponseLength = options.MaximumRecordedResponseLength ?? _maxResponseLength;
        }

        /// <summary>
        /// Processes the incoming HTTP call and capture details about
        /// the request, the response, the identity of the caller and the
        /// call duration to persistent storage.
        /// </summary>
        /// <param name="context">A reference to the Owin context.</param>
        /// <returns />
        public override async Task Invoke(IOwinContext context)
        {
            var request = context.Request;
            var response = context.Response;

            // capture details about the caller identity

            var identity =
                request.User != null && request.User.Identity.IsAuthenticated ?
                    request.User.Identity.Name :
                    "(anonymous)"
                    ;

            var record = new HttpEntry
            {
                CallerIdentity = identity,
            };

            // replace the request stream in order to intercept downstream reads

            var requestBuffer = new MemoryStream();
            var requestStream = new ContentStream(requestBuffer, request.Body);
            request.Body = requestStream;

            // replace the response stream in order to intercept downstream writes

            var responseBuffer = new MemoryStream();
            var responseStream = new ContentStream(responseBuffer, response.Body);
            response.Body = responseStream;

            // add the "Http-Tracking-Id" response header

            context.Response.OnSendingHeaders(state =>
            {
                var ctx = state as IOwinContext;
                if (ctx == null) return;
                var resp = ctx.Response;

                // adding the tracking id response header so that the user
                // of the API can correlate the call back to this entry

                resp.Headers.Add(_trackingIdPropertyName, new[] { record.LoggingId.ToString("d"), });
            }, context);

            // invoke the next middleware in the pipeline

            await Next.Invoke(context);

            // rewind the request and response buffers
            // and record their content

            WriteRequestHeaders(request, record);
            record.RequestLength = requestStream.ContentLength;
            record.Request = await WriteContentAsync(requestStream, record.RequestHeaders, _maxRequestLength);

            WriteResponseHeaders(response, record);
            record.ResponseLength = responseStream.ContentLength;
            record.Response = await WriteContentAsync(responseStream, record.ResponseHeaders, _maxResponseLength);

            // persist details of the call to durable storage

            await HttpLoggingStore.InsertRecordAsync(record);
        }

        private static void WriteRequestHeaders(IOwinRequest request, HttpEntry record)
        {
            record.Verb = request.Method;
            record.RequestUri = request.Uri;
            record.RequestHeaders = request.Headers;
        }

        private static void WriteResponseHeaders(IOwinResponse response, HttpEntry record)
        {
            record.StatusCode = response.StatusCode;
            record.ReasonPhrase = response.ReasonPhrase;
            record.ResponseHeaders = response.Headers;
        }

        private static async Task<string> WriteContentAsync(ContentStream stream, IDictionary<string, string[]> headers, long maxLength)
        {
            var contentType =
                headers.ContainsKey(ContentType) ?
                headers[ContentType][0] :
                null
                ;

            return await stream.ReadContentAsync(contentType, maxLength);
        }
    }
}
