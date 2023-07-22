using BMOS.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text.Json;

namespace BMOS.Helpers
{
	public static class ExtensionHelper
	{
		public static string toVnd(this double price) {
			return $"{price:#,##0.00} vnd";
		}



		public static void Set<T>(this ISession session, string key, T value)
		{
			session.SetString(key, JsonSerializer.Serialize(value));
		}

		public static T? Get<T>(this ISession session, string key)
		{
			var value = session.GetString(key);
			return value == null ? default : JsonSerializer.Deserialize<T>(value); 
		}
	}
}
