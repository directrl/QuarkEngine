using System.Numerics;
using Silk.NET.Core;
using Silk.NET.OpenGL;

namespace Monospace.Graphics.OpenGL {
	
	public class ShaderProgram : IDisposable {

		internal readonly GL gl;
		internal bool ready = false;
		
		private Shader[] Program { get; }
		public uint Id { get; }

		public ShaderProgram(GL gl, params Shader[] program) {
			Id = gl.CreateProgram();

			if(Id == 0) {
				throw new PlatformException("Could not create a GL shader program");
			}

			this.gl = gl;
			Program = program;
		}
		
		public unsafe void SetUniform(string name, Matrix4x4 value) {
			int location = gl.GetUniformLocation(Id, name);
			if(location < 0) throw new PlatformException($"Could not find the uniform location for {name}");
			
			gl.UniformMatrix4(location, 1, false, (float*) &value);
		}

		public void Build() {
			List<uint> shaderIds = new();

			foreach(var shader in Program) {
				var shaderId = shader.Compile(gl);

				if(shaderId != 0) {
					gl.AttachShader(Id, shaderId);
					shaderIds.Add(shaderId);
				}
			}

			gl.LinkProgram(Id);

			if(gl.GetProgram(Id, GLEnum.LinkStatus) == 0) {
				throw new LinkingException(gl, Id);
			}
			
			// cleanup
			foreach(var shaderId in shaderIds) {
				gl.DetachShader(Id, shaderId);
				gl.DeleteShader(shaderId);
			}

			ready = true;
		}

		public bool Validate() {
			gl.ValidateProgram(Id);
			return gl.GetProgram(Id, GLEnum.ValidateStatus) != 0;
		}

		public void Bind() {
			if(!ready) Build();
			gl.UseProgram(Id);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			
			if(!ready) return;
			gl.DeleteProgram(Id);
		}

		public class LinkingException : Exception {
			
			public LinkingException(GL gl, uint id)
				: base($"Error occured during program linking: {gl.GetProgramInfoLog(id)}") { }
		}
	}
}