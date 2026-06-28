// Copyright (c) ktsu.dev
// All rights reserved.
// Licensed under the MIT license.

namespace ktsu.Abstractions;

using System.IO;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Defines a contract for configuration providers that can load and save strongly-typed configuration objects
/// to and from text-based formats such as JSON, YAML, or TOML.
/// </summary>
public interface IConfigurationProvider
{
	/// <summary>
	/// Tries to load a configuration object from the specified source.
	/// </summary>
	/// <typeparam name="T">The type of configuration object to load.</typeparam>
	/// <param name="source">The text reader containing the configuration data.</param>
	/// <param name="config">When this method returns, contains the loaded configuration if successful, or the default value if not.</param>
	/// <returns>True if the configuration was loaded successfully, false otherwise.</returns>
	public bool TryLoad<T>(TextReader source, out T? config);

	/// <summary>
	/// Tries to save a configuration object to the specified destination.
	/// </summary>
	/// <typeparam name="T">The type of configuration object to save.</typeparam>
	/// <param name="config">The configuration object to save.</param>
	/// <param name="destination">The text writer to write the configuration data to.</param>
	/// <returns>True if the configuration was saved successfully, false otherwise.</returns>
	public bool TrySave<T>(T config, TextWriter destination);

	/// <summary>
	/// Loads a configuration object from the specified text reader.
	/// </summary>
	/// <typeparam name="T">The type of configuration object to load.</typeparam>
	/// <param name="source">The text reader containing the configuration data.</param>
	/// <returns>The loaded configuration object, or default if loading fails.</returns>
	public T? Load<T>(TextReader source)
	{
		Ensure.NotNull(source);

		return TryLoad(source, out T? config) ? config : default;
	}

	/// <summary>
	/// Loads a configuration object from the specified string content.
	/// </summary>
	/// <typeparam name="T">The type of configuration object to load.</typeparam>
	/// <param name="content">The string containing the configuration data.</param>
	/// <returns>The loaded configuration object, or default if loading fails.</returns>
	public T? Load<T>(string content)
	{
		Ensure.NotNull(content);

		using StringReader reader = new(content);
		return Load<T>(reader);
	}

	/// <summary>
	/// Saves a configuration object and returns the serialized result.
	/// </summary>
	/// <typeparam name="T">The type of configuration object to save.</typeparam>
	/// <param name="config">The configuration object to save.</param>
	/// <returns>A string containing the serialized configuration.</returns>
	public string Save<T>(T config)
	{
		using StringWriter writer = new();
		_ = TrySave(config, writer);
		return writer.ToString();
	}

	/// <summary>
	/// Saves a configuration object to the specified text writer.
	/// </summary>
	/// <typeparam name="T">The type of configuration object to save.</typeparam>
	/// <param name="config">The configuration object to save.</param>
	/// <param name="destination">The text writer to write the configuration data to.</param>
	public void Save<T>(T config, TextWriter destination)
		=> TrySave(config, destination);

	/// <summary>
	/// Loads a configuration object from the specified string content asynchronously.
	/// </summary>
	/// <typeparam name="T">The type of configuration object to load.</typeparam>
	/// <param name="content">The string containing the configuration data.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The loaded configuration object, or default if loading fails.</returns>
	public Task<T?> LoadAsync<T>(string content, CancellationToken cancellationToken = default)
		=> ProviderHelpers.RunAsync(() => Load<T>(content), cancellationToken);

	/// <summary>
	/// Saves a configuration object and returns the serialized result asynchronously.
	/// </summary>
	/// <typeparam name="T">The type of configuration object to save.</typeparam>
	/// <param name="config">The configuration object to save.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A string containing the serialized configuration.</returns>
	public Task<string> SaveAsync<T>(T config, CancellationToken cancellationToken = default)
		=> ProviderHelpers.RunAsync(() => Save(config), cancellationToken);
}
