#Requires -Version 5.0

[CmdletBinding()]
param(
    [string[]]$taskList = @(),
	
	[ValidateSet("Debug","Release")]
	[string]$configuration = "Debug",
	
	[string]$targetEnvironment
)

  #set up variables
$BASE_DIR = Resolve-Path .
Write-Host "BASE_DIR resolves to: $BASE_DIR"

$NUGET_URL = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
Write-Host "nuget URL: $NUGET_URL"

$TOOLS_DIR = Join-Path $BASE_DIR "buildtools"
Write-Host "Tools directory: $TOOLS_DIR"

$TOOLS_PACKAGES_FILE = Join-Path $TOOLS_DIR "packages.config"
Write-Host "Packages config : $TOOLS_PACKAGES_FILE"

$LOGGER = Join-Path $TOOLS_DIR "Microsoft.Build.Logging.StructuredLogger.1.0.89\lib\net46\StructuredLogger.dll"

$TOOLS_PACKAGES_FILE = Join-Path $TOOLS_DIR "packages.config"
Write-Host "Nuget Pacakge File resolves to :  $TOOLS_PACKAGES_FILE"

$NUGET_EXE = Join-Path $TOOLS_DIR "nuget.exe"
Write-Host "$NUGET_EXE"

$NUNIT_EXE = Join-Path $TOOLS_DIR "NUnit.ConsoleRunner.3.6.0\tools\nunit3-console.exe"

$SOLUTION_PATH = Join-Path "RelativityAgent1\RelativityAgent1" -ChildPath "RelativityAgent.sln"

# Restore Nuget package
Write-Verbose "Checking for NuGet in tools path..."
if (-Not (Test-Path $NUGET_EXE -Verbose:$VerbosePreference)) {
    Write-Output "Installing NuGet from $NUGET_URL..."
    Invoke-WebRequest $NUGET_URL -OutFile $NUGET_EXE -Verbose:$VerbosePreference -ErrorAction Stop
}

Write-Output "Restoring tools from NuGet..."
Write-Verbose "Using $TOOLS_PACKAGES_FILE..."
& $NUGET_EXE install $TOOLS_PACKAGES_FILE -o $TOOLS_DIR

if ($LASTEXITCODE -ne 0) {
	Throw "An error occured while restoring NuGet tools."
}


# Build the solution

# Import any modules that you'll need to build
Import-Module (Join-Path $TOOLS_DIR "psake.4.6.0\tools\psake.psm1") -ErrorAction Stop

# Execute the build
Invoke-PSake "default.ps1" `
	-parameters @{	'root' = $BASE_DIR;
					'tools_dir' = $TOOLS_DIR;
					'nuget_exe' = $NUGET_EXE;  
					'nunit_exe' = $NUNIT_EXE;
					'logger' = $LOGGER }`
	-properties @{	'build_config' = $configuration;
					'target_environment' = $targetEnvironment }`
	-nologo `
	-taskList $taskList `
	-Verbose:$VerbosePreference `
	-Debug:$DebugPreference

exit ( [int]( -not $psake.build_success ) )