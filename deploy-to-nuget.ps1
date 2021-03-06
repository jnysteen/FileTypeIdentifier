param(
    [string] $nugetApiKey = "",
    [string] $nugetEndpoint = "https://api.nuget.org/v3/index.json"
)

Write-Output "Creating NuGet package..."
$nugetPackOuput = dotnet pack .\JNysteen.FileTypeIdentifier\JNysteen.FileTypeIdentifier.csproj --configuration=Release --output=createdNugetPackages

if($LASTEXITCODE -eq 0)
{
    Write-Host "NuGet pack was successful!"
} 
else 
{
    Write-Host "NuGet pack failed"
    exit
}

# Retrieve the package location from the NuGet pack output
$nugetPackOutputLines = ($nugetPackOuput -split '\n')
$lastNugetPackLine = $nugetPackOutputLines[$nugetPackOutputLines.Length-1];
$lastNugetPackLine -match ".*Successfully created package '(?<nugetPackage>.*)'"
$createdPackage = $Matches.nugetPackage

Write-Output "Created package: $createdPackage"

Write-Output "API key: $nugetApiKey"
Write-Output "NuGet endpoint: $nugetEndpoint"

$confirmation = Read-Host "Push package to NuGet? [y/n]"
while($confirmation -ne "y")
{
    if ($confirmation -eq 'n') {
        exit
    }
    $confirmation = Read-Host "Push package to NuGet? [y/n]"
}

$createdPackage

Write-Output "Pushing package to $nugetEndpoint!"
dotnet nuget push $createdPackage -k $nugetApiKey -s $nugetEndpoint

if($LASTEXITCODE -eq 0)
{
    Write-Host "NuGet publish was successful!"
} 
else 
{
    Write-Host "NuGet publish failed"
}