namespace JBirdEngine {

	namespace RenUnity {

		// *****************************************************************************
		// * STATS MUST BE ADDED TO THIS ENUM TO BE RECOGNIZED BY THE PARSER           *
		// *                                                                           *
		// * ~ Separate names with commas                                              *
		// * ~ Do not use spaces within stat names.                                    *
		// * ~ Leave 'InvalidStat = 0,' as the first entry                             *
		// *                                                                           *
		// * Example:                                                                  *
		// * public enum Stat {                                                        *
		// *     InvalidStat = 0,                                                      *
		// *     Happiness,                                                            *
		// *     Trust,                                                                *
		// *     Loyalty,                                                              *
		// * }                                                                         *
		// *                                                                           *
		// * ~ It is good practice to leave a comma after the last stat, so you don't  *
		// *   forget when adding stats later.                                         *
		// * ~ Make sure to spell these stats EXACTLY the same way when you write      *
		// *   dialogue commands.                                                      *
		// * ~ DO NOT REORDER THESE NAMES, it will break any character data you may    *
		// *   have saved (re-naming is fine).                                         *
		// * ~ WARNING: Removing characters from the middle of the list will also      *
		// *   break and character data you may have saved.                            *
		// *****************************************************************************

		public enum Stat {
			InvalidStat = 0,
			Suspicion,
			Loyalty,
		}

	}

}