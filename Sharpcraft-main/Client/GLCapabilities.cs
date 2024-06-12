using LWCSGL.OpenGL;

namespace SharpCraft.Client
{
    public class GLCapabilities
    {
        public bool enableOcclusionQuery = true;
        public bool GL_NV_fog_distance;
        public bool GL_ARB_occlusion_query;

        public GLCapabilities() 
        {
            //caching the results so don't need to look up the list contents within render loops
            GL_NV_fog_distance = Display.CheckGLExtension("GL_NV_fog_distance");
            GL_ARB_occlusion_query = enableOcclusionQuery && Display.CheckGLExtension("GL_ARB_occlusion_query");
        }
    }
}