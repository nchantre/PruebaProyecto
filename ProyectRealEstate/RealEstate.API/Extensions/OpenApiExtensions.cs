namespace RealEstate.API.Extensions
{


    public static class OpenApiExtensions
    {
        public static IServiceCollection CustomAddOpenApi(this IServiceCollection services)
        {
            services.AddSwaggerGen();
            return services;
        }

        public static IApplicationBuilder CustomMapOpenApi(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
