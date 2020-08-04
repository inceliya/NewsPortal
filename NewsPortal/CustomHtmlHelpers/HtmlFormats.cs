using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.CustomHtmlHelpers
{
    class IndexParams
    {
        public string filter = null;
        public string sort = null;
        public string search = null;
        public bool? reverse = null;
        public int? page = null;
        public IndexParams(HttpRequestBase request)
        {
            filter = request.Params["filter"];
            sort = request.Params["sort"];
            search = request.Params["search"];
            if (!string.IsNullOrEmpty(request.Params["reverse"]))
                reverse = (request.Params["reverse"].ToLower() == "true");
            if (!string.IsNullOrEmpty(request.Params["page"]))
            {
                int tmp;
                if (int.TryParse(request.Params["page"], out tmp))
                    page = tmp;
            }
        }
        public void AddParam(string key, string param)
        {
            switch (key)
            {
                case "filter":
                    if (filter != param)
                    {
                        filter = param;
                        page = 1;
                    }
                    break;
                case "sort":
                    sort = param;
                    break;
                case "search":
                    search = param;
                    page = 1;
                    break;
                case "reverse":
                    reverse = param.ToLower() == "true";
                    break;
                case "page":
                    int tmp;
                    if (int.TryParse(param, out tmp))
                        page = tmp;
                    break;
            }
        }
        public string GetParams()
        {
            string res = "";
            res += !string.IsNullOrEmpty(filter) ? $"filter={filter}&" : "";
            res += !string.IsNullOrEmpty(sort) ? $"sort={sort}&" : "";
            res += reverse != null ? $"reverse={reverse}&" : "";
            res += page != null ? $"page={page}&" : "";
            res += !string.IsNullOrEmpty(search) ? $"search={search}&" : "";
            if (!string.IsNullOrEmpty(res)) res = "?" + res.Substring(0, res.Length - 1);
            return res;
        }
    }
    public static class HtmlFormats
    {
        public static IHtmlString Date(this HtmlHelper helper, DateTime date)
        {
            string res = "<div class=\"dateUtc\">{";
            res += $"\"year\":{date.Year},";
            res += $"\"month\":{date.Month},";
            res += $"\"day\":{date.Day},";
            res += $"\"hour\":{date.Hour},";
            res += $"\"minute\":{date.Minute}";
            res += "}</div>";

            return new MvcHtmlString(res);
        }
        public static IHtmlString IndexUrl(this HtmlHelper helper, HttpRequestBase request, string key, string param, bool? reverse = null)
        {
            var indexParams = new IndexParams(request);
            indexParams.AddParam(key, param);
            if (reverse != null)
                indexParams.AddParam("reverse", reverse.ToString());
            return new HtmlString(new UrlHelper(request.RequestContext).Action("Index", request.QueryString["controller"]) + indexParams.GetParams());
        }
    }
}