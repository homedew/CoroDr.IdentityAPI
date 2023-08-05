using CoroDr.IdentityAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

public static class Extension
{
	public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
	{
		static void ConfigurationSqlOptions(SqlServerDbContextOptionsBuilder sqlOption)
		{
			sqlOption.MigrationsAssembly(typeof(Program).Assembly.FullName);
			sqlOption.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);


		}

		services.AddDbContext<IdentityDbContext>(option =>
		{
			var connectionString = configuration.GetRequiredConnectionString("Identity");
			option.UseSqlServer(connectionString, ConfigurationSqlOptions);
		});


		return services;
	}
}

