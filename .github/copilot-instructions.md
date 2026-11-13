# GitHub Copilot Instructions for [SR]Realistic Ore Generation (Continued)

## Mod Overview and Purpose
[SR]Realistic Ore Generation (Continued) is a RimWorld mod that overhauls the vanilla resource generation mechanics. It provides a unique resource distribution for each world map tile, both at the surface and underground, enhancing the game's realism and depth. The mod also introduces a new world UI tile info tab to display these resource distributions. Additionally, it modifies the long range scanner to allow players to scan remote tiles for resource abundance and changes the deep scanner to make underground resources exhaustible.

## Key Features and Systems
### Features
- **Unique Resource Distribution**: Each world map tile has its own unique resource distribution, displayed in a new world tile UI tab.
- **Diverse Resource Abundances**: Some tiles are resource-rich, while others have little resources.
- **Long Range Scanner**: Can scan remote tiles for surface and underground resource abundance, with two scan modes: area scan and single tile mode.
- **Exhaustible Underground Resources**: Deep scanner no longer generates unlimited resource lumps; scan times increase dramatically after a certain number of cycles, providing a soft cap for resource generation.
- **Mining Camps**: New option for caravans to set up temporary mining camps.

### Systems
- **Resource Generation**: Customizable multipliers for total resource abundances and randomization of vanilla resource abundances.
- **UI Enhancements**: New world tile UI tab for resource distribution information.
- **Long Range Scanner**: Enhanced functionality to scan remote tiles.
- **Deep Scanner**: Modified to make underground resources exhaustible.

## Coding Patterns and Conventions
- **Harmony Patching**: The mod extensively uses Harmony patches to modify vanilla RimWorld behavior. Patches are located in the `HarmonyPatches` directory.
- **XML Integration**: XML files are used to define new world objects and map generators. These files are located in the `Defs` and `Patches` directories.
- **Consistent Naming**: Classes and methods follow a consistent naming convention to maintain readability and maintainability.
- **Modular Design**: The mod is designed in a modular fashion, with different systems encapsulated in separate classes and files.

## XML Integration
The mod uses XML files to define new world objects and map generators. Here are some examples:
- **MiningOutpostMapGenerator.xml**: Defines a new map generator for mining outposts.
- **WorldObjects.xml**: Defines new world objects, such as mining camps.
- **PatchBuildingsMisc.xml**: Contains patches for building-related functionality.
- **PatchWorldObjects.xml**: Contains patches for world object-related functionality.

## Harmony Patching
The mod uses Harmony patches to modify vanilla RimWorld behavior. Here are some examples:
- **CompDeepScanner_ChooseLumpThingDef.cs**: Prefix patch to modify the deep scanner's behavior.
- **World_ExposeData.cs**: Postfix patch to modify world data exposure.
- **WorldInspectPane_CurTabs.cs**: Postfix patch to modify world inspect pane tabs.
- **GenStep_ScatterLumpsMineable_Generate.cs**: Prefix patch to modify resource generation.

## Suggestions for Copilot
1. **Focus on Harmony Patching**: When generating code for Harmony patches, focus on the specific behavior you want to modify in vanilla RimWorld.
2. **Use XML Integration**: When generating XML files, ensure that the definitions are consistent with the mod's design and functionality.
3. **Follow Coding Patterns**: When writing C# code, follow the mod's coding patterns and conventions, such as consistent naming and modular design.
4. **Test Thoroughly**: After generating any code, thoroughly test it to ensure it works as expected and does not cause any issues with the mod or vanilla RimWorld.

By following these guidelines, you can effectively use GitHub Copilot to generate code for your RimWorld mod, ensuring that it is consistent with the mod's design and functionality.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

