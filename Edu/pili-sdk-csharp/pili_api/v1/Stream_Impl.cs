using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pili_sdk.pili;
using pili_sdk.pili_common;

namespace pili_sdk.pili_api.v1
{
    public class Stream_Impl : IStream
    {
        private pili_sdk.pili_api.v1.Credentials _credentials = (pili_sdk.pili_api.v1.Credentials)null;

        public pili_sdk.ICredentials Credentials
        {
            get
            {
                return this._credentials == null ? (pili_sdk.ICredentials)new pili_sdk.pili_api.v1.Credentials(Config.ACCESS_KEY, Config.SECRET_KEY) : (pili_sdk.ICredentials)this._credentials;
            }
        }

        public bool Test(string accesskey, string secretkey, string hubname, string version)
        {
            pili_sdk.pili_api.v1.Credentials credentials = new pili_sdk.pili_api.v1.Credentials(accesskey, secretkey);
            string str1 = string.Empty;
            string str2 = string.Empty;
            int num = 0;
            try
            {
                Encoding utF8 = Encoding.UTF8;
                hubname = HttpUtility.UrlEncode(hubname);
                if (Utils.isArgNotEmpty(str1))
                    str1 = HttpUtility.UrlEncode(str1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            string uriString = string.Format("{0}/streams?hub={1}", (object)Config.API_BASE_URL, (object)hubname);
            if (Utils.isArgNotEmpty(str1))
                uriString = uriString + "&marker=" + str1;
            if (num > 0)
                uriString = uriString + (object)"&limit=" + (string)(object)num;
            if (Utils.isArgNotEmpty(str2))
                uriString = uriString + "&title=" + str2;
            HttpWebResponse httpWebResponse;
            try
            {
                Uri uri = new Uri(uriString);
                string str3 = credentials.signRequest(uri, "GET", (byte[])null, (string)null);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Method = "GET";
                httpWebRequest.UserAgent = Utils.UserAgent;
                httpWebRequest.Headers.Add("Authorization", str3);
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (httpWebResponse.StatusCode != HttpStatusCode.OK)
                throw new Exception();
            try
            {
                StreamList streamList = new StreamList(JObject.Parse(new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd()), (pili_sdk.ICredentials)credentials);
                return true;
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        public pili_sdk.pili.Stream Create()
        {
            return this.Create((string)null, (string)null, (string)null);
        }

        public pili_sdk.pili.Stream Create(string title)
        {
            return this.Create(title, (string)null, (string)null);
        }

        public pili_sdk.pili.Stream Create(string title, string publishKey, string publishSecurity)
        {
            string uriString = Config.API_BASE_URL + "/streams";
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("hub", Config.HUB_NAME);
            if (Utils.isArgNotEmpty(title))
            {
                if (title.Length < 5 || title.Length > 200)
                    throw new Exception(MessageConfig.ILLEGAL_TITLE_MSG);
                dictionary.Add("title", title);
            }
            if (Utils.isArgNotEmpty(publishKey))
                dictionary.Add("publishKey", publishKey);
            if (Utils.isArgNotEmpty(publishSecurity))
                dictionary.Add("publishSecurity", publishSecurity);
            HttpWebResponse httpWebResponse;
            try
            {
                Uri uri = new Uri(uriString);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                string contentType = "application/json";
                httpWebRequest.Method = "POST";
                byte[] bytes = StringHelperClass.GetBytes(JsonConvert.SerializeObject((object)dictionary).ToString(), "UTF-8");
                string str = this.Credentials.signRequest(uri, "POST", bytes, contentType);
                httpWebRequest.ContentType = contentType;
                httpWebRequest.UserAgent = Utils.UserAgent;
                httpWebRequest.Headers.Add("Authorization", str);
                httpWebRequest.ContentLength = (long)bytes.Length;
                using (System.IO.Stream requestStream = httpWebRequest.GetRequestStream())
                    Utils.CopyN(requestStream, (System.IO.Stream)new MemoryStream(bytes), (long)bytes.Length);
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (httpWebResponse.StatusCode != HttpStatusCode.OK)
                throw new Exception();
            try
            {
                return new pili_sdk.pili.Stream(JObject.Parse(new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd()), this.Credentials);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        public StreamList List()
        {
            return this.List((string)null, 0L, (string)null, new bool?());
        }

        public StreamList List(bool liveonly)
        {
            return this.List((string)null, 0L, (string)null, new bool?(true));
        }

        public StreamList List(string marker, long limit)
        {
            return this.List(marker, limit, (string)null, new bool?());
        }

        public StreamList List(string marker, long limit, string titlePrefix)
        {
            return this.List(marker, limit, titlePrefix, new bool?());
        }

        public StreamList List(string marker, long limit, string titlePrefix, bool? liveonly)
        {
            string hubName = Config.HUB_NAME;
            string str1;
            try
            {
                Encoding utF8 = Encoding.UTF8;
                str1 = HttpUtility.UrlEncode(hubName);
                if (Utils.isArgNotEmpty(marker))
                    marker = HttpUtility.UrlEncode(marker);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            string uriString = string.Format("{0}/streams?hub={1}", (object)Config.API_BASE_URL, (object)str1);
            if (Utils.isArgNotEmpty(marker))
                uriString = uriString + "&marker=" + marker;
            if (limit > 0L)
                uriString = uriString + (object)"&limit=" + (string)(object)limit;
            if (Utils.isArgNotEmpty(titlePrefix))
                uriString = uriString + "&title=" + titlePrefix;
            if (liveonly.HasValue && liveonly.Value)
                uriString += "&status=connected";
            HttpWebResponse httpWebResponse;
            try
            {
                Uri uri = new Uri(uriString);
                string str2 = this.Credentials.signRequest(uri, "GET", (byte[])null, (string)null);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Method = "GET";
                httpWebRequest.UserAgent = Utils.UserAgent;
                httpWebRequest.Headers.Add("Authorization", str2);
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (httpWebResponse.StatusCode != HttpStatusCode.OK)
                throw new Exception();
            try
            {
                return new StreamList(JObject.Parse(new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd()), this.Credentials);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        public pili_sdk.pili.Stream GetForID(string id)
        {
            string str1 = id;
            if (str1 == null)
                throw new Exception("FATAL EXCEPTION: streamId is null!");
            if (str1.IndexOf(".") < 0)
                str1 = "z1." + Config.HUB_NAME + "." + id;
            string uriString = string.Format("{0}/streams/{1}", (object)Config.API_BASE_URL, (object)str1);
            HttpWebResponse httpWebResponse;
            try
            {
                Uri uri = new Uri(uriString);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Method = "GET";
                string str2 = this.Credentials.signRequest(uri, "GET", (byte[])null, (string)null);
                httpWebRequest.UserAgent = Utils.UserAgent;
                httpWebRequest.Headers.Add("Authorization", str2);
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (httpWebResponse.StatusCode != HttpStatusCode.OK)
                throw new Exception();
            try
            {
                return new pili_sdk.pili.Stream(JObject.Parse(new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd()), this.Credentials);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        public pili_sdk.pili.Stream GetForTitle(string title)
        {
            pili_sdk.pili.Stream stream1 = (pili_sdk.pili.Stream)null;
            try
            {
                stream1 = this.GetForID(title);
            }
            catch
            {
            }
            if (stream1 != null)
                return stream1;
            string marker = (string)null;
            long limit = 30L;
            string titlePrefix = (string)null;
            StreamList streamList = this.List(marker, limit, titlePrefix);
            while (stream1 == null && streamList.Streams.Count > 0)
            {
                foreach (pili_sdk.pili.Stream stream2 in (IEnumerable<pili_sdk.pili.Stream>)streamList.Streams)
                {
                    if (stream2.StreamID.EndsWith("." + Config.HUB_NAME + "." + title))
                    {
                        stream1 = stream2;
                        break;
                    }
                }
                if (stream1 == null)
                    streamList = this.List(streamList.Marker, limit, titlePrefix);
            }
            return stream1;
        }

        public string Delete(pili_sdk.pili.Stream stream)
        {
            if (stream == null)
                throw new Exception("FATAL EXCEPTION: streamId is null!");
            string uriString = string.Format("{0}/streams/{1}", (object)Config.API_BASE_URL, (object)stream.StreamID);
            HttpWebResponse httpWebResponse;
            try
            {
                Uri uri = new Uri(uriString);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                string str = this.Credentials.signRequest(uri, "DELETE", (byte[])null, (string)null);
                httpWebRequest.Method = "DELETE";
                httpWebRequest.UserAgent = Utils.UserAgent;
                httpWebRequest.Headers.Add("Authorization", str);
                Console.WriteLine(str);
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if ((int)httpWebResponse.StatusCode / 100 == 2)
                return "No Content";
            throw new Exception();
        }

        public string Delete(string title)
        {
            return this.Delete(this.GetForTitle(title));
        }

        public string DeleteForID(string id)
        {
            return this.Delete(this.GetForID(id));
        }

        public pili_sdk.pili.Stream Update(pili_sdk.pili.Stream stream, string publishKey, string publishSecrity)
        {
            return this.Update(stream, publishKey, publishSecrity, new bool?());
        }

        public pili_sdk.pili.Stream Update(pili_sdk.pili.Stream stream, string publishKey, string publishSecrity, bool? disabled)
        {
            JObject jobject = new JObject();
            if (stream == null)
                throw new Exception("FATAL EXCEPTION: streamId is null!");
            if (Utils.isArgNotEmpty(publishKey))
                jobject.Add("publishKey", (JToken)publishKey);
            if (Utils.isArgNotEmpty(publishSecrity))
                jobject.Add("publishSecurity", (JToken)publishSecrity);
            if (disabled.HasValue)
                jobject.Add("disabled", (JToken)disabled);
            string uriString = string.Format("{0}/streams/{1}", (object)Config.API_BASE_URL, (object)stream.StreamID);
            HttpWebResponse httpWebResponse;
            try
            {
                Uri uri = new Uri(uriString);
                byte[] bytes = StringHelperClass.GetBytes(JsonConvert.SerializeObject((object)jobject).ToString(), "UTF-8");
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                string contentType = "application/json";
                string str = this.Credentials.signRequest(uri, "POST", bytes, contentType);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = contentType;
                httpWebRequest.UserAgent = Utils.UserAgent;
                httpWebRequest.Headers.Add("Authorization", str);
                httpWebRequest.ContentLength = (long)bytes.Length;
                using (System.IO.Stream requestStream = httpWebRequest.GetRequestStream())
                    Utils.CopyN(requestStream, (System.IO.Stream)new MemoryStream(bytes), (long)bytes.Length);
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (httpWebResponse.StatusCode != HttpStatusCode.OK)
                throw new Exception();
            try
            {
                return new pili_sdk.pili.Stream(JObject.Parse(new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd()), this.Credentials);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        public StreamStatus Status(pili_sdk.pili.Stream stream)
        {
            if (stream == null)
                throw new Exception("FATAL EXCEPTION: streamId is null!");
            string uriString = string.Format("{0}/streams/{1}/status", (object)Config.API_BASE_URL, (object)stream.StreamID);
            HttpWebResponse httpWebResponse;
            try
            {
                Uri uri = new Uri(uriString);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                string str = this.Credentials.signRequest(uri, "GET", (byte[])null, (string)null);
                httpWebRequest.Method = "GET";
                httpWebRequest.UserAgent = Utils.UserAgent;
                httpWebRequest.Headers.Add("Authorization", str);
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (httpWebResponse.StatusCode != HttpStatusCode.OK)
                throw new Exception();
            try
            {
                return new StreamStatus(JObject.Parse(new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd()), stream);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        public StreamStatus Status(string streamid)
        {
            return this.Status(this.GetForID(streamid));
        }

        public pili_sdk.pili.Stream Enable(pili_sdk.pili.Stream stream)
        {
            return this.Update(stream, (string)null, (string)null, new bool?(false));
        }

        public pili_sdk.pili.Stream Disable(pili_sdk.pili.Stream stream)
        {
            return this.Update(stream, (string)null, (string)null, new bool?(true));
        }

        public SnapshotResponse Snapshot(pili_sdk.pili.Stream stream, string name, string format)
        {
            return this.Snapshot(stream, name, format, 0L, (string)null);
        }

        public SnapshotResponse Snapshot(pili_sdk.pili.Stream stream, string name, string format, string notifyUrl)
        {
            return this.Snapshot(stream, name, format, 0L, notifyUrl);
        }

        public SnapshotResponse Snapshot(pili_sdk.pili.Stream stream, string name, string format, long time, string notifyUrl)
        {
            if (stream == null)
                throw new Exception("FATAL EXCEPTION: streamId is null!");
            if (!Utils.isArgNotEmpty(name))
                throw new Exception("Illegal file name !");
            if (!Utils.isArgNotEmpty(format))
                throw new Exception("Illegal format !");
            string uriString = string.Format("{0}/streams/{1}/snapshot", (object)Config.API_BASE_URL, (object)stream.StreamID);
            JObject jobject = new JObject();
            jobject.Add("name", (JToken)name);
            jobject.Add("format", (JToken)format);
            if (time > 0L)
                jobject.Add("time", (JToken)time);
            if (Utils.isArgNotEmpty(notifyUrl))
                jobject.Add("notifyUrl", (JToken)notifyUrl);
            HttpWebResponse httpWebResponse;
            try
            {
                Uri uri = new Uri(uriString);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                string contentType = "application/json";
                byte[] bytes = StringHelperClass.GetBytes(jobject.ToString(), "UTF-8");
                string str = this.Credentials.signRequest(uri, "POST", bytes, contentType);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = contentType;
                httpWebRequest.UserAgent = Utils.UserAgent;
                httpWebRequest.Headers.Add("Authorization", str);
                httpWebRequest.ContentLength = (long)bytes.Length;
                using (System.IO.Stream requestStream = httpWebRequest.GetRequestStream())
                    Utils.CopyN(requestStream, (System.IO.Stream)new MemoryStream(bytes), (long)bytes.Length);
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (httpWebResponse.StatusCode != HttpStatusCode.OK)
                throw new Exception();
            try
            {
                return new SnapshotResponse(JObject.Parse(new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd()));
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
    }
}
