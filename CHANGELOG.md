## v1.0.8

Changes since v1.0.8:
## v1.0.8 (patch)

Changes since v1.0.7:

- Add ISerializationOptions interface and related policies for serialization configuration ([@matt-edmondson](https://github.com/matt-edmondson))
- Refactor ISerializationOptions interface to unify member serialization policies and enhance clarity. Introduce new properties for serialization and deserialization policies, and update related enums for improved configurability. ([@matt-edmondson](https://github.com/matt-edmondson))
## v1.0.7 (patch)

Changes since v1.0.6:

- Update global.json and Abstractions.csproj to use ktsu.Sdk version 1.60.0 and switch project SDK to Microsoft.NET.Sdk, improving compatibility with .NET 8.0. ([@matt-edmondson](https://github.com/matt-edmondson))
## v1.0.6 (patch)

Changes since v1.0.5:

- Refactor ISerializationProvider interface to use generic type parameters for deserialization methods, enhancing type safety and usability. Update CompatibilitySuppressions.xml to remove obsolete suppressions related to nullable attributes, ensuring compatibility with .NET 8.0. ([@matt-edmondson](https://github.com/matt-edmondson))
## v1.0.5 (patch)

Changes since v1.0.4:

- Update CompatibilitySuppressions.xml to reflect changes in diagnostic IDs and target methods for the ktsu.Abstractions library, enhancing compatibility with .NET 8.0. This includes updates for compression, encryption, hashing, and obfuscation methods, ensuring accurate suppression of diagnostics across versions. ([@matt-edmondson](https://github.com/matt-edmondson))
## v1.0.4 (patch)

Changes since v1.0.3:

- Enhance ktsu.Abstractions library by refining interface descriptions and adding zero-allocation Try methods for compression, encryption, hashing, obfuscation, and serialization. Update README to reflect these changes, emphasizing performance improvements and usage examples. ([@matt-edmondson](https://github.com/matt-edmondson))
## v1.0.3 (patch)

Changes since v1.0.2:

- Add System.Memory package reference and enhance interfaces in ktsu.Abstractions for better async support. Update README for clarity on usage and installation. ([@matt-edmondson](https://github.com/matt-edmondson))
- Update README to reflect changes in target frameworks and provide an example implementation of a custom MD5 hash provider, enhancing clarity on usage and functionality. ([@matt-edmondson](https://github.com/matt-edmondson))
- Refactor interfaces in ktsu.Abstractions to use Try methods for compression, encryption, hashing, obfuscation, and serialization, enhancing performance by reducing allocations. Update README to reflect these changes and clarify usage. ([@matt-edmondson](https://github.com/matt-edmondson))
## v1.0.2 (patch)

Changes since v1.0.1:

- Remove EnumOrderingAnalyzer project and related files from the solution, streamlining the project structure and eliminating unused analyzers. ([@matt-edmondson](https://github.com/matt-edmondson))
## v1.0.1 (patch)

Changes since v1.0.0:

- Add detailed README for ktsu.Abstractions library, outlining interfaces for compression, encryption, hashing, obfuscation, serialization, and filesystem access. Include installation instructions, quickstart examples, and contributing guidelines. ([@matt-edmondson](https://github.com/matt-edmondson))
- Remove obsolete abstraction models for compression, encryption, hashing, obfuscation, and filesystem types, along with global usings. This cleanup streamlines the project structure. ([@matt-edmondson](https://github.com/matt-edmondson))
## v1.0.0 (major)

- Initial commit ([@matt-edmondson](https://github.com/matt-edmondson))
- Remove outdated files and update project references to reflect the new repository name 'Abstractions'. Set version to 1.0.0 and clean up changelog, README, and tags. ([@matt-edmondson](https://github.com/matt-edmondson))
