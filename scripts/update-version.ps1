# PowerShell script to update version
param(
    [Parameter(Mandatory=$true)]
    [string]$NewVersion,
    [string]$PreRelease = ""
)

$ErrorActionPreference = "Stop"

Write-Host "Updating version to $NewVersion" -ForegroundColor Green

# Parse version parts
$versionParts = $NewVersion.Split('.')
if ($versionParts.Length -ne 3) {
    throw "Version must be in format Major.Minor.Patch (e.g., 1.2.3)"
}

$major = $versionParts[0]
$minor = $versionParts[1]
$patch = $versionParts[2]

# Update Directory.Build.props
$buildPropsPath = "$PSScriptRoot\..\Directory.Build.props"
if (Test-Path $buildPropsPath) {
    Write-Host "Updating Directory.Build.props..." -ForegroundColor Yellow
    
    $content = Get-Content $buildPropsPath -Raw
    $content = $content -replace '<MajorVersion>\d+</MajorVersion>', "<MajorVersion>$major</MajorVersion>"
    $content = $content -replace '<MinorVersion>\d+</MinorVersion>', "<MinorVersion>$minor</MinorVersion>"
    $content = $content -replace '<PatchVersion>\d+</PatchVersion>', "<PatchVersion>$patch</PatchVersion>"
    $content = $content -replace '<PreReleaseLabel>.*?</PreReleaseLabel>', "<PreReleaseLabel>$PreRelease</PreReleaseLabel>"
    
    Set-Content $buildPropsPath $content -NoNewline
    Write-Host "✅ Directory.Build.props updated" -ForegroundColor Green
}

# Create git tag
$tagName = if ($PreRelease) { "v$NewVersion-$PreRelease" } else { "v$NewVersion" }
Write-Host "Creating git tag: $tagName" -ForegroundColor Yellow

git tag -a $tagName -m "Release $tagName"
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Git tag created: $tagName" -ForegroundColor Green
    Write-Host "💡 Push with: git push origin $tagName" -ForegroundColor Cyan
} else {
    Write-Warning "Failed to create git tag"
}

Write-Host "🎉 Version update complete!" -ForegroundColor Green
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host "  1. dotnet build" -ForegroundColor White
Write-Host "  2. dotnet pack" -ForegroundColor White
Write-Host "  3. git push origin $tagName" -ForegroundColor White
