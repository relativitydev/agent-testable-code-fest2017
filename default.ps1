FormatTaskName "------- Executing Task: {0} -------"
Framework "4.6" #.NET framework version

properties {
	$build_config = "Debug"
	$verbosity = "normal" # q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]
	$build_artifacts = Join-Path $root "Artifacts"
	$test_logs = Join-Path $build_artifacts "TestLogs"
	$build_logs = Join-Path $build_artifacts "BuildLogs"
	$solution = Join-Path $root "RelativityAgent1\RelativityAgent.sln"  
}

task default -Depends LocalBuild
task LocalBuild -Depends Compile, UnitTest, IntegrationTest

task NuGetRestore -Description "Restore NuGet packages for the solution" {
	Write-Host "Solution :  $solution"
	exec { & $nuget_exe @('restore', $solution) }
}


task CompileInitialize -Description "Cleanup pre-existing build artifacts, if they exist" {
	InitializeDirectory $build_logs
}

task TestInitialize -Description "Cleanup pre-existing test artifacts, if they exist"{
	InitializeDirectory $test_logs
}


task Compile -Depends CompileInitialize, NuGetRestore -Description "Compile the solution" {
	Write-Verbose "Configuration: $build_config"
	Write-Verbose "Verbosity: $verbosity"

	# https://msdn.microsoft.com/en-us/library/ms164311.aspx
	exec { msbuild @($solution,
					("/property:Configuration=$build_config")
					("/verbosity:$verbosity"),
					('/nologo'),
					('/maxcpucount'),
					('/nodeReuse:false'),
					('/distributedfilelogger'),
					("/flp:LogFile=$build_logs\build.log"),
					("/flp1:warningsonly;LogFile=$build_logs\buildwarnings.log"),
					("/flp2:errorsonly;LogFile=$build_logs\builderrors.log"),
					("/logger:StructuredLogger,$logger;$build_logs\structured.buildlog"))
	}
}

task UnitTest -Alias Test -Depends TestInitialize -Description "Run NUnit unit tests" {
	exec { & $nunit_exe $solution --where "class=~/^.+\.UnitTests\..+$/" --result="$test_logs\UnitTests.xml;format=nunit2" } -errorMessage "Unit tests failed!"
}

task IntegrationTest -Depends TestInitialize -Description "Run NUnit integration unit tests. " {
	$testDir = Join-Path $root "RelativityAgent1\AgentNunitIntegrationTest"
	Write-Verbose "Test directory is : $testDir"
	Write-Host "Test directory is : $testDir"
	#$configSource = Join-Path $root "JenkinsApp.config"
	$configSource = "C:/smoketest.config"
	Write-Verbose "configSource is : $configSource"
	Write-Host "configSource is : $configSource"
	$configDestination = Join-Path $root "RelativityAgent1\AgentNunitIntegrationTest\bin\Debug\AgentNunitIntegrationTest.dll.config"
	Write-Verbose "configDestination is : $configDestination"
	Write-Host "configDestination is : $configDestination"
	Write-Host "Nunit exe is here : $nunit_exe"
	$testAssembly = Join-Path $root "RelativityAgent1\AgentNunitIntegrationTest\bin\Debug\AgentNunitIntegrationTest.dll"
	Write-Host "Test assembly : $testAssembly"

	Copy-Item $configSource $configDestination -Verbose:$VerbosePreference

	#exec { & $nunit_exe $solution --where "cat == testtype.universal && class=~/^.+\.AgentIntegrationTest\..+$/" --result="$test_logs\IntegrationTests.xml;format=nunit2" } -errorMessage "Integration tests failed!"
	exec { & $nunit_exe $testAssembly --result="$test_logs\IntegrationTests.xml;format=nunit2" } -errorMessage "Integration tests failed!"
}
	

Function InitializeDirectory($directory) {
	If (Test-Path $directory) {
		Remove-Item -Force -Recurse $directory
	}
	New-Item -ItemType Directory -Force -Path $directory
}
