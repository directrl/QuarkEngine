﻿using Newtonsoft.Json.Linq;
using Playground.Scenes;
using Monospace;
using Monospace.Configuration;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Window = Monospace.Graphics.Window;

namespace Playground {

	public class Program : Application {
		
		private Program() : base("playground") { }

		public override void Initialize() {
			var window = Window.Create();
			var scene = new EmptyScene();
			window.Scene = scene;

			Windows.Add(window);

			string sT = GameSettings.GetOrDefault("st", "hello");
			double dT = GameSettings.GetOrDefault("dt", 4.642);
			float fT = GameSettings.GetOrDefault("ft", 598.3129f);
			JObject a = new JObject();
			a["test"] = 3;
			a["ads"] = "sdasd";
			JObject joT = GameSettings.GetOrDefault("jot", a);
			byte bT = GameSettings.GetOrDefault("bt", (byte) 32);

			foreach(var binding in scene.KeyBindings.Bindings.Values) {
				Console.WriteLine(binding);
			}
		}

		public static void Main(string[] args) {
			var app = new Program();
			app.Start(args);
		}
	}
}