using RazorLight;

namespace Evenda.Infrastructure.Utils
{
    public class RazorViewToStringRenderer
    {
        private readonly RazorLightEngine _engine;

        public RazorViewToStringRenderer(string templateRootPath)
        {
            _engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(templateRootPath)
                .UseMemoryCachingProvider()
                .Build();
        }

        public async Task<string> RenderAsync<T>(string viewPath, T model)
        {
            return await _engine.CompileRenderAsync(viewPath, model);
        }
    }
}
