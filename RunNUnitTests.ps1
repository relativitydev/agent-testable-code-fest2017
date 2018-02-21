try {
    # Get all test dlls
    $nunit_dlls = Get-ChildItem -Recurse -Name -Filter *UnitTests.dll | Select-String -Pattern bin\\Debug | Sort-Object
	
	echo $nunit_dlls

    # Run each test dll
    ForEach ($nunit_dll in $nunit_dlls) {
        'S:/NUnit-2.6.4/bin/nunit-console.exe' $nunit_dll
    }
}
catch {
    throw $_.exception
}