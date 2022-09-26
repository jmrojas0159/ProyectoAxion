using System;
using System.Collections.Generic;

namespace Crosscutting.Common.JSON
{
    public class JsonResponse
    {
        public IDictionary<string, object> Callbacks { get; } = new Dictionary<string, object>();

        public bool Status { get; set; }

        public JsonResponse()
        {
            //Default CTOR
        }

        public string Message { get; set; }

        public void SetPartial(Uri url, string target, object vars, string loadtext = "Actualizando...")
        {
            Callbacks.Add("XGeneral.renderpartial", new
            {
                url,
                target,
                loadtext,
                vars
            });
        }

        public void SetGlitter(string title, string text, string className = "gritter-primary",
            string beforeClose = "")
        {
            Callbacks.Add("XGeneral.gritter", new
            {
                title,
                text,
                class_name = className,
                before_close = beforeClose
            });
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)")]
        public void SetToastr(
            string title,
            string text,
            string type = "Success",
            bool closeButton = true,
            string positionClass = "toast-top-right",
            int showDuration = 300,
            int hideDuration = 300,
            int timeOut = 5000,
            int extendedTimeout = 1000,
            string showEasing = "swing",
            string hideEasing = "linear",
            string showMethod = "fadeIn",
            string hideMethod = "fadeOut")
        {
            Callbacks.Add(
                $"XGeneral.toastr{type}",
                new
                {
                    title,
                    text,
                    closeButton,
                    debug = false,
                    positionClass,
                    showDuration,
                    hideDuration,
                    timeOut,
                    extendedTimeout,
                    showEasing,
                    hideEasing,
                    showMethod,
                    hideMethod
                });
        }

        public void SetCallBack(string functionname, object parameters)
        {
            Callbacks.Add(functionname, parameters);
        }

        public void SetRedirect(Uri url, object vars = null)
        {
            Callbacks.Add("XGeneral.redirect", new
            {
                url,
                vars
            });
        }

        public void SetLoadingClose()
        {
            Callbacks.Add("XGeneral.loading", null);
        }

        public void SetHtml(string target, string message)
        {
            Callbacks.Add("XGeneral.Html", new
            {
                target,
                message
            });
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)")]
        public void SetModalWithPartial(ModalType type, string path, string selector = null)
        {
            Callbacks.Add("XModal.initModal", new
            {
                type,
                selector = selector ?? $"#domModalGeneric{type}",
                path
            });
        }
    }
}