namespace QuarkEngine.Core {
	
	public sealed class Quark {

		internal static Application? _currentApplication;
		public static Application? CurrentApplication => _currentApplication;
	}
}