// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Sharpie.Bind;

public static class AnnotatableImpl {
	class AnnotationsList : List<object> {
	}

	static ConditionalWeakTable<object, AnnotationsList> annotationsTable = new ConditionalWeakTable<object, AnnotationsList> ();

	public static IEnumerable<object?> GetAnnotations (this object annotatable)
	{
		return GetAnnotations<object> (annotatable);
	}

	public static IEnumerable<T?> GetAnnotations<T> (this object annotatable)
	{
		if (!annotationsTable.TryGetValue (annotatable, out var annotations))
			yield break;

		foreach (var annotation in annotations) {
			if (annotation is T ann)
				yield return ann;
		}
	}

	public static void AddAnnotation (this object annotatable, object annotation)
	{
		if (annotatable is null)
			throw new ArgumentNullException (nameof (annotatable));

		if (!annotationsTable.TryGetValue (annotatable, out var annotations))
			annotationsTable.Add (annotatable, annotations = new AnnotationsList ());

		annotations.Add (annotation);
	}

	public static T? Annotation<T> (this object annotatable)
	{
		return annotatable.GetAnnotations<T> ().SingleOrDefault ();
	}
}
