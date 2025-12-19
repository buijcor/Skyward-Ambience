# Building Skyward Ambience with Visual Studio

## Prerequisites

1. **Visual Studio 2017 or later** (Community edition is free)
   - Make sure you have ".NET desktop development" workload installed
   - Or install .NET Framework 4.7.2 Developer Pack

2. **RimWorld Installation**
   - RimWorld should be installed at: `C:\RimWorld.v1.6.4633_LinkNeverDie.Com`
   - If your RimWorld is in a different location, you'll need to update the `RimWorldPath` property in the project file

3. **Harmony Mod**
   - Harmony mod should be installed in the GSE mods folder
   - Expected path: `C:\RimWorld.v1.6.4633_LinkNeverDie.Com\LinkNeverDie.Com-GSE\mods\2009463077\Current\Assemblies\0Harmony.dll`

## Building Steps

### Method 1: Using Visual Studio GUI

1. **Open the Solution**
   - Double-click `SkyAmbient.sln` in the mod folder
   - Or open Visual Studio → File → Open → Project/Solution → Navigate to `SkyAmbient.sln`

2. **Verify Project Settings**
   - Right-click on `SkywardAmbience` project in Solution Explorer
   - Select Properties
   - Check that:
     - Target framework is `.NET Framework 4.7.2`
     - Output path is `..\Assemblies\` (relative to Source folder)
     - Configuration is set to `Debug` or `Release`

3. **Build the Project**
   - Press `Ctrl+Shift+B` (or Build → Build Solution)
   - Or right-click the project → Build
   - Check the Output window for any errors

4. **Verify Output**
   - After successful build, check that `SkywardAmbience.dll` appears in the `Assemblies\` folder
   - The DLL should be at: `Mods\SkyAmbient\Assemblies\SkywardAmbience.dll`

### Method 2: Using Command Line (MSBuild)

1. **Open Developer Command Prompt for VS**
   - Search for "Developer Command Prompt for VS" in Windows Start menu

2. **Navigate to Source Folder**
   ```cmd
   cd "C:\RimWorld.v1.6.4633_LinkNeverDie.Com\Mods\SkyAmbient\Source"
   ```

3. **Build Debug Version**
   ```cmd
   msbuild SkywardAmbience.csproj /p:Configuration=Debug
   ```

4. **Build Release Version**
   ```cmd
   msbuild SkywardAmbience.csproj /p:Configuration=Release
   ```

### Method 3: Using Visual Studio Code (with C# extension)

1. **Open the Source Folder**
   - Open VS Code in the `Source` folder
   - Install C# extension if not already installed

2. **Build**
   - Press `Ctrl+Shift+P`
   - Type "Tasks: Run Task"
   - Select "build" or create a build task

## Troubleshooting

### Error: Cannot find Harmony DLL
- **Solution**: Verify that Harmony mod is installed at the expected path
- Check: `C:\RimWorld.v1.6.4633_LinkNeverDie.Com\LinkNeverDie.Com-GSE\mods\2009463077\Current\Assemblies\0Harmony.dll`
- If Harmony is in a different location, update the `HintPath` in `SkywardAmbience.csproj`

### Error: Cannot find Assembly-CSharp.dll
- **Solution**: Verify RimWorld installation path
- Check: `C:\RimWorld.v1.6.4633_LinkNeverDie.Com\RimWorldWin64_Data\Managed\Assembly-CSharp.dll`
- If RimWorld is elsewhere, update the `RimWorldPath` property in the project file

### Error: Target framework not found
- **Solution**: Install .NET Framework 4.7.2 Developer Pack
- Download from: https://dotnet.microsoft.com/download/dotnet-framework/net472

### Build Succeeds but DLL Not in Assemblies Folder
- **Solution**: Check the Output window for the actual output path
- Verify the `OutputPath` property in project properties matches `..\Assemblies\`

## Build Configurations

- **Debug**: Includes debug symbols, easier to debug, larger file size
- **Release**: Optimized, smaller file size, no debug symbols

For distribution, use **Release** configuration.

## After Building

1. Verify `SkywardAmbience.dll` is in `Mods\SkyAmbient\Assemblies\`
2. The mod structure should be:
   ```
   SkyAmbient/
   ├── About/
   │   └── About.xml
   ├── Assemblies/
   │   └── SkywardAmbience.dll  ← This should exist after build
   ├── Defs/
   │   └── MapComponents.xml
   └── Source/
       └── (source files)
   ```

3. Test in RimWorld by enabling the mod in the mod menu

