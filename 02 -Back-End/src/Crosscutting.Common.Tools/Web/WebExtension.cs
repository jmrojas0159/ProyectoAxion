using Crosscutting.Common.Tools.DataType;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Crosscutting.Common.Tools.Web
{
    public static class WebExtension
    {
        public static byte[] HasBytes(this FormCollection collection, string key, HttpFileCollectionBase files)
        {
            if (files == null) throw new ArgumentNullException(nameof(files));
            var file = files[key];
            if (file == null) return null;
            using (var inputStream = file.InputStream)
            {
                byte[] data;
                var memoryStream = inputStream as MemoryStream;
                try
                {
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    data = memoryStream.ToArray();
                }
                finally
                {
                    memoryStream?.Dispose();
                }

                return data;
            }
        }

        public static IList<FieldValueOrder> ToFieldValueOrder(this FormCollection collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var list = new List<FieldValueOrder>();
            var counter = 0;
            foreach (var key in collection.AllKeys.Distinct())
            {
                int keyint;
                var lastkey = "";
                var field = new FieldValueOrder();
                var order = 1;
                var applicant = 1;
                if (key.Contains("_"))
                {
                    var splitvalues = key.Split('_');
                    //only applicant
                    if (splitvalues.Length - 1 == 1)
                    {
                        lastkey = splitvalues.First();
                        int.TryParse(splitvalues.Last(), out applicant);
                    }
                    // applicant & order
                    if (splitvalues.Length - 1 == 2)
                    {
                        lastkey = splitvalues.First();
                        int.TryParse(splitvalues[1], out applicant);
                        int.TryParse(splitvalues.Last(), out order);
                    }
                }
                else
                {
                    lastkey = key;
                }
                if (!int.TryParse(lastkey, out keyint))
                {
                    counter++;
                    continue;
                }

                field.Applicant = applicant;
                field.Order = order;
                field.Key = lastkey;
                field.Value = collection[counter];
                list.Add(field);
                counter++;
            }
            return list;
        }

        public static FormCollection RemoveUnnecessaryAndEmptyFields(this FormCollection collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var list = new FormCollection();
            foreach (var key in collection.AllKeys.Distinct())
            {
                var value = collection.Get(key);
                if (!string.IsNullOrEmpty(value))
                {
                    list.Add(key, collection.Get(key));
                }
            }
            return list;
        }
        public static FormCollection RemovePrefijId_(this FormCollection collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var list = new FormCollection();
            foreach (var key in collection.AllKeys.Distinct())
            {
                var value = collection.Get(key);
                if (!string.IsNullOrEmpty(value))
                {
                    list.Add(key.Replace("Id_",""), collection.Get(key));
                }
            }
            return list;
        }

        public static IReadOnlyCollection<FieldValueOrder> ToFieldValueOrder(this HttpFileCollectionBase files)
        {
            if (files == null) throw new ArgumentNullException(nameof(files));
            var list = new List<FieldValueOrder>();
            foreach (var key in files.AllKeys.Distinct())
            {
                var fieldfile = files[key];
                int keyint;
                var lastkey = "";
                var field = new FieldValueOrder();
                var order = 1;
                var applicant = 1;
                if (key.Contains("_"))
                {
                    var splitvalues = key.Split('_');
                    //only applicant
                    if (splitvalues.Length - 1 == 1)
                    {
                        lastkey = splitvalues.First();
                        int.TryParse(splitvalues.Last(), out applicant);
                    }
                    // applicant & order
                    if (splitvalues.Length - 1 == 2)
                    {
                        lastkey = splitvalues.First();
                        int.TryParse(splitvalues[1], out applicant);
                        int.TryParse(splitvalues.Last(), out order);
                    }
                }
                else
                {
                    lastkey = key;
                }
                if (!int.TryParse(lastkey, out keyint))
                {
                    continue;
                }
                if (fieldfile == null) continue;
                using (var inputStream = fieldfile.InputStream)
                {
                    byte[] data;
                    var memoryStream = inputStream as MemoryStream;
                    try
                    {
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        data = memoryStream.ToArray();
                    }
                    finally
                    {
                        memoryStream?.Dispose();
                    }

                    field.Applicant = applicant;
                    field.Order = order;
                    field.Key = lastkey;
                    field.Bytes = data;
                    field.Value = fieldfile.FileName;
                    list.Add(field);
                }
            }
            return list;
        }

        public static Dictionary<string, string> ToDictionary(this FormCollection collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            return collection.AllKeys.ToDictionary(k => k, k => collection[k].Split(',')[0]);
        }

        public static IList<FieldValueOrder> RemoveEmptyFields(this IEnumerable<FieldValueOrder> collection)
        {
            return collection.Where(s => s.Key != "" && s.Key.Length >0).ToList();
        }

        /// <summary>
        ///     Clear fields
        /// </summary>
        public static IEnumerable<FieldValueOrder> ToFieldValueOrderCustom(this FormCollection collection)
            => collection?.AllKeys.Distinct().Select(x => new FieldValueOrder
            {
                Key = x.Replace("id_", string.Empty),
                Value = collection.GetValues(x)?.FirstOrDefault(),
                Applicant = 1
            }).Where(w => !string.IsNullOrWhiteSpace(w.Value) && w.Key.IsInt());
    }
}