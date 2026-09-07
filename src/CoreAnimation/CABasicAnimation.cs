
#if MONOMAC
using AppKit;
#endif
using CoreGraphics;

#nullable enable

namespace CoreAnimation {
	public partial class CABasicAnimation {
		/// <summary>Returns the initial value for the property to animate, returned as an object of the specified type.</summary>
		/// <typeparam name="T">The type to return the value as.</typeparam>
		public T GetFromAs<T> () where T : class, INativeObject
		{
			return Runtime.GetINativeObject<T> (_From, false)!;
		}

		/// <summary>Sets the value for the initial value of the property to animate, by using a non-NSObject type.</summary>
		/// <param name="value">
		///   <para>Initial value that the property will have.</para>
		///   <para>If you want to set the value to null, use the From property.</para>
		/// </param>
		public void SetFrom (INativeObject value)
		{
			_From = value.Handle;
			GC.KeepAlive (value);
		}

		/// <summary>Returns the destination value for the property to animate, returned as an object of the specified type.</summary>
		/// <typeparam name="T">The type to return the value as.</typeparam>
		public T GetToAs<T> () where T : class, INativeObject
		{
			return Runtime.GetINativeObject<T> (_To, false)!;
		}

		/// <summary>Destination value for the property to animate (using INativeObject).</summary>
		/// <param name="value">
		///   <para>Final value that the property will have.</para>
		///   <para tool="nullallowed">If you want to set the value to null, use the property To.</para>
		/// </param>
		public void SetTo (INativeObject value)
		{
			_To = value.Handle;
			GC.KeepAlive (value);
		}

		/// <summary>Returns the value to increment by, returned as an object of the specified type.</summary>
		/// <typeparam name="T">The type to return the value as.</typeparam>
		public T GetByAs<T> () where T : class, INativeObject
		{
			return Runtime.GetINativeObject<T> (_By, false)!;
		}

		/// <summary>Sets the value to increment by, by using a non-NSObject type.</summary>
		/// <param name="value">The value to increment by.</param>
		public void SetBy (INativeObject value)
		{
			_By = value.Handle;
			GC.KeepAlive (value);
		}
	}
}
