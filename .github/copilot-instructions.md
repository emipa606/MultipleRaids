# Copilot Instructions for MultipleRaids (Continued)

## Mod Overview and Purpose

**MultipleRaids (Continued)** is an exciting mod designed for RimWorld players seeking to increase the challenge of their gameplay. This mod allows multiple raid events to occur simultaneously without altering the frequency of raids or other events. It's perfect for those who find handling a single raid too simple and want to test their strategic skills by managing several raids at once.

## Key Features and Systems

1. **Multiple Simultaneous Raids:**
   - Players can face 2, 3, or even more raids at the same time.
   - Raid difficulty adjusts as more raids spawn, distributing points among them.

2. **Customizable Mod Variables:**
   - **Extra Raids:** Set the maximum number of additional raids. Affects point distribution.
   - **Spawn Threshold:** Determines the probability of spawning additional raids.
   - **Points Offset:** Adjusts the scaling of raid points.
   - **Force Desperate:** Allows varied raider types in extreme environments.
   - **Force Raid Type:** Changes some raids to specific types (e.g., siege).
   - **Random Factions:** Enables diversity by allowing raids from different factions.

3. **Compatibility:**
   - Works with existing saves and storytellers, given the mod is loaded last.

4. **Migration and Localization:**
   - Moves settings from old HugsLib, opening them for localization and translation.

## Coding Patterns and Conventions

- **C# Project Structure:**
  - Organized into clear classes (e.g., `MultipleRaids`, `IncidentWorker_RaidEnemy_Custom`).
  - Methods are logically grouped by functionality (e.g., `TryExecuteWorker` handles raid execution logic).

- **Naming Conventions:**
  - Use PascalCase for classes and methods.
  - Use descriptive names that convey functionality, such as `FactionCanBeGroupSource`.

## XML Integration

- **Purpose:** XML is used primarily for defining incident patches and translation templates.
- **Files:**
  - Modification of `Incidents_Map_Threats.xml` to integrate raid events.
  - Translation templates are present for multi-language support.

## Harmony Patching

- **Dependency on Harmony:** Essential for patching the existing `RaidEnemy` incident class.
- **Patch Types:**
  - **Postfix and Prefix Methods:** These are used to modify the behavior of existing methods without altering the original source code.
  - For instance, `IncidentWorker_RaidEnemy_FactionCanBeGroupSource` uses a `Prefix` to change faction selection logic.

## Suggestions for Copilot

- **Code Suggestions:**
  - Code suggestions should focus on enhancing the raid spawning logic, adjusting factors such as raid point distribution and spawn tactics.

- **XML Suggestions:**
  - Focus on expanding translation templates to support more languages.

- **Harmony Patches:**
  - Propose new patch methods for additional improvements, such as customizing raid strategies or behavior directly via Harmony.

- **Localization Enhancements:**
  - Encourage suggestions to streamline translation integration and mod setting localization.

By following these instructions, Copilot can assist in maintaining and improving the MultipleRaids mod, ensuring it continues to enhance the experience for RimWorld players who seek increased difficulty and strategic complexity.

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

