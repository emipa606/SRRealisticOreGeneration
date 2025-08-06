# GitHub Copilot Instructions for [SR]Realistic Ore Generation (Continued)

## Mod Overview and Purpose

[SR]Realistic Ore Generation (Continued) is a mod for RimWorld designed to overhaul the vanilla resource generation mechanics and enhance the functionality of long-range scanners. The mod introduces unique resource distributions for each world map tile, adding depth and realism to mining operations. It challenges players to engage with the world map to locate and extract scarce resources, enhancing the survival and technological development experience in RimWorld.

## Key Features and Systems

- **Unique Resource Distribution**: Each world map tile features distinct surface and underground ore distributions, viewable in a new world tile UI tab.
- **Long Range Scanner Enhancements**: Scans remote tiles for resource abundances, offering two modes—area scan and long-range single-tile scan.
- **Deep Scanner Modifications**: Removes endless resource generation, instead making underground resources exhaustible over time.
- **Mining Camps**: New caravan option to establish temporary mining camps for resource extraction.
- **Customization Settings**: Offers extensive mod settings to tailor resource abundances, lump sizes, and mining camp limits.

## Coding Patterns and Conventions

- **Class Prefixes**: Utilize descriptive class names with prefixes indicating their purpose, such as `Comp`, `WorldObjectComp` for component classes, or `WorldLayer` for world rendering classes.
- **Singleton Pattern**: Implement singleton design using `BaseSingleTon` for managing global or shared data across the mod.
- **Consistent Naming**: Follow C# naming conventions where class names are PascalCase and methods are camelCase.

## XML Integration

- **Mod Settings**: Uses XML for defining customizable settings, allowing seamless integration and adjustment of features via RimWorld's mod settings interface.
- **Compatibility with Custom Resources**: The mod automatically supports resources from other mods by dynamically retrieving resource definitions (`ThingDef`).

## Harmony Patching

- **Harmony Integration**: Use Harmony to patch vanilla classes, allowing the modification of existing methods (e.g., `HarmonyMethod`) without altering original game files.
- **PatchMain.cs**: Centralized patch application class ensuring efficient management of multiple patches.

## Suggestions for Copilot

- **Method Stubs**: Provide initial method stubs with comments describing their intended functionality to guide Copilot in generating relevant code.
- **Pattern Recognition**: Ensure consistent use of prefixes and design patterns in code to help Copilot better predict implementations based on existing structure.
- **Documentation Comments**: Include XML documentation for all public methods and classes to enable Copilot to utilize context for generating accurate suggestions.
- **Dynamic Resource Handling**: For scripts handling resources (e.g., `GenStep_ScatterLumpsMineable_Generate.cs`), suggest code that accommodates dynamic resources from other mods.
- **User Interface**: Guide Copilot to generate UI elements by providing example methods like `DrawSurfaceInfo` or `getScanAreaGizmo`.

## Conclusion

[SR]Realistic Ore Generation (Continued) transforms RimWorld's resource extraction experience. By integrating nuanced coding conventions, XML settings, and advanced Harmony patching, this mod leverages GitHub Copilot to maintain and enhance existing functionality efficiently. These instructions should guide future development and ensure continued compatibility and innovation.


This `.github/copilot-instructions.md` file offers a comprehensive guideline for using GitHub Copilot effectively in the development and maintenance of the RimWorld mod. It emphasizes critical aspects such as coding standards, XML integration, and suggestions for better AI-assisted coding.
