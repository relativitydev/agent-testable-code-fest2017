FormatTaskName "------- Executing Task: {0} -------"
Framework "4.6" #.NET framework version

try {
  #set up variables
$BASE_DIR = Resolve-Path .
Write-Host "BASE_DIR resolves to: $BASE_DIR"

$NUGET_URL = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
$TOOLS_PACKAGES_FILE = Join-Path "RelativityAgent1\RelativityAgent1" -ChildPath "packages.config"
Write-Host "Packages config : $TOOLS_PACKAGES_FILE"

$TOOLS_PACKAGES_FILE = Join-Path $TOOLS_DIR "packages.config"
Write-Host "Nuget Pacakge File resolves to :  $TOOLS_PACKAGES_FILE"

$NUNIT_EXE = "S:/nuget.exe"
Write-Host "$NUNIT_EXE"

$MSBUILD_EXE = "C:/Program Files (x86)/Microsoft Visual Studio/2017/Professional/MSBuild/15.0/Bin/MSBuild.exe"
$SOLUTION_PATH = Join-Path "RelativityAgent1\RelativityAgent1" -ChildPath "RelativityAgent.sln"

# Restore Nuget package
Write-Output "Restoring tools from NuGet..."
Write-Verbose "Using $TOOLS_PACKAGES_FILE..."
& $NUNIT_EXE install $TOOLS_PACKAGES_FILE -o $TOOLS_DIR

# Build the solution
exec $MSBUILD_EXE $SOLUTION_PATH

}
catch {
    throw $_.exception
}