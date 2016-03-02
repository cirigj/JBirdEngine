namespace JBirdEngine {

	namespace RenUnity {

		// *****************************************************************************
		// * MOODS MUST BE ADDED TO THIS ENUM TO BE RECOGNIZED BY THE PARSER           *
		// *                                                                           *
		// * ~ Separate names with commas                                              *
		// * ~ Do not use spaces within mood names.                                    *
		// * ~ Leave 'InvalidMood = 0,' as the first entry                             *
		// *                                                                           *
		// * Example:                                                                  *
		// * public enum Mood {                                                        *
		// *     InvalidMood = 0,                                                      *
		// *     Neutral,                                                              *
		// *     Happy,                                                                *
		// *     Sad,                                                                  *
		// * }                                                                         *
		// *                                                                           *
		// * ~ It is good practice to leave a comma after the last mood, so you don't  *
		// *   forget when adding moods later.                                         *
		// * ~ Make sure to spell these moods EXACTLY the same way when you write      *
		// *   dialogue commands.                                                      *
		// * ~ DO NOT REORDER THESE NAMES, it will break any character data you may    *
		// *   have saved (re-naming is fine).                                         *
		// * ~ WARNING: Removing moods from the middle of the list will also           *
		// *   break and character data you may have saved.                            *
		// *****************************************************************************

		public enum Mood {
			InvalidMood = 0,
			Happy,
			Sad,
		}

	}

}