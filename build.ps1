(Get-Content .\src\CommonAssemblyInfo.cs -Raw) -match 'AssemblyVersion\("([^"]+)"\)';
$version = $matches[1]

msbuild .\src\TomatoDoer.sln /target:rebuild /p:configuration=release
$archiveName = "tomatodoer.$version.zip"
if(Test-Path $archiveName) {
	del $archiveName
}
7za a $archiveName .\src\TomatoDoer\bin\Release\*
