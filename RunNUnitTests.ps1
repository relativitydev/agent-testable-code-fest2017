try {
    # Get all test dlls
    $nunit_dlls = Get-ChildItem -Recurse -Name -Filter *UnitTests.dll | Select-String -Pattern bin\\Debug | Sort-Object
	
	Write-Host "dll printed out : "  $($nunit_dlls)

    # Run each test dll
    ForEach ($nunit_dll in $nunit_dlls) {
         S:/NUnit-2.6.4/bin/nunit-console.exe $nunit_dlls
    }
}
catch {
    throw $_.exception
}