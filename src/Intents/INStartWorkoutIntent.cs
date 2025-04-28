#if IOS
#if !__MACOS__

using Foundation;
using Intents;
using ObjCRuntime;

#nullable enable

namespace Intents {

	public partial class INStartWorkoutIntent {

		/// <param name="workoutName">To be added.</param>
		///         <param name="goalValue">To be added.</param>
		///         <param name="workoutGoalUnitType">To be added.</param>
		///         <param name="workoutLocationType">To be added.</param>
		///         <param name="isOpenEnded">To be added.</param>
		///         <summary>To be added.</summary>
		///         <remarks>To be added.</remarks>
		public INStartWorkoutIntent (INSpeakableString workoutName, NSNumber goalValue, INWorkoutGoalUnitType workoutGoalUnitType, INWorkoutLocationType workoutLocationType, bool? isOpenEnded) :
			this (workoutName, goalValue, workoutGoalUnitType, workoutLocationType, isOpenEnded.HasValue ? new NSNumber (isOpenEnded.Value) : null)
		{
		}
	}
}

#endif // !__MACOS__
#endif
