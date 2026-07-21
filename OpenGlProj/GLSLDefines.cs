namespace OpenGlProj
{
    static class GLSLDefines
    {
        public static string shaderVert = """
        #version 410 core
        layout(location = 0) in vec3 aPosition;

        layout(location = 1) in vec2 aTexCoord;

        out vec2 texCoord;

        void main(void)
        {
            texCoord = aTexCoord;

            gl_Position = vec4(aPosition, 1.0);
        }
        """;

        public static string shaderFrag = """
        #version 410 core

        out vec4 FragColor;

        in vec2 texCoord;

        uniform sampler2D texture1;
        uniform sampler2D texture2;

        void main()
        {
            FragColor = mix(texture(texture1, texCoord), texture(texture2, texCoord), 0.2);
        }
        """;
    }
}
