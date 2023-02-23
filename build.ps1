param (
   [string]$config = $(throw "-config is required."),
   [switch]$sign = $false
)

& "$env:ProgramFiles\Microsoft Visual Studio\2022\Community\Common7\Tools\Launch-VsDevShell.ps1"
if ((Get-Command "msbuild.exe" -ErrorAction SilentlyContinue) -eq $null) 
{ 
   Write-Host "msbuild.exe not found in PATH."
   exit 1
}
Set-Location -Path $PSScriptRoot

# get the version
[xml]$commonbuildinfo = Get-Content Directory.Build.props
$publishversion = $commonbuildinfo.Project.PropertyGroup.Version

dotnet clean --configuration $config

# build
dotnet build --configuration $config

# tests
dotnet test StopSnooze.Tests\StopSnooze.Tests.csproj -c $config --no-build

# Remove old stuff
Remove-Item -path "$PSScriptRoot\Publish\win-arm64" -recurse -ErrorAction SilentlyContinue
Remove-Item -path "$PSScriptRoot\Publish\win-x64" -recurse -ErrorAction SilentlyContinue

# Publish x64/arm64
# (publishing the solution because StopSnooze.Runner uses the solution name as the exe name)
dotnet publish StopSnooze.sln /p:Configuration=$config /p:PublishProfile=Folder_win-x64
dotnet publish StopSnooze.sln /p:Configuration=$config /p:PublishProfile=Folder_win-arm64

# sign the executables if -sign was specified
if ($sign -eq $true)
{
   signtool.exe sign /n "Open Source Developer, Sherman Chan" /t http://time.certum.pl /fd sha256 /v "Publish\win-arm64\*.exe" "Publish\win-x64\*.exe"
}

# Compute hashes
certutil -hashfile Publish\win-arm64\StopSnooze.exe SHA512 | tee Publish\win-arm64\StopSnooze.exe.sha512
certutil -hashfile Publish\win-x64\StopSnooze.exe SHA512 | tee Publish\win-x64\StopSnooze.exe.sha512

# Create the archives
Compress-Archive -Path "Publish\win-arm64\*" -DestinationPath "Publish\win-arm64\StopSnooze_$publishversion`_win-arm64.zip"
Compress-Archive -Path "Publish\win-x64\*" -DestinationPath "Publish\win-x64\StopSnooze_$publishversion`_win-x64.zip"
