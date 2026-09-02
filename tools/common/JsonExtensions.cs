using System.Text.Json;

namespace Xamarin.Utils;

public static class JsonExtensions {
	public static JsonElement? GetNullableProperty (this JsonElement element, string propertyName)
	{
		if (element.TryGetProperty (propertyName, out var value))
			return value;
		return null;
	}

	public static JsonElement? GetNullableProperty (this JsonElement? element, string propertyName)
	{
		if (element?.TryGetProperty (propertyName, out var value) == true)
			return value;
		return null;
	}

	public static string? GetStringProperty (this JsonElement? element, string propertyName)
	{
		return GetNullableProperty (element, propertyName)?.GetString ();
	}

	public static string? GetStringProperty (this JsonElement element, params string [] propertyName)
	{
		return FindProperty (element, propertyName)?.GetString ();
	}

	public static string GetStringPropertyOrEmpty (this JsonElement? element, string propertyName)
	{
		return GetStringProperty (element, propertyName) ?? string.Empty;
	}

	public static string GetStringPropertyOrEmpty (this JsonElement element, params string [] propertyName)
	{
		return GetStringProperty (element, propertyName) ?? string.Empty;
	}

	public static long? GetInt64Property (this JsonElement? element, params string [] nodes)
	{
		return FindProperty (element, nodes)?.GetInt64 ();
	}

	public static ulong? GetUInt64Property (this JsonElement? element, params string [] nodes)
	{
		return FindProperty (element, nodes)?.GetUInt64 ();
	}

	public static int? GetInt32Property (this JsonElement element, params string [] nodes)
	{
		return FindProperty (element, nodes)?.GetInt32 ();
	}

	public static uint? GetUInt32Property (this JsonElement element, params string [] nodes)
	{
		return FindProperty (element, nodes)?.GetUInt32 ();
	}

	public static bool? GetBooleanProperty (this JsonElement element, params string [] nodes)
	{
		return FindProperty (element, nodes)?.GetBoolean ();
	}

	public static JsonElement? FindProperty (this JsonDocument doc, params string [] nodes)
	{
		return FindProperty (doc.RootElement, nodes);
	}

	public static JsonElement? FindProperty (this JsonElement? element, params string [] nodes)
	{
		if (element is null)
			return null;
		return FindProperty (element.Value, nodes);
	}

	public static JsonElement? FindProperty (this JsonElement element, params string [] nodes)
	{
		foreach (var node in nodes) {
			if (element.ValueKind != JsonValueKind.Object)
				return null;

			if (!element.TryGetProperty (node, out element))
				return null;
		}
		return element;
	}

	public static bool TryGetProperty (this JsonDocument element, string propertyName, out JsonElement value)
	{
		value = default;

		if (element.RootElement.ValueKind != JsonValueKind.Object)
			return false;

		return element.RootElement.TryGetProperty (propertyName, out value);
	}

	public static IEnumerable<JsonElement> EnumerateIfArray (this JsonElement element)
	{
		if (element.ValueKind == JsonValueKind.Array) {
			foreach (var item in element.EnumerateArray ())
				yield return item;
		}
	}
}
