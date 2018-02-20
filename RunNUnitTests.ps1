try {
    # Get all test dlls
    $nunit_dlls = Get-ChildItem -Recurse -Name -Filter *test.dll | Select-String -Pattern bin\\x64\\Release | Sort-Object

    # Run each test dll
    ForEach ($nunit_dll in $nunit_dlls) {
        $xml_results = [System.IO.Path]::GetRandomFileName().Split(".")[0] + ".xml"
        & "C:/Program Files (x86)/NUnit.org/nunit-console/nunit3-console.exe" $nunit_dll --result=$xml_results`;transform="./Artifacts/Scripts/Testing/nunit3-junit.xslt"
    }
}
catch {
    throw $_.exception
}