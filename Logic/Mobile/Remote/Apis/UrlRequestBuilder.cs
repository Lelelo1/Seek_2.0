using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinLogic.Remote.Apis
{

	// use nuget instead..?
	/*
	public class UrlRequestBuilder
	{
		public static string GetUrlFrom(Endpoint endpoint)
		{

			var url = endpoint.BaseUrl;
			if (!(url.EndsWith("?", StringComparison.InvariantCulture))) url += "?";

			var mandatory = endpoint.MandatoryParams;
			if (endpoint.MandatoryParams.Count == 0) throw new ArgumentException("You did not supply mandatory argument to UrlRequestBuilder. Api key, sessionToken etc");

			foreach(var param in mandatory)
			{
				url += param.Key + "=" + param.Value + "&";
			}

			var extraParams = endpoint.ExtraParams;
			if (extraParams != null) // throw new ArgumentException("You did not supply extraParams argument to UrlRequestBuilder");
			{
				foreach (var param in extraParams)
				{
					url += param.Key + "=" + param.Value + "&";
				}
			}
			var extras = endpoint.Extras;
			if(extras != null)
			{
				foreach (var param in extras)
				{
					url += param + "&";
				}
			}
			url = url.Remove(url.Length - 1);

			return url;
		}

	}
	*/
}