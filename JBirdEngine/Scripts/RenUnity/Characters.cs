namespace JBirdEngine {

	namespace RenUnity {

		// *****************************************************************************
		// * CHARACTER NAMES MUST BE ADDED TO THIS ENUM TO BE RECOGNIZED BY THE PARSER *
		// *                                                                           *
		// * ~ Separate names with commas                                              *
		// * ~ Do not use spaces within character names.                               *
		// * ~ Leave 'InvalidName = 0,' as the first entry                             *
		// *                                                                           *
		// * Example:                                                                  *
		// * public enum Character {                                                   *
		// *     InvalidName = 0,                                                      *
		// *     Jeff,                                                                 *
		// *     JohnSmith,                                                            *
		// *     SomeGuy,                                                              *
		// * }                                                                         *
		// *                                                                           *
		// * ~ It is good practice to leave a comma after the last name, so you don't  *
		// *   forget when adding names later.                                         *
		// * ~ Make sure to spell these names EXACTLY the same way when you write      *
		// *   dialogue commands.                                                      *
		// * ~ DO NOT REORDER THESE NAMES, it will break any character data you may    *
		// *   have saved (re-naming is fine).                                         *
		// * ~ WARNING: Removing characters from the middle of the list will also      *
		// *   break and character data you may have saved.                            *
		// *****************************************************************************

		public enum Character {
			InvalidName = 0,
			Example,
			JohnCena,
		}

	}

}