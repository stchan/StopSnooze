if ((Get-Command "msbuild.exe" -ErrorAction SilentlyContinue) -eq $null) 
{ 
   Write-Host "msbuild.exe not found in PATH."
   exit 1
}
Set-Location -Path $PSScriptRoot

# get the version
[xml]$commonbuildinfo = Get-Content Directory.Build.props
$publishversion = $commonbuildinfo.Project.PropertyGroup.Version
echo $publishversion

# Remove old stuff
Remove-Item -path "$PSScriptRoot\Publish\win-arm64" -recurse -ErrorAction SilentlyContinue
Remove-Item -path "$PSScriptRoot\Publish\win-x64" -recurse -ErrorAction SilentlyContinue

# Publish x64/arm64
dotnet publish StopSnooze.sln /p:Configuration=Release /p:PublishProfile=Folder_win-x64
dotnet publish StopSnooze.sln /p:Configuration=Release /p:PublishProfile=Folder_win-arm64

# Compute hashes
certutil -hashfile Publish\win-arm64\StopSnooze.exe SHA512 | tee Publish\win-arm64\NoSnooze.exe.sha512
certutil -hashfile Publish\win-x64\StopSnooze.exe SHA512 | tee Publish\win-x64\NoSnooze.exe.sha512

Compress-Archive -Path "Publish\win-arm64\*" -DestinationPath "Publish\win-arm64\StopSnooze_$publishversion`_win-arm64.zip"
Compress-Archive -Path "Publish\win-x64\*" -DestinationPath "Publish\win-x64\StopSnooze_$publishversion`_win-x64.zip"
