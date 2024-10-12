using System.Reflection;

namespace Monospace {

	public class Resource {
		
		private Assembly Assembly { get; }
		public ResourceType Type { get; }
		public string Namespace { get; }
		public string Name { get; }

		public Resource(ResourceType type, string _namespace, string name, Assembly? assembly = null) {
			if(assembly == null) {
				Assembly = Assembly.GetCallingAssembly();
			} else {
				Assembly = assembly;
			}
			
			Type = type;
			Namespace = _namespace;
			Name = name;
		}

		public Stream? GetStream() {
			return Assembly
				.GetManifestResourceStream($"{Namespace}.{Type.Path}.{Name}{Type.Extension}");
		}

		public byte[]? ReadBytes() {
			using(var stream = GetStream()) {
				if(stream == null) return null;
				
				using(var memoryStream = new MemoryStream()) {
					stream.CopyTo(memoryStream);
					return memoryStream.ToArray();
				}
			}
		}

		public string? ReadString() {
			using(var stream = GetStream()) {
				if(stream == null) return null;
				
				using(var reader = new StreamReader(stream)) {
					return reader.ReadToEnd();
				}
			}
		}
	}
	
	public class ResourceType {
		
		public static readonly ResourceType SHADER = new("Shaders", "");
			
		public string Path { get; }
		public string Extension { get; }

		ResourceType(string path, string extension) {
			Path = path;
			Extension = extension;
		}
	}
	
	public class Resources {

		private Assembly Assembly { get; }
		public string Namespace { get; }
		
		public Resources(string _namespace) {
			Assembly = Assembly.GetCallingAssembly();
			Namespace = _namespace;
		}

		public Resource Get(ResourceType type, string name) {
			return new(type, Namespace, name, Assembly);
		}

		public Resource this[ResourceType type, string name]
			=> new(type, Namespace, name, Assembly);
	}
}