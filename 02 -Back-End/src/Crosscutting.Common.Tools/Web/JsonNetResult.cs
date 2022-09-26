using Newtonsoft.Json;
using System;
using System.Text;
using System.Web.Mvc;

namespace Crosscutting.Common.Tools.Web
{
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult()
        {
            SerializerSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MaxDepth = int.MaxValue
            };
        }

        public new Encoding ContentEncoding { get; set; }
        public new string ContentType { get; set; }
        public new object Data { get; set; }

        public Formatting Formatting { get; set; }
        public JsonSerializerSettings SerializerSettings { get; set; }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(ContentType)
                ? ContentType
                : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data == null) return;
            var writer = new JsonTextWriter(response.Output) { Formatting = Formatting };
            var serializer = JsonSerializer.Create(SerializerSettings);
            serializer.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
            serializer.Serialize(writer, Data);

            writer.Flush();
        }
    }
}